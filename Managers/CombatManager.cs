using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] ExitCombatManager _exitCombatManager;
    [SerializeField] SpawnManager _spawnManager;

    [Header("Player")]
    public Vector3 PlayerPositionOutOfCombat;
    public List<GameObject> PlayerPrefabs = new List<GameObject>();
    [SerializeField] GameObject[] cachedPlayerPrefabs;

    [Header("Enemy")]
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    [SerializeField] GameObject[] cachedEnemyPrefabs;

    [Header("Combat")]
    public List<PlayerUseAbility> ActivePlayers = new List<PlayerUseAbility>();
    public List<Enemy> ActiveEnemies = new List<Enemy>();
    [SerializeField] CombatAbilityDisplay _playerAbilityUI;
    public List<ScriptableObjectAbility> CachedAbilities = new List<ScriptableObjectAbility>();
    public bool SelectionPhase;
    public bool CharacterAttacking;
    public bool InCombat;
    [SerializeField] string previousScene;

    void Start()
    {
        _playerAbilityUI = FindObjectOfType<CombatAbilityDisplay>();
    }

    void Update()
    {
        ManageCombatPhases();
        //Tests
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KillAllEnemies();
        }
        TestResetPlayerPrefs();

    }
    //Test
    void TestResetPlayerPrefs()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void KillAllEnemies()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
            enemy.GetComponent<CharacterStats>().Kill();
    }

    #region EnteringCombat

    public void EnterCombat(Collider enemyTrigger)
    {
        BuildPlayerAndEnemyLists(enemyTrigger);
        //Change The Scene
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name + "_combat");
        SoundManager.instance.ToggleCombatMusic(true);
        InCombat = true;
        //Spawn Characters
        _spawnManager.enabled = true;
    }

    #endregion

    public void CheckIfAllEnemiesDead()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            if(!enemy.GetComponent<CharacterStats>().IsDead) { return; }
        }
        print("All Enemies Dead.");
        ExitCombat();
    }

    #region ExitingCombat
    public void ExitCombat()
    {
        InCombat = false;
        CharacterAttacking = false;
        ClearCachedData();
        SceneManager.LoadScene(previousScene);
        SoundManager.instance.ToggleCombatMusic(false);
        _spawnManager.enabled = true;
        _exitCombatManager.enabled = true;
    }

    #endregion

    void BuildPlayerAndEnemyLists(Collider enemyTrigger = null)
    {
        //Build Cross-Scene List of Player Objects
        var buildPlayerList = FindObjectsOfType<Player>();
        PlayerPositionOutOfCombat = buildPlayerList[0].transform.position;
        cachedPlayerPrefabs = new GameObject[buildPlayerList.Length];
        for (int i = 0; i < buildPlayerList.Length; i++)
        {
            cachedPlayerPrefabs[i] = Instantiate(buildPlayerList[i].gameObject, transform);
            PlayerPrefabs.Add(cachedPlayerPrefabs[i]);
            if (PlayerPrefabs[i].GetComponent<PlayerCharacterController>() != null)
            {
                PlayerPrefabs[i].GetComponent<PlayerCharacterController>().Moving = false;
                PlayerPrefabs[i].GetComponent<PlayerCharacterController>().enabled = false;
            }
            PlayerPrefabs[i].SetActive(false);
        }
        //Build Cross-Scene List of Enemy Objects

        //Using Trigger
        if (enemyTrigger != null)
        {
            int numberOfEnemies = enemyTrigger.transform.childCount;
            if (numberOfEnemies <= 0) { Debug.Log("No enemies added to List."); return; }
            cachedEnemyPrefabs = new GameObject[numberOfEnemies];
            for (int i = 0; i < numberOfEnemies; i++)
            {
                cachedEnemyPrefabs[i] = Instantiate(enemyTrigger.transform.GetChild(i).gameObject, transform);
                EnemyPrefabs.Add(cachedEnemyPrefabs[i]);
                EnemyPrefabs[i].SetActive(false);
            }
        }
        //Using Event
        else
        {
            return;
        }
    }

    public void ClearCachedData()
    {
        foreach (var prefab in cachedPlayerPrefabs)
            Destroy(prefab);
        foreach (var prefab in cachedEnemyPrefabs)
            Destroy(prefab);
        cachedPlayerPrefabs = null;
        cachedEnemyPrefabs = null;
        PlayerPrefabs.Clear();
        EnemyPrefabs.Clear();
        ActivePlayers.Clear();
        ActiveEnemies.Clear();
        CachedAbilities.Clear();
        _spawnManager.PlayerSpawnPointsList.Clear();
        _spawnManager.EnemySpawnPointsList.Clear();
        _spawnManager.PlayersSpawned.Clear();
        _spawnManager.EnemiesSpawned.Clear();
    }

    #region Combat

    void ManageCombatPhases()
    {
        if (!InCombat)
        {
            if (_playerAbilityUI.gameObject.activeSelf)
            {
                print("Deactivating player UI");
                _playerAbilityUI.gameObject.SetActive(false);
            }
            return;
        }

        int deadPlayers = 0;
        //Check to see if all players are dead
        foreach(var player in _spawnManager.PlayersSpawned)
        {
            if (player.GetComponent<CharacterStats>().IsDead)
                deadPlayers++;
            if (deadPlayers == _spawnManager.PlayersSpawned.Count)
            {
                print($"All players are dead.");
                return;
            }
        }



        //Check for Selection Phase
        if (CachedAbilities.Count <= 0)
        {
            SelectionPhase = true;
            _playerAbilityUI.gameObject.SetActive(true);
        }

        if (SelectionPhase && ActiveEnemies.Count > 0)
        {
            int numberOfPlayers = 0;
            foreach(var enemy in ActiveEnemies)
            {
                if (!enemy.AbilitySelected)
                {
                    var ability = enemy.SelectAbility();
                    if (ability == null) { enemy.AbilitySelected = false; return; }
                    CachedAbilities.Add(ability);
                }
            }
            foreach (var player in ActivePlayers)
            {
                if (player.AbilitySelected)
                {
                    CachedAbilities.Add(player.SelectedAbility);
                    player.AbilitySelected = false;
                    numberOfPlayers++;
                }
            }

            if(numberOfPlayers >= ActivePlayers.Count)
            {
                CachedAbilities = CachedAbilities.OrderBy(ability => ability.abilitySpeed).ToList();
                SelectionPhase = false;
            }
        }
        else
        {
            if (_playerAbilityUI.gameObject.activeSelf)
                _playerAbilityUI.gameObject.SetActive(false);
            if (CharacterAttacking)
                return;
            foreach(var player in ActivePlayers)
            {
                if (player.SelectedAbility == CachedAbilities[0])
                {
                    player.AttackTarget();
                    CachedAbilities.RemoveAt(0);
                    return;
                }
            }
            foreach (var enemy in ActiveEnemies)
            {
                if (enemy.SelectedAbility == CachedAbilities[0])
                {
                    enemy.AttackTarget();
                    CachedAbilities.RemoveAt(0);
                    return;
                }
            }
        }
    }

    public void BuildActivePlayerAndEnemyLists()
    {
        if (ActivePlayers.Count <= 0)
        {
            var players = FindObjectsOfType<PlayerUseAbility>();
            foreach (var player in players)
                ActivePlayers.Add(player);
        }

        if (ActiveEnemies.Count <= 0)
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
                ActiveEnemies.Add(enemy);
        }
    }

    #endregion
}
