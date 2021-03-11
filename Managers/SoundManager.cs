using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] Music _music;
    [SerializeField] SFX _sfx;

    void Start()
    {
        _music = GetComponentInChildren<Music>();
        _sfx = GetComponentInChildren<SFX>();
    }

    public void PlayFootstep()
    {
        _sfx.PlayFootstep();
    }

    public void InteractWithNPC()
    {
        _sfx.PlayInteractWithNPC();
    }

    public void ToggleCombatMusic(bool value)
    {
        if (value)
            _music.SwitchToBattleMusic();
        else
            _music.SwitchToDefaultMusic();
    }
}
