using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject[] _uiPanels;

    void Awake()
    {
        instance = this;
        foreach (var panel in _uiPanels)
            panel.SetActive(true);
    }

    [Header("References")]
    [SerializeField] TMP_Text _interactText;

    public void ToggleInteractTextUI(bool value, string name = "")
    {
        GameObject interactUI = _interactText.gameObject;
        _interactText.text = $"Press E to interact with {name}";
        interactUI.SetActive(value);
    }
}
