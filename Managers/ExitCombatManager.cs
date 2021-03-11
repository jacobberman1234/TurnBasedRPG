using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCombatManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;

    void OnEnable() => SceneManager.sceneLoaded += RepositionPlayer;
    void OnDisable() => SceneManager.sceneLoaded -= RepositionPlayer;

    void RepositionPlayer(Scene scene, LoadSceneMode mode)
    {
        var players = FindObjectsOfType<Player>();
        foreach(var player in players)
        {
            if (player.CompareTag(Helper.PLAYER_TAG))
                player.transform.position = _combatManager.PlayerPositionOutOfCombat;
        }
        this.enabled = false;
    }
}
