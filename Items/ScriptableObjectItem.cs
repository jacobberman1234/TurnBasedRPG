using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ScriptableObjectItem : ScriptableObject
{
    public new string name;
    public string description;
    public bool isWeapon;
    public bool isQuestItem;
    public Sprite icon;
    public Quest quest;
    public int itemID;
}
