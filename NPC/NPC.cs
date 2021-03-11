using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] new string name;

    [Header("References")]
    [SerializeField] DialogueManager _dialogueManager;
    [SerializeField] List<Dialogue> _onQuestDialogues = new List<Dialogue>();
    [SerializeField] List<Dialogue> _offQuestDialogues = new List<Dialogue>();
    [SerializeField] bool _onQuest;
    [SerializeField] GameObject _player;

    List<Dialogue> _currentDialogue = new List<Dialogue>();

    int _offQuestIndex;

    [Header("Settings")]
    public List<Quest> Quests = new List<Quest>();
    [SerializeField] int npcID;

    void Start()
    {
        _dialogueManager = DialogueManager.instance;
        _offQuestIndex = 0;
    }

    public void Interact()
    {
        var questLog = _player.GetComponent<QuestLog>();
        for (int i = 0; i < questLog.Quests.Count; i++)
            if (questLog.Quests[i].name == Quests[i].name)
            {
                if (questLog.Quests[i].isComplete)
                    _offQuestDialogues.RemoveAt(0);
            }
        SelectDialogueSequence();
        TriggerDialogue();
        if (_onQuest)
            return;
        if(Quests.Count > 0)
        {
            questLog.AddQuest(Quests[0]);
            _onQuest = true;
            _offQuestDialogues.RemoveAt(0);
            _offQuestIndex++;
            PlayerPrefs.SetInt("OffQuestIndex", _offQuestIndex);
        }

    }

    void TriggerDialogue()
    {
        foreach (var quest in Quests)
            if (quest.isComplete)
                TurnInQuest(quest);
        _dialogueManager.StartDialogue(_currentDialogue[0], name);
    }

    void TurnInQuest(Quest quest)
    {
        Quests.Remove(quest);
        _player.GetComponent<QuestLog>().RemoveQuest(quest);
        _onQuestDialogues.RemoveAt(0);
        _onQuest = false;
    }

    void SelectDialogueSequence()
    {
        if (Quests.Count > 0)
            foreach (var quest in _player.GetComponent<QuestLog>().Quests)
                if (quest == Quests[0])
                    _onQuest = true;
        if (_onQuest)
            _currentDialogue = _onQuestDialogues;
        else
            _currentDialogue = _offQuestDialogues;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helper.PLAYER_TAG))
        {
            _player = other.gameObject;
            if(_player != null)
            {
                _player.GetComponent<PlayerInteract>().InRange = true;
                _player.GetComponent<PlayerInteract>().Interactable = this;
            }

            UIManager.instance.ToggleInteractTextUI(true, name);
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
}
