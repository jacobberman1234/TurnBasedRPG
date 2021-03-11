using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _questUI;

    public List<Quest> Quests;
    public void AddQuest(Quest quest) => Quests.Add(quest);
    public void RemoveQuest(Quest quest) => Quests.Remove(quest);

    void Start()
    {
        _questUI = QuestLogManager.instance.gameObject; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ToggleQuestLog();
        if (Quests.Count > 0 && !TutorialManager.instance.QuestLogTutorialShown)
        {
            TutorialManager.instance.ShowQuestLogTutorial();
        }
    }

    void ToggleQuestLog()
    {
        if (TutorialManager.instance.gameObject.activeSelf && TutorialManager.instance.QuestLogTutorialShown)
            TutorialManager.instance.gameObject.SetActive(false);
        QuestLogManager.instance.ChangeQuestButtonInfo();
        _questUI.SetActive(!_questUI.activeSelf);
    }
}
