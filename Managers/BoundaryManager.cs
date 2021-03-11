using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpawnManager _spawnManager;

    [Header("Player")]
    public List<GameObject> PlayerPrefabs = new List<GameObject>();
    [SerializeField] GameObject[] _cachedPlayerPrefabs;

    [Header("Timer")]
    public bool Ready;
    static float _refreshTime = 3f;
    float _timer;
    bool _countdownStarted;

    void OnEnable() => SceneManager.sceneLoaded += RefreshSceneLoadTime;
    void OnDisable() => SceneManager.sceneLoaded -= RefreshSceneLoadTime;

    void Start()
    {
        Ready = true;
    }

    private void Update()
    {
        if (_countdownStarted)
            StartCountdown();
    }

    void RefreshSceneLoadTime(Scene scene, LoadSceneMode mode)
    {
        Ready = false;
        if (!Ready && !_countdownStarted)
            _countdownStarted = true;
    }

    void StartCountdown()
    {
        _timer += Time.deltaTime;

        if (_timer >= _refreshTime)
        {
            Ready = true;
            _timer = 0;
            _countdownStarted = false;
        }
    }

    public void BuildPlayerList()
    {
        var buildPlayerList = FindObjectsOfType<Player>();
        if (_cachedPlayerPrefabs.Length > buildPlayerList.Length)
            _cachedPlayerPrefabs = null;
        if (PlayerPrefabs.Count > 0)
            PlayerPrefabs.Clear();
        _cachedPlayerPrefabs = new GameObject[buildPlayerList.Length];
        for (int i = 0; i < buildPlayerList.Length; i++)
        {
            _cachedPlayerPrefabs[i] = Instantiate(buildPlayerList[i].gameObject, transform);
            PlayerPrefabs.Add(_cachedPlayerPrefabs[i]);
            PlayerPrefabs[i].SetActive(false);
        }
    }
}
