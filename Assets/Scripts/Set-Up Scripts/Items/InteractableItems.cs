using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{

    GameController controller;

    void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void ExamineObject(string actionVerb, BaseObject target)
    {
        controller.ClearLoggedText();
        controller.LogStringWithReturn(target.examineDescription);
        controller.DisplayRoomText();
        controller.DisplayPhoto();
    }

    public void TakeObject(string actionVerb, BaseObject target, int position)
    {
        controller.playerController.player.inventory.Add(controller.roomNavigation.currentRoom.objectsInRoom[position]);
        controller.roomNavigation.currentRoom.objectsInRoom.Remove(controller.roomNavigation.currentRoom.objectsInRoom[position]);
        controller.ClearLoggedText();
        controller.LogStringWithReturn("You " + actionVerb + " the " + target.noun);
        controller.DisplayRoomText();
        controller.DisplayPhoto();
    }

    public void UseObject(string[] seperatedInputWords, string actionVerb, BaseObject target) {

        for (int u = 0; u < controller.playerController.player.inventory.Count; u++) {

            string passInObject = controller.playerController.player.inventory[u].noun;
            for (int invObj = 1; invObj < seperatedInputWords.Length; invObj++) {

                string inventoryObject = seperatedInputWords[invObj];
                if (passInObject == inventoryObject) {

                    string passInTest = target.useFunction.passInObject.noun;
                    if (passInTest == passInObject) {
                        controller.ClearLoggedText();
                        controller.LogStringWithReturn("    " + target.useFunction.actionDescription);
                        controller.DisplayRoomText();
                        controller.DisplayPhoto();
                        //--------------------------------------------------
                        string methodName = target.useFunction.methodName;
                        controller.actionResponse.inventoryObject = controller.playerController.player.inventory[u];
                        controller.actionResponse.targetObject = target;
                        controller.actionResponse.DetermineMethod(controller, methodName);
                    } else if ((u == controller.playerController.player.inventory.Count-1) && (invObj == seperatedInputWords.Length-1)) {

                        controller.ClearLoggedText();
                        controller.LogStringWithReturn("    What black magic is that?");
                        controller.DisplayRoomText();
                        controller.DisplayPhoto();
                    }
                }
            }
        }
    }

    public void EquipObject(string actionVerb, BaseObject target) {
        target.equipped = true;
        controller.ClearLoggedText();
        controller.LogStringWithReturn("    You equip the " + target.noun);
        controller.DisplayRoomText();
        controller.DisplayPhoto();
    }
}