/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour {
	
	public Room currentRoom;

	Dictionary<string, Room> exitDictionary = new Dictionary<string, Room> ();

	GameController controller;

	void Awake() {
		controller = GetComponent<GameController> ();

	}

	public void UnpackExitsInRoom() {
        string exitIntro = "You see ";
        string[] exitSep = new string[currentRoom.exits.Length];
        string[] roomExits = new string[currentRoom.exits.Length];
		for (int i = 0; i < currentRoom.exits.Length; i++) {
			exitDictionary.Add (currentRoom.exits [i].keyString, currentRoom.exits [i].valueRoom);
            roomExits[i] = currentRoom.exits[i].exitDescription + " " + currentRoom.exits[i].keyString;
            if (i < currentRoom.exits.Length-1) {
                exitSep[i] = ", ";
            } else {
                if (i > 0) {
                    if (currentRoom.charactersInRoom.Length != 0) {
                        exitSep[i] = ". ";
                        exitSep[i - 1] = " and ";
                    } else {
                        
                    }
                } else {
                    exitSep[i] = ". ";
                }
            }
		}
        string[] exitOutput = new string[currentRoom.exits.Length];
        for (int exOut = 0; exOut < currentRoom.exits.Length; exOut++) {
            exitOutput[exOut] = roomExits[exOut] + exitSep[exOut];
        }
        controller.interactionDescriptionsInRoom.Add(exitIntro + string.Join("", exitOutput));
	}

    //directionNoun derives from "Go" script
    public void AttemptToChangeRooms(string actionVerb, string directionNoun) {
		if (exitDictionary.ContainsKey (directionNoun)) {
            //Changes currentRoom to exitDictionary
            controller.ClearLoggedText();
            currentRoom = exitDictionary [directionNoun];
            controller.LogStringWithReturn ("    You " + actionVerb + " to the " + directionNoun);
			controller.DisplayRoomText ();
			controller.DisplayPhoto ();
		} else {
			controller.LogStringWithReturn ("There is no path to the " + directionNoun);
		}
	}


	public void ClearExits() {
		exitDictionary.Clear();
	}

}
