using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManagers : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _managersPrefab;
    [SerializeField] GameObject _uiPrefab;

    private void Awake()
    {
        var player = FindObjectOfType<Player>();
        player.GetComponent<CharacterStats>().ConfigureStats();
        GameObject manager = GameObject.FindGameObjectWithTag(Helper.MANAGERS_TAG);
        GameObject ui = GameObject.FindGameObjectWithTag(Helper.UI_TAG);
        if (manager != null && ui != null)
        {
            Destroy(gameObject);
            return;
        }
        if (ui != null)
        {
            Instantiate(_managersPrefab);
            Destroy(gameObject);
            return;
        }
        Instantiate(_managersPrefab);
        Instantiate(_uiPrefab);
        Destroy(this.gameObject);
    }
}
