using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerUseAbility _playerAbility;

    void Start()
    {
        var root = transform.parent;
        _playerAbility = root.GetComponent<PlayerUseAbility>();

    }

    public void UseAbility()
    {
        _playerAbility.PerformAbility();
    }

    public void MoveToEnemyPosition()
    {
        _playerAbility.MovePosition(true);
    }

    public void MoveToOriginalPosition()
    {
        _playerAbility.MovePosition(false);
    }

    public void DoneAttacking()
    {
        _playerAbility.CharacterDoneAttacking();
    }
}
