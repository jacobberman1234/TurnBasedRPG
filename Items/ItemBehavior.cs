using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] ScriptableObjectItem _itemDetails;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _itemPrefab;

    public void Interact()
    {
        AddToInventory();
    }

    void AddToInventory()
    {
        if (_player == null)
            return;
        this.enabled = false;
        if (_itemDetails.isWeapon)
        {
            EquipWeapon(_player);
            return;
        }
        _player.GetComponent<Inventory>().Items.Add(this._itemDetails);
        _player.GetComponent<Inventory>().UpdateInventoryInfo();
        if (_itemDetails.quest != null)
            _itemDetails.quest.isComplete = true;
        UIManager.instance.ToggleInteractTextUI(false);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helper.PLAYER_TAG))
        {
            _player = other.gameObject;
            if (_player != null)
            {
                _player.GetComponent<PlayerInteract>().InRange = true;
                _player.GetComponent<PlayerInteract>().Interactable = this;
            }

            UIManager.instance.ToggleInteractTextUI(true, _itemDetails.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Helper.PLAYER_TAG))
        {
            _player.GetComponent<PlayerInteract>().InRange = false;
            _player.GetComponent<PlayerInteract>().Interactable = null;
            _player = null;
            UIManager.instance.ToggleInteractTextUI(false);
        }
    }

    void EquipWeapon(GameObject player)
    {
        GameObject equipSlot = player.GetComponentInChildren<EquipSlot>().gameObject;
        Instantiate(_itemPrefab, equipSlot.transform);
        UIManager.instance.ToggleInteractTextUI(false);
        Destroy(gameObject);
    }
}
