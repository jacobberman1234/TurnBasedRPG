using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability")]
public class ScriptableObjectAbility : ScriptableObject
{
    public new string name;
    public int minDamage;
    public int maxDamage;
    public float abilitySpeed;
    public bool stationary;
    public float animationSpeed;
    public StatEnum statAffected;
}

public enum StatEnum { HEALTH, ENERGY, DEFENSE, ATTACK }
