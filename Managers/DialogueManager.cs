using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("References")]
    [SerializeField] UIManager _uiManager;
    [SerializeField] Queue<string> _sentences;
    [SerializeField] DialogueUI _dialogueUI;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _dialogueText;

    void Start()
    {
        _sentences = new Queue<string>();
        _uiManager = UIManager.instance;
    }

    public void StartDialogue(Dialogue dialogue, string name)
    {
        SoundManager.instance.InteractWithNPC();
        if (!_dialogueUI.gameObject.activeSelf)
            _dialogueUI.gameObject.SetActive(true);
        _uiManager.ToggleInteractTextUI(false);
        _dialogueUI.Open();
        _nameText.text = name;
        _sentences.Clear();
        print("Cleared sentences");
        foreach(var sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(_sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }
        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        _dialogueUI.Close();
        _uiManager.ToggleInteractTextUI(true, _nameText.text);
    }
}
