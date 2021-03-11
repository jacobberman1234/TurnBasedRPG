using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogManager : MonoBehaviour
{
    public static QuestLogManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] GameObject[] _questButtons;
    [SerializeField] TMP_Text[] _questButtonText;
    [SerializeField] TMP_Text _questNameText;
    [SerializeField] TMP_Text _questDescriptionText;
    [SerializeField] GameObject _questDescriptionObject;
    [SerializeField] GameObject _noQuestText;
    [SerializeField] Player _player;

    List<Quest> _activeQuests = new List<Quest>();

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ChangeQuestButtonInfo()
    {
        _player = FindObjectOfType<Player>();
        _activeQuests = _player.GetComponent<QuestLog>().Quests;
        var quests = _activeQuests;
        _questDescriptionObject.SetActive(false);
        foreach (var button in _questButtons)
            button.SetActive(false);
        if (quests.Count <= 0)
        {
            _noQuestText.SetActive(true);
            return;
        }
        _noQuestText.SetActive(false);
        for (int i = 0; i < quests.Count; i++)
        {
            _questButtonText[i].text = quests[i].name;
            _questButtons[i].SetActive(true);
        }
    }

    public void DisplayQuestDetail(int i)
    {
        _questDescriptionObject.SetActive(true);
        _questNameText.text = _activeQuests[i].name;
        _questDescriptionText.text = _activeQuests[i].description;
    }
}
