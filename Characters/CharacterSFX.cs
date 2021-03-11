using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSFX : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;

    private void Start()
    {
        _soundManager = SoundManager.instance;
    }

    public void PlayFootstep()
    {
        _soundManager.PlayFootstep();
    }
}
