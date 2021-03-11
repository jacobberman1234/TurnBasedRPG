using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;
    [SerializeField] SpawnManager _spawnManager;
    [SerializeField] CharacterAbilities _abilities;
    [SerializeField] CharacterStats _stats;
    [SerializeField] CharacterAnimation _animation;
    [SerializeField] Renderer _renderer;
    [SerializeField] Shader _regularShader;
    [SerializeField] Shader _selectedShader;

    [Header("Settings")]
    [SerializeField] Vector3 _offset = new Vector3(-1, 0, 0);

    bool _isDefeated;

    public float ENEMY_ID;
    public bool IsSelected;
    public GameObject SelectedTarget;
    public ScriptableObjectAbility SelectedAbility;
    public bool AbilitySelected;

    public Vector3 DefaultPosition;

    void Awake() => IsSelected = false;

    void Start()
    {
        if (PlayerPrefs.GetInt($"Enemy{ENEMY_ID}-IsDefeated") == 1)
            _isDefeated = true;

        if (_isDefeated)
            Destroy(transform.parent.gameObject);
        _renderer = GetComponentInChildren<Renderer>();
        _regularShader = Shader.Find("Universal Render Pipeline/Lit");
        _selectedShader = Shader.Find("Shader Graphs/SelectedEnemy");
        _combatManager = FindObjectOfType<CombatManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _abilities = GetComponent<CharacterAbilities>();
        _stats = GetComponent<CharacterStats>();
        _stats.ConfigureStats();
        _animation = GetComponent<CharacterAnimation>();
        AbilitySelected = false;
    }

    void Update()
    {
        GetSelected();
    }

    void GetSelected()
    {
        if (IsSelected)
            _renderer.material.shader = _regularShader;
        else
            _renderer.material.shader = _regularShader;
    }

    #region CombatLoop

    public ScriptableObjectAbility SelectAbility()
    {
        SelectedAbility = null;
        SelectedTarget = null;
        int randomTargetIndex = Random.Range(0, _combatManager.ActivePlayers.Count);
        SelectedTarget = _combatManager.ActivePlayers[randomTargetIndex].gameObject;
        int randomAbilityIndex = Random.Range(0, _abilities.Abilities.Length);
        AbilitySelected = true;
        return SelectedAbility = _abilities.Abilities[randomAbilityIndex]; ;
    }

    public void AttackTarget()
    {
        if (!_stats.IsDead)
        {
            //Look at Target
            //PlayAttack Animation
            _animation.PlayAttackAnimation(SelectedAbility.name);
            AbilitySelected = false;
            _combatManager.CharacterAttacking = true;
        }
    }

    public void PerformAbility()
    {
        var targetStats = SelectedTarget.GetComponent<CharacterStats>();
        int statIndex = (int)SelectedAbility.statAffected;
        int damage = Random.Range(SelectedAbility.minDamage, SelectedAbility.maxDamage + 1);
        float attackMultiplier = 1 + _stats.CurrentStats[4] / 10;
        float damageForAttack = damage * attackMultiplier;
        damage = Mathf.RoundToInt(damageForAttack);
        targetStats.DamageOrBuffStat(statIndex, damage);
        print($"{gameObject.name} attacked {SelectedTarget.name} with {SelectedAbility.name} for {damage} damage.");
    }

    public void MovePosition(bool toPlayer)
    {
        if (toPlayer)
        {
            DefaultPosition = transform.position;
            var newPosition = new Vector3(SelectedTarget.transform.position.x, transform.position.y, SelectedTarget.transform.position.z);
            transform.position = newPosition - _offset;
        }
        else
        {
            transform.position = DefaultPosition;
        }
    }

    public void CharacterDoneAttacking()
    {
        _combatManager.CharacterAttacking = false;
    }

    #endregion
}
