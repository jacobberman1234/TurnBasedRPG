using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _inventoryUI;

    public List<ScriptableObjectItem> Items = new List<ScriptableObjectItem>();

    void Start()
    {
        _inventoryUI = InventoryManager.instance.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ToggleInventory();
    }

    void ToggleInventory()
    {
        if (TutorialManager.instance.gameObject.activeSelf && TutorialManager.instance.QuestLogTutorialShown)
            TutorialManager.instance.gameObject.SetActive(false);
        UpdateInventoryInfo();
        _inventoryUI.SetActive(!_inventoryUI.activeSelf);
    }

    public void UpdateInventoryInfo()
    {
        InventoryManager.instance.ChangeInventoryInfo(Items);
    }

    public void UseItem(int slot)
    {
        if (Items[slot].isWeapon)
            EquipItem(slot);
        else
            print("Can't equip this");
            
    }

    void EquipItem(int slot)
    {
        Destroy(Items[slot]);
        UpdateInventoryInfo();
    }
}
