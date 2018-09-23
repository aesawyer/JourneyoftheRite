/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour {

	public InputField inputField;
	GameController controller;
    
	void Awake () {
		controller = GetComponent <GameController> ();
        //Submit text to parse
        inputField.onEndEdit.AddListener(AcceptStringInput);
	}

    void OptionInput() {
        if (Input.GetKeyDown("1"))
        {
            controller.playerController.player.state = State.explore;
            controller.CheckGameState(controller.playerController.player.state);
        }
    }

    void AcceptStringInput(string userInput) {
        userInput = userInput.ToLower();
        char delimiterCharacters = ' ';
        string[] seperatedInputWords = userInput.Split(delimiterCharacters);
        string actionWord = seperatedInputWords[0];

        if ((actionWord == "?") || (actionWord == "")) {
            HelpCommand();
        } else {
            LibraryParser(seperatedInputWords, actionWord);
        }
        InputComplete();
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    void HelpCommand() {
        controller.ClearLoggedText();
        controller.LogStringWithReturn("?~~~~~~~~~~~~~~~~~~~~~? \n 1st word must be a verb\n try to examine the room\n?~~~~~~~~~~~~~~~~~~~~~?");
        controller.DisplayRoomText();
        controller.DisplayPhoto();
    }

    void LibraryParser(string[] seperatedInputWords, string actionWord) {
        Category libraryCheck;
        string verbCheck;
        for (int l = 0; l < controller.library.categories.Length; l++) {
            libraryCheck = controller.library.categories[l];
            for (int d = 0; d < libraryCheck.verbs.Length; d++) {
                verbCheck = libraryCheck.verbs[d];
                if (actionWord == verbCheck) {
                    LibraryActions(libraryCheck, seperatedInputWords, actionWord);
                    return;
                } else if ((l == controller.library.categories.Length-1) && (d == libraryCheck.verbs.Length-1)) {
                    SpecialParser(seperatedInputWords, actionWord);
                }
            }
        }
    }

    void LibraryActions (Category libraryCheck, string[] seperatedInputWords, string actionWord) {
        switch (libraryCheck.name) {
            case "movement":
                CheckMovement(seperatedInputWords, actionWord);
                break;
            case "examine":
                CheckExamine(seperatedInputWords, actionWord);
                break;
            case "take":
                CheckTake(seperatedInputWords, actionWord);
                break;
            case "use":
                CheckUse(seperatedInputWords, actionWord);
                break;
            case "attack":
                CheckAttack(seperatedInputWords, actionWord);
                break;
            case "talk":
                CheckSpeak(seperatedInputWords, actionWord);
                break;
            case "equip":
                CheckEquip(seperatedInputWords, actionWord);
                break;
        }
    }

    void SpecialParser(string[] seperatedInputWords, string actionWord) {
        for (int s = 0; s < controller.roomNavigation.currentRoom.objectsInRoom.Count; s++) {
            BaseObject roomObj = controller.roomNavigation.currentRoom.objectsInRoom[s];
            for (int od = 0; od < roomObj.specialInput.Length; od++) {
                for (int oc = 0; oc < roomObj.specialInput[od].dictionary.categories.Length; oc++) {
                    for (int ov = 0; ov < roomObj.specialInput[od].dictionary.categories[oc].verbs.Length; ov++) {
                        string objVerbCheck = roomObj.specialInput[od].dictionary.categories[oc].verbs[ov];
                        if (actionWord == objVerbCheck) {
                            controller.ClearLoggedText();
                            controller.LogStringWithReturn("    " + roomObj.specialInput[od].actionDescription);
                            //--------------------------------------------------
                            string methodName = roomObj.specialInput[od].methodName;
                            controller.actionResponse.targetObject = roomObj;
                            controller.actionResponse.DetermineMethod(controller, methodName);
                            return;
                        } else if (s == controller.roomNavigation.currentRoom.objectsInRoom.Count-1) {
                            if ((od == controller.roomNavigation.currentRoom.objectsInRoom[s].specialInput.Length-1) &&
                            (oc == controller.roomNavigation.currentRoom.objectsInRoom[s].specialInput[od].dictionary.categories.Length-1) &&
                            (ov == controller.roomNavigation.currentRoom.objectsInRoom[s].specialInput[od].dictionary.categories[oc].verbs.Length-1)) {
                                FailSpeach();
                            }
                        }
                    }
                }
            }
        }
    }

    void FailSpeach() {
        controller.ClearLoggedText();
        controller.LogStringWithReturn("    What black magic is that?");
        controller.DisplayRoomText();
        controller.DisplayPhoto();
    }
    //-----------------------------------------------------------------------------------------------------------------------------
	void CheckMovement(string[] seperatedInputWords, string actionWord) {
        for (int m2 = 0; m2 < seperatedInputWords.Length; m2++) {
			string exitString = seperatedInputWords [m2];

			for (int ex = 0; ex < controller.roomNavigation.currentRoom.exits.Length; ex++) {
				string exit = controller.roomNavigation.currentRoom.exits [ex].keyString;

				if (exit == exitString) {
					controller.roomNavigation.AttemptToChangeRooms (actionWord, exit);
                    return;
				} else if ((m2 == seperatedInputWords.Length-1) && (ex == controller.roomNavigation.currentRoom.exits.Length-1)) {
                    FailSpeach();
                }
			}
		}
	}

    void CheckTake(string[] seperatedInputWords, string actionWord) {
        for (int tak2 = 0; tak2 < seperatedInputWords.Length; tak2++) {
            string takeTargetString = seperatedInputWords[tak2];
            for (int taktar = 0; taktar < controller.roomNavigation.currentRoom.objectsInRoom.Count; taktar++) {
                string takeTarget = controller.roomNavigation.currentRoom.objectsInRoom[taktar].noun;
                if (takeTarget == takeTargetString) {
                    controller.interactableItems.TakeObject(actionWord, controller.roomNavigation.currentRoom.objectsInRoom[taktar], taktar);
                    return;
                } else if ((tak2 == seperatedInputWords.Length-1) && (taktar == controller.roomNavigation.currentRoom.objectsInRoom.Count-1)) {
                    FailSpeach();
                }
            }

        }
    }
    
    void CheckUse(string[] seperatedInputWords, string actionWord)
    {
        for (int u2 = 0; u2 < seperatedInputWords.Length; u2++)
        {
            string useTargetString = seperatedInputWords[u2];
            for (int utar = 0; utar < controller.roomNavigation.currentRoom.objectsInRoom.Count; utar++)
            {
                string useTarget = controller.roomNavigation.currentRoom.objectsInRoom[utar].noun;
                if (useTarget == useTargetString)
                {
                    controller.interactableItems.UseObject(seperatedInputWords, actionWord, controller.roomNavigation.currentRoom.objectsInRoom[utar]);
                    return;
                } else if ((u2 == seperatedInputWords.Length-1) && (utar == controller.roomNavigation.currentRoom.objectsInRoom.Count-1)) {
                    FailSpeach();
                }
            }
        }
    }

    void CheckAttack(string[] seperatedInputWords, string actionWord)
    {
        List<string> attackTarget = new List<string>();
        for (int att2 = 0; att2 < seperatedInputWords.Length; att2++)
        {
            attackTarget.Clear();
            string attackTargetString = seperatedInputWords[att2];
            for (int attTar = 0; attTar < controller.roomNavigation.currentRoom.charactersInRoom.Length; attTar++)
            {
                attackTarget.Add(controller.roomNavigation.currentRoom.charactersInRoom[attTar].characterName);
                for (int charSyn = 0; charSyn < controller.roomNavigation.currentRoom.charactersInRoom[attTar].alias.Length; charSyn++) {
                    attackTarget.Add(controller.roomNavigation.currentRoom.charactersInRoom[attTar].alias[charSyn]);
                }
                for (int i = 0; i < attackTarget.Count; i++) {

                    if (attackTarget[i] == attackTargetString) {
                        controller.playerController.player.state = State.battle;
                        controller.CheckGameState(controller.playerController.player.state);
                        for (int x = 0; x < controller.roomNavigation.currentRoom.charactersInRoom.Length; x++) {
                            if (controller.roomNavigation.currentRoom.charactersInRoom[x].characterName == attackTarget[i]) {
                                controller.playerController.player.target = controller.roomNavigation.currentRoom.charactersInRoom[x];
                            }
                        }
                        attackTarget.Clear();
                        return;
                    } else if ((att2 == seperatedInputWords.Length - 1) && (attTar == controller.roomNavigation.currentRoom.charactersInRoom.Length - 1) && (i == attackTarget.Count-1)) {
                        FailSpeach();
                    }
                }
            }
        }
    }

    void CheckSpeak(string[] seperatedInputWords, string actionWord)
    {
        List<string> talkTarget = new List<string>();
        for (int tal2 = 0; tal2 < seperatedInputWords.Length; tal2++)
        {
            talkTarget.Clear();
            string talkTargetString = seperatedInputWords[tal2];
            for (int talTar = 0; talTar < controller.roomNavigation.currentRoom.charactersInRoom.Length; talTar++)
            {
                talkTarget.Add(controller.roomNavigation.currentRoom.charactersInRoom[talTar].characterName);
                for (int charSyn = 0; charSyn < controller.roomNavigation.currentRoom.charactersInRoom[talTar].alias.Length; charSyn++) {
                    talkTarget.Add(controller.roomNavigation.currentRoom.charactersInRoom[talTar].alias[charSyn]);
                }
                for (int i = 0; i < talkTarget.Count; i++) {
                    if (talkTarget[i] == talkTargetString) {
                        controller.playerController.player.state = State.talk;
                        controller.conversationTarget = controller.roomNavigation.currentRoom.charactersInRoom[talTar];
                        controller.CheckGameState(controller.playerController.player.state);
                        for (int x = 0; x < controller.roomNavigation.currentRoom.charactersInRoom.Length; x++) {
                            if (controller.roomNavigation.currentRoom.charactersInRoom[x].characterName == talkTarget[i]) {
                                controller.playerController.player.target = controller.roomNavigation.currentRoom.charactersInRoom[x];
                            }
                        }
                        talkTarget.Clear();
                        return;
                    } else if ((tal2 == seperatedInputWords.Length-1) && (talTar == controller.roomNavigation.currentRoom.charactersInRoom.Length-1) &&
                                (i == talkTarget.Count - 1)) {
                        FailSpeach();
                    }
                }
            }
        }
    }

    void CheckExamine(string[] seperatedInputWords, string actionWord) {
        for (int e = 1; e < seperatedInputWords.Length; e++) {
            string examineTargetString = seperatedInputWords[e];
            for (int tar = 0; tar < controller.roomNavigation.currentRoom.objectsInRoom.Count; tar++) {
                string examineTarget = controller.roomNavigation.currentRoom.objectsInRoom[tar].noun;
                if (examineTarget == examineTargetString) {
                    controller.interactableItems.ExamineObject(actionWord, controller.roomNavigation.currentRoom.objectsInRoom[tar]);
                    return;
                } else if ((e == seperatedInputWords.Length - 1) && (tar == controller.roomNavigation.currentRoom.objectsInRoom.Count - 1)) {
                    ExamineRoom(seperatedInputWords, actionWord);
                }
            }
        }
    }

    void ExamineRoom(string[] seperatedInputWords, string actionWord) {
        for (int eR = 1; eR < seperatedInputWords.Length; eR++) {
            string examineRoomString = seperatedInputWords[eR];
            for (int eR2 = 0; eR2 < controller.roomSynonyms.categories[0].verbs.Length; eR2++) {
                string examineRoom = controller.roomSynonyms.categories[0].verbs[eR2];
                if (examineRoomString == examineRoom) {
                    controller.PrepareRoomObjects(controller.roomNavigation.currentRoom);
                    return;
                } else if ((eR == seperatedInputWords.Length - 1) && (eR2 == controller.roomSynonyms.categories[0].verbs.Length - 1)) {
                    ExamineInventory(seperatedInputWords, actionWord);
                }
            }
        }
    }

    void ExamineInventory(string[] seperatedInputWords, string actionWord) {
        controller.ClearLoggedText();
        string[] invSep = new string[controller.playerController.player.inventory.Count];
        List<string> CountOutput = new List<string>();
        List<string> invItems = new List<string>();
        int[] invCount = new int[controller.playerController.player.inventory.Count];
        for (int inv = 0; inv < controller.playerController.player.inventory.Count; inv++) {
            invItems.Add(controller.playerController.player.inventory[inv].noun);
        }
        invItems.Sort();
        //Sort and count inventory
        for (int ch = 0; ch < invItems.Count; ch++) {
            LOOP:
            if (ch + 1 < invItems.Count) {
                if (invItems[ch] == invItems[ch + 1]) {
                    if (invCount[ch] < 1) {
                        invCount[ch]++;
                    } else {
                        invCount[ch]++;
                    }
                    invItems.RemoveAt(ch + 1);
                    goto LOOP;
                }
            }
            invCount[ch]++;
            invItems[ch] = invItems[ch] + "s";
        }
        //Grammar Control
        for (int pinv = 0; pinv < invItems.Count; pinv++) {
            if (pinv < invItems.Count - 1) {
                invSep[pinv] = ", ";
            } else {
                if (pinv > 0) {
                    invSep[pinv - 1] = " and ";
                    invSep[pinv] = ".";
                } else {
                    invSep[pinv] = ".";
                }
            }
        }
        //Construct Output Sentence
        for (int co = 0; co < invItems.Count; co++) {
            CountOutput.Add(invCount[co].ToString() + " " + invItems[co]);
        }
        for (int eI = 1; eI < seperatedInputWords.Length; eI++) {
            string examineInvString = seperatedInputWords[eI];
            for (int eI2 = 0; eI2 < controller.inventory.categories[0].verbs.Length; eI2++) {
                string examineInv = controller.inventory.categories[0].verbs[eI2];
                if (examineInv == examineInvString) {
                    string[] invOutput = new string[controller.playerController.player.inventory.Count];
                    for (int inOu = 0; inOu < invItems.Count; inOu++) {
                        invOutput[inOu] = CountOutput[inOu] + invSep[inOu];
                    }
                    invItems.Clear();
                    CountOutput.Clear();
                    controller.ClearLoggedText();
                    controller.LogStringWithReturn("Your " + examineInv + " contains " + string.Join("", invOutput));
                    controller.DisplayRoomText();
                    controller.DisplayPhoto();
                    return;
                } else if ((eI == seperatedInputWords.Length - 1) && (eI2 == controller.inventory.categories[0].verbs.Length - 1) ) {
                    FailSpeach();
                }
            }
        }
    }

    void CheckEquip(string[] seperatedInputWords, string actionWord) {
        for (int eq = 0; eq < seperatedInputWords.Length; eq++) {
            string equipTargetString = seperatedInputWords[eq];
            for (int invTar = 0; invTar < controller.playerController.player.inventory.Count; invTar++) {
                string equipTarget = controller.playerController.player.inventory[invTar].noun;
                if (equipTarget == equipTargetString) {
                    if ((controller.playerController.player.inventory[invTar].equippable == true) && (controller.playerController.player.inventory[invTar].equipped == false)) {
                        for (int unEqui = 0; unEqui < controller.playerController.player.inventory.Count; unEqui++) {
                            controller.playerController.player.inventory[unEqui].equipped = false;
                        }
                        controller.interactableItems.EquipObject(actionWord, controller.playerController.player.inventory[invTar]);
                        return;
                    } else if ((controller.playerController.player.inventory[invTar].equippable == true) && (controller.playerController.player.inventory[invTar].equipped == true)) {
                        controller.ClearLoggedText();
                        controller.LogStringWithReturn("    The " + controller.playerController.player.inventory[invTar].noun + " is already equpped.");
                        controller.DisplayRoomText();
                        controller.DisplayPhoto();
                        return;
                    } else if (controller.playerController.player.inventory[invTar].equippable == false) {
                        controller.ClearLoggedText();
                        controller.LogStringWithReturn("    You cannot equip the " + controller.playerController.player.inventory[invTar].noun);
                        controller.DisplayRoomText();
                        controller.DisplayPhoto();
                        return;
                    }
                } else if ((eq == seperatedInputWords.Length - 1) && (invTar == controller.playerController.player.inventory.Count - 1)) {
                    FailSpeach();
                }
            }
        }
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    void InputComplete() {
		controller.DisplayLoggedText ();
		inputField.ActivateInputField ();
		inputField.text = null;
	}
}
