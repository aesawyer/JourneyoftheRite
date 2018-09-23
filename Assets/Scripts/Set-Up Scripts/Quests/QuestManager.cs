/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    GameController controller;
    [HideInInspector] public BaseCharacter fetchGoal;
    [HideInInspector] public BaseObject fetchTarget;

    public void DetermineMethod(GameController cont, string methodName) {
        controller = cont;
        Invoke(methodName, 0);
    }

    public void FetchQuestCheck() {
        string goalString = fetchGoal.characterName;
        for (int i = 0; i < controller.roomNavigation.currentRoom.charactersInRoom.Length; i++) {
            string checkChar = controller.roomNavigation.currentRoom.charactersInRoom[i].characterName;
            if (goalString == checkChar) {
                if (controller.playerController.player.inventory.Contains(fetchTarget)) {
                    FetchQuestComplete();
                    return;
                }
            }
        }
    }

    void FetchQuestComplete() {
        controller.playerController.player.inventory.Remove(fetchTarget);
        fetchGoal.inventory.Add(fetchTarget);
    }

}
