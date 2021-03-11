using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;
    [SerializeField] PlayerCharacterController _characterController;
    [SerializeField] Animator _animator;

    bool _isPlayer;

    void Start()
    {
        _combatManager = CombatManager.instance;
        if (GetComponent<Player>())
            _isPlayer = true;
        if (_isPlayer)
            _characterController = GetComponent<PlayerCharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(_isPlayer)
            _animator.SetBool("moving", _characterController.Moving);

        //InCombat
        if (_combatManager.InCombat)
            _animator.SetBool("inCombat", true);
        else
            _animator.SetBool("inCombat", false);
    }

    public void PlayAttackAnimation(string attackName)
    {
        _animator.SetTrigger(attackName);
    }
}
