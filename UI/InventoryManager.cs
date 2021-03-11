using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] Image[] icons;
    [SerializeField] Player _player;

    void Start()
    {
        gameObject.SetActive(false);
        foreach(var icon in icons)
        {
            if (icon.sprite == null)
                icon.gameObject.SetActive(false);
        }
    }

    public void ChangeInventoryInfo(List<ScriptableObjectItem> items)
    {
        for(int i = 0; i < items.Count; i++)
        {
            icons[i].sprite = items[i].icon;
            icons[i].gameObject.SetActive(true);
        }
    }

    public void UseItem(int slot)
    {
        if (_player == null)
            _player = FindObjectOfType<Player>();
        var inventory = _player.GetComponent<Inventory>();
        inventory.UseItem(slot);
    }
}
