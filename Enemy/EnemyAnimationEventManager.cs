using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Enemy _enemyAbility;

    void Start()
    {
        var root = transform.parent;
        _enemyAbility = root.GetComponent<Enemy>();
    }

    public void UseAbility()
    {
        _enemyAbility.PerformAbility();
    }

    public void MoveToPlayerPosition()
    {
        _enemyAbility.MovePosition(true);
    }

    public void MoveToOriginalPosition()
    {
        _enemyAbility.MovePosition(false);
    }

    public void DoneAttacking()
    {
        _enemyAbility.CharacterDoneAttacking();
    }
}
