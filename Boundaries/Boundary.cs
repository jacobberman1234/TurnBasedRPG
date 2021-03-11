using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boundary : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoundaryManager _boundaryManager;
    [SerializeField] SpawnManager _spawnManager;

    [Header("Settings")]
    [SerializeField] string _sceneToLoad;

    private void Start()
    {
        _boundaryManager = FindObjectOfType<BoundaryManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helper.PLAYER_TAG))
            SwitchScene();
    }

    void SwitchScene()
    {
        if (_boundaryManager.Ready)
        {
            _boundaryManager.BuildPlayerList();
            SceneManager.LoadScene(_sceneToLoad);
            _boundaryManager.Ready = false;
            _spawnManager.enabled = true;
        }
    }
}
