using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _footsteps;
    [SerializeField] AudioClip _interactWithNPC;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        _audioSource.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
    }

    public void PlayInteractWithNPC()
    {
        _audioSource.PlayOneShot(_interactWithNPC);
    }
}
