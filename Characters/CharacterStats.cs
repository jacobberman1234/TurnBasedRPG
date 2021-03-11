using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;

    [Header("Stats")]
    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _maxEnergy = 100;
    [SerializeField] int _maxAccuracy = 100;
    [SerializeField] int _maxDefense = 100;
    [SerializeField] int _maxAttack = 100;
    public int[] CurrentStats = new int[5];

    public bool IsDead;

    void Start()
    {
        _combatManager = FindObjectOfType<CombatManager>();    
    }

    public void ConfigureStats()
    {
        CurrentStats[0] = _maxHealth;
        CurrentStats[1] = _maxEnergy;
        CurrentStats[2] = _maxAccuracy;
        CurrentStats[3] = _maxDefense;
        CurrentStats[4] = _maxAttack;
    }

    public void DamageOrBuffStat(int statIndex, int damage)
    {
        float defenseMultiplier = 1 + CurrentStats[3] / 10;
        if(CurrentStats[statIndex] <= 0)
            return;
        float damageDone = damage / defenseMultiplier;
        CurrentStats[statIndex] -= Mathf.RoundToInt(damageDone);

        print($"{this.name} current Health: {CurrentStats[0]}");

        if (CurrentStats[0] <= 0)
            Die();
    }

    void Die()
    {
        IsDead = true;
        print($"{this.name} has died.");
        if(GetComponent<Enemy>() == null) { return; }
        PlayerPrefs.SetInt($"Enemy{GetComponent<Enemy>().ENEMY_ID}-IsDefeated", 1);
        _combatManager.CheckIfAllEnemiesDead();
    }

    //Test
    public void Kill()
    {
        Die();
    }
}
