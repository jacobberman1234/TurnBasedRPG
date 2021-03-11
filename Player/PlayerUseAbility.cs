using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;
    [SerializeField] SpawnManager _spawnManager;
    [SerializeField] CharacterStats _stats;
    [SerializeField] CharacterAnimation _animation;

    [Header("Settings")]
    [SerializeField] Vector3 _offset = new Vector3(1, 0, 0);

    public GameObject SelectedTarget;
    int _selectedIndex, _previousSelectedIndex;
    public ScriptableObjectAbility SelectedAbility;
    public bool AbilitySelected;
    public Vector3 DefaultPosition;

    void Start()
    {
        _combatManager = CombatManager.instance;
        _spawnManager = SpawnManager.instance;
        _stats = GetComponent<CharacterStats>();
        _animation = GetComponent<CharacterAnimation>();
        _selectedIndex = 0;
        _previousSelectedIndex = _selectedIndex;
    }

    private void Update()
    {
        SelectTarget();
    }

    void SelectTarget()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _previousSelectedIndex = _selectedIndex;
            _selectedIndex--;
            if(_selectedIndex < 0)
            {
                _selectedIndex = _spawnManager.EnemiesSpawned.Count-1;
            }
            _spawnManager.EnemiesSpawned[_previousSelectedIndex].GetComponent<Enemy>().IsSelected = false;
            _spawnManager.EnemiesSpawned[_selectedIndex].GetComponent<Enemy>().IsSelected = true;

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _previousSelectedIndex = _selectedIndex;
            _selectedIndex++;
            if (_selectedIndex >= _spawnManager.EnemiesSpawned.Count)
            {
                _selectedIndex = 0;
            }
            _spawnManager.EnemiesSpawned[_previousSelectedIndex].GetComponent<Enemy>().IsSelected = false;
            _spawnManager.EnemiesSpawned[_selectedIndex].GetComponent<Enemy>().IsSelected = true;
        }

    }

    public void SelectAbility(ScriptableObjectAbility ability)
    {
        foreach (var enemy in _combatManager.ActiveEnemies)
        {
            if (enemy.IsSelected)
                SelectedTarget = enemy.gameObject;
        }
        SelectedAbility = ability;
        AbilitySelected = true;
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
        float attackMultiplier = _stats.CurrentStats[4];
        print($"Multiplier: {attackMultiplier}");
        float damageForAttack = damage * attackMultiplier;
        print($"Damage without multiplier: {damage}");
        damage = Mathf.RoundToInt(damageForAttack);
        print($"Damage with multiplier: {damageForAttack}");
        targetStats.DamageOrBuffStat(statIndex, damage);
        print($"{gameObject.name} attacked {SelectedTarget.name} with {SelectedAbility.name} for {damage} damage.");
        SelectedAbility = null;
        SelectedTarget = null;
    }

    public void MovePosition(bool toEnemy)
    {
        if (toEnemy)
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
}
