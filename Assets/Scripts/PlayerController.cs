/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Player player;
    public GameController controller;
    [HideInInspector] public QuestManager questManager;
    //---------------------------------------------

    private void Awake() {
        SecondaryStatCheck();
    }

    void SecondaryStatCheck() {
        player.stats.fight = ((player.stats.strength + player.stats.endurance) / 2);
        player.stats.speed = ((player.stats.strength + player.stats.grace) / 2);
        player.stats.stealth = ((player.stats.dexterity + player.stats.charisma) / 2);
        player.stats.dodge = ((player.stats.dexterity + player.stats.grace) / 2);
        player.stats.will = ((player.stats.intelligence + player.stats.endurance) / 2);
        player.stats.perception = ((player.stats.intelligence + player.stats.charisma) / 2);
        player.stats.maxHealth = player.stats.strength;
        player.stats.maxEnergy = player.stats.intelligence;
        player.stats.damage = player.stats.strength;
        /*
        Debug.Log("Fight: " + player.stats.fight);
        Debug.Log("Speed: " + player.stats.speed);
        Debug.Log("Stealth: " + player.stats.stealth);
        Debug.Log("Dodge: " + player.stats.dodge);
        Debug.Log("Will: " + player.stats.will);
        Debug.Log("Perception: " + player.stats.perception);
        Debug.Log("Health: " + player.stats.maxHealth);
        Debug.Log("Energy: " + player.stats.maxEnergy);
        */
    }

    void CheckQuests() {
        BaseQuest quest;
        string methodSource;
        string questCompleteMethod;
        for (int q = 0; q < player.quests.Length; q++) {
            quest = player.quests[q];
            //Check Quest Type
            switch (quest.type.ToString()) {
                case "fetch":
                    methodSource = "fetch";
                    questManager.fetchGoal = quest.fetchgoal;
                    questManager.fetchTarget = quest.fetchTarget;
                    break;
                case "kill":
                    methodSource = "kill";
                    break;
                case "explore":
                    methodSource = "explore";
                    break;
                case "gather":
                    methodSource = "gather";
                    break;
                case "defend":
                    methodSource = "defend";
                    break;
                default:
                    methodSource = "";
                    break;
            }
            //Runs a check for each Quest for completion
            questCompleteMethod = methodSource + "QuestCheck";
            questManager.DetermineMethod(controller, questCompleteMethod);
        }

    }
}
