using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public bool isStarted;
    public new string name;
    public string description;
    public int experienceReward;
    public bool isComplete;
    public int questID;
}
