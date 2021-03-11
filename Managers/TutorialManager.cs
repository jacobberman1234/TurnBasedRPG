using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    void Awake() => instance = this;

    [Header("References")]
    [SerializeField] GameObject _questLogTutorial;

    public bool QuestLogTutorialShown;

    private void Start()
    {
        _questLogTutorial.SetActive(false);
        if (PlayerPrefs.GetInt("QuestTutorialShown") == 1)
            QuestLogTutorialShown = true;
        else
            QuestLogTutorialShown = false;
    }

    public void ShowQuestLogTutorial()
    {
        if (!QuestLogTutorialShown)
        {
            _questLogTutorial.SetActive(true);
            QuestLogTutorialShown = true;
            PlayerPrefs.SetInt("QuestTutorialShown", 1);
        }
    }
}
