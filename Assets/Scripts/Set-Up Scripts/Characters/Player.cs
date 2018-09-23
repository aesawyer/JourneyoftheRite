using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="TextAdventure/Player")]
public class Player : ScriptableObject {

    [HideInInspector] public string characterName = "player";
    public State state;
    public Title title;
    public BaseStats stats;
    public List<BaseObject> inventory;
    public int gold;
    public BaseQuest[] quests;
    [HideInInspector] public BaseCharacter[] party;
    [HideInInspector] public BaseCharacter target;
    [HideInInspector] public int threat = 15;
}
