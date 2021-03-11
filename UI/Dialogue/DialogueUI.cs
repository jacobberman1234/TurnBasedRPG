using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _dialogueBox;
    [SerializeField] Button _continueButton;
    
    public TMP_Text NameText;
    public TMP_Text DialogueText;

    private void Start()
    {
        _dialogueBox.SetActive(true);
    }

    public void Open()
    {
        _dialogueBox.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

}
