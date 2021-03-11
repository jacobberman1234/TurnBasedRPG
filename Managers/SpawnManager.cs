using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] CombatManager _combatManager;
    [SerializeField] BoundaryManager _boundaryManager;

    [Header("Player")]
    [SerializeField] PlayerSpawnPoint[] _playerSpawnPoints;
    public List<Transform> PlayerSpawnPointsList = new List<Transform>();
    public List<GameObject> PlayersSpawned = new List<GameObject>();

    [Header("Enemy")]
    [SerializeField] EnemySpawnPoint[] _enemySpawnPoints;
    public List<Transform> EnemySpawnPointsList = new List<Transform>();
    public List<GameObject> EnemiesSpawned = new List<GameObject>();

    void OnEnable() => SceneManager.sceneLoaded += Spawn;
    void OnDisable() => SceneManager.sceneLoaded -= Spawn;

    void Spawn(Scene scene, LoadSceneMode mode)
    {
        if (_combatManager.InCombat)
        {
            BuildCombatSpawnPointLists();
            ClearCharacterLists();
            SpawnCombatPrefabs();
            _combatManager.BuildActivePlayerAndEnemyLists();
        }
        else
        {
            BuildOutOfCombatSpawnPointLists();
            ClearCharacterLists();
            SpawnOutOfCombatPrefabs();
        }
        this.enabled = false;
    }

    void BuildCombatSpawnPointLists()
    {
        PlayerSpawnPointsList.Clear();
        EnemySpawnPointsList.Clear();
        _playerSpawnPoints = FindObjectsOfType<PlayerSpawnPoint>();
        _enemySpawnPoints = FindObjectsOfType<EnemySpawnPoint>();
        //Null Check
        if (_playerSpawnPoints.Length <= 0) { Debug.Log("No spawn points for players found."); return; }
        if (_enemySpawnPoints.Length <= 0) { Debug.Log("No spawn points for enemies found."); return; }
        //Build Lists
        foreach (var spawnPoint in _playerSpawnPoints)
            PlayerSpawnPointsList.Add(spawnPoint.transform);
        foreach (var spawnPoint in _enemySpawnPoints)
            EnemySpawnPointsList.Add(spawnPoint.transform);
        //Organize List Alphabetically
        PlayerSpawnPointsList = PlayerSpawnPointsList.OrderBy(go => go.name).ToList();
        EnemySpawnPointsList = EnemySpawnPointsList.OrderBy(go => go.name).ToList();
    }
    
    void BuildOutOfCombatSpawnPointLists()
    {
        PlayerSpawnPointsList.Clear();
        EnemySpawnPointsList.Clear();
        _playerSpawnPoints = FindObjectsOfType<PlayerSpawnPoint>();
        if (_playerSpawnPoints.Length <= 0) { Debug.Log("No spawn points for players found."); return; }
        foreach (var spawnPoint in _playerSpawnPoints)
            PlayerSpawnPointsList.Add(spawnPoint.transform);
        PlayerSpawnPointsList = PlayerSpawnPointsList.OrderBy(go => go.name).ToList();
    }

    void SpawnOutOfCombatPrefabs()
    {
        var players = FindObjectsOfType<Player>();
        if(players != null)
        {
            foreach (var player in players)
                Destroy(player.gameObject);
        }
        Quaternion directionToSpawnPlayer = Quaternion.Euler(0, 90, 0);
        var playerPrefabsToSpawn = _boundaryManager.PlayerPrefabs;
        for (int i = 0; i < playerPrefabsToSpawn.Count; i++)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefabsToSpawn[i], PlayerSpawnPointsList[i].position, directionToSpawnPlayer);
            PlayersSpawned.Add(spawnedPlayer);
            spawnedPlayer.SetActive(true);
        }
    }

    void SpawnCombatPrefabs()
    {
        Quaternion directionToSpawnPlayer = Quaternion.Euler(0, 90, 0);
        //Spawn players
        var playerPrefabsToSpawn = _combatManager.PlayerPrefabs;
        for (int i = 0; i < playerPrefabsToSpawn.Count; i++)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefabsToSpawn[i], PlayerSpawnPointsList[i].position, directionToSpawnPlayer);
            PlayersSpawned.Add(spawnedPlayer);
            spawnedPlayer.SetActive(true);
        }
        //Spawn enemies
        var enemyPrefabsToSpawn = _combatManager.EnemyPrefabs;
        for (int i = 0; i < enemyPrefabsToSpawn.Count; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefabsToSpawn[i], EnemySpawnPointsList[i].position, Quaternion.identity);
            EnemiesSpawned.Add(spawnedEnemy);
            spawnedEnemy.transform.LookAt(PlayersSpawned[0].transform);
            spawnedEnemy.SetActive(true);
        }
        EnemiesSpawned[0].GetComponent<Enemy>().IsSelected = true;
    }

    void ClearCharacterLists()
    {
        PlayersSpawned.Clear();
        EnemiesSpawned.Clear();
        _combatManager.ActiveEnemies.Clear();
        _combatManager.ActivePlayers.Clear();
    }
}
