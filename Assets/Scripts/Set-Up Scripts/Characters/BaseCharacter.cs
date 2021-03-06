using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Character")]
public class BaseCharacter : ScriptableObject {

    public string characterName;
    public Sprite portrait;
    public string[] alias;
    public Title title;
    public Home home;
    public bool stock;
    [HideInInspector] public bool randomized; //Unhide for testing
    public BaseStats stats;
    public List<BaseObject> inventory;
    public int gold;
    public BaseQuest quest;
    [HideInInspector] public CharacterState state;
    public string roomLocation;
    [TextArea]
    public string description;

    void OnEnable() {
        if ((stock == true) && (randomized == false)) {
            RandomizeStats();
            randomized = true;
        }
        stats.fight = ((stats.strength + stats.endurance) / 2);
        stats.speed = ((stats.strength + stats.grace) / 2);
        stats.stealth = ((stats.dexterity + stats.charisma) / 2);
        stats.dodge = ((stats.dexterity + stats.grace) / 2);
        stats.will = ((stats.intelligence + stats.endurance) / 2);
        stats.perception = ((stats.intelligence + stats.charisma) / 2);
        stats.maxHealth = stats.strength;
        stats.maxEnergy = stats.intelligence;
        stats.damage = stats.strength;
    }

    void RandomizeStats() {
        System.Random rnd = new System.Random();
        stats.strength = rnd.Next(1, 10);
        stats.intelligence = rnd.Next(1, 10);
        stats.dexterity = rnd.Next(1, 10);
        stats.charisma = rnd.Next(1, 10);
        stats.endurance = rnd.Next(1, 10);
        stats.grace = rnd.Next(1, 10);
    }
}