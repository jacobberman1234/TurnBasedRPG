using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _defaultMusic;
    [SerializeField] AudioClip _defaultBattleMusic;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _defaultMusic;
    }

    void ResetMusic()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void SwitchToDefaultMusic()
    {
        _audioSource.clip = _defaultMusic;
        ResetMusic();
    }

    public void SwitchToBattleMusic()
    {
        _audioSource.clip = _defaultBattleMusic;
        ResetMusic();
    }
}
