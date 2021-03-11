using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;

    private void Start()
    {
        _combatManager = GameObject.FindGameObjectWithTag(Helper.MANAGERS_TAG).GetComponentInChildren<CombatManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helper.ENEMY_TAG))
        {
            _combatManager.EnterCombat(other);
        }
    }
}
