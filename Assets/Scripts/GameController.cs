/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public PlayerController playerController;
    public BattleController battleController;
    public new GameObject renderer;
    public Text displayText;
    public Dictionary library;
    public Dictionary roomSynonyms;
    public Dictionary inventory;
    [HideInInspector] public BaseCharacter conversationTarget;
	[HideInInspector] public RoomNavigation roomNavigation;
	[HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string> ();
    [HideInInspector] public int controlObject;
    TextInput textInput;
	SpriteRenderer display;
    GameObject spriteDisplay;
    [HideInInspector] public InteractableItems interactableItems;
    [HideInInspector] public ActionResponse actionResponse;
	List<string> actionLog = new List<string> ();
    //---------------------------------------------
    void Awake () {
		interactableItems = GetComponent<InteractableItems> ();
        textInput = GetComponent<TextInput>();
        actionResponse = GetComponent<ActionResponse>();
		roomNavigation = GetComponent<RoomNavigation> ();
        spriteDisplay = Instantiate(renderer);//, new Vector3(0,0,0),Quaternion.identity);
		display = spriteDisplay.GetComponent<SpriteRenderer> ();
        //controlObject = GetInstanceID();
        //Debug.Log(controlObject);
        
	}
    
	void Start () {
		DisplayPhoto ();
		DisplayRoomText ();
		DisplayLoggedText ();
        CheckGameState(playerController.player.state);
    }
	//-----------------------------------------------------------------------------------------------------------------------------
	public void DisplayPhoto() {
        if (display != null) {
            display.sprite = roomNavigation.currentRoom.sprite;
            //Debug.Log(display.sprite.name);
        }
    }
	
	public void DisplayLoggedText() {
		string logAsText = string.Join ("\n", actionLog.ToArray ());
		displayText.text = logAsText;
	}

	public void DisplayRoomText() {
		ClearCollectionsForNewRoom ();
		UnpackRoom ();
		string combinedText;
		string joinedInteractionDescriptions = string.Join ("", interactionDescriptionsInRoom.ToArray ());
		//Changes Description of Room
		if (roomNavigation.currentRoom.roomDescriptions.researched == false) {
			if (roomNavigation.currentRoom.roomDescriptions.changed == false) {
				combinedText = roomNavigation.currentRoom.roomDescriptions.mainDescription + "\n" + "\n" + joinedInteractionDescriptions;
			} else {
				combinedText = roomNavigation.currentRoom.roomDescriptions.changeDescription + "\n" + "\n" + joinedInteractionDescriptions;
			}
			LogStringWithReturn (combinedText);
		} else {
			if (roomNavigation.currentRoom.roomDescriptions.changed == false) {
				combinedText = roomNavigation.currentRoom.roomDescriptions.mainDescription + "\n" + 
                roomNavigation.currentRoom.roomDescriptions.secondaryDescription + "\n" + "\n" +
                joinedInteractionDescriptions;
			} else {
				combinedText = roomNavigation.currentRoom.roomDescriptions.changeDescription + "\n" + 
                roomNavigation.currentRoom.roomDescriptions.secondaryDescription + "\n" + "\n" +
                joinedInteractionDescriptions;
			}
			LogStringWithReturn (combinedText);
		}
	}
    //-----------------------------------------------------------------------------------------------------------------------------
    void UnpackRoom() {
		roomNavigation.UnpackExitsInRoom ();
		PrepareRoomCharacters (roomNavigation.currentRoom);
    }
	
	public void PrepareRoomObjects( Room currentRoom) {
        ClearLoggedText();
        string examineIntro = "You notice ";
        string[] sep = new string[currentRoom.objectsInRoom.Count];
        string[] roomObjectDescription = new string[currentRoom.objectsInRoom.Count];
        for (int i = 0; i < currentRoom.objectsInRoom.Count; i++) {
			BaseObject interactableInRoom = currentRoom.objectsInRoom[i];
            roomObjectDescription[i] = (interactableInRoom.examineDescription + " " + interactableInRoom.roomLocation);
            if (i < currentRoom.objectsInRoom.Count -1) {
                sep[i] = ", ";
            } else {
                if (i > 0) {
                    sep[i - 1] = " and ";
                } else {
                    sep[i] = "";
                }
            }
        }
        string[] logOutput = new string[currentRoom.objectsInRoom.Count];
        for (int log = 0; log < currentRoom.objectsInRoom.Count; log++) {
            logOutput[log] = roomObjectDescription[log] + sep[log];
        }
        LogStringWithReturn(examineIntro + string.Join("", logOutput));
        DisplayRoomText();
        DisplayPhoto();
    }

	void PrepareRoomCharacters(Room currentRoom) {
        if (currentRoom.charactersInRoom.Length != 0) {
            string charIntro = "You also see ";
            string[] charSep = new string[currentRoom.charactersInRoom.Length];
            string[] roomCharacters = new string[currentRoom.charactersInRoom.Length];
            for (int c = 0; c < currentRoom.charactersInRoom.Length; c++) {
                roomCharacters[c] = (currentRoom.charactersInRoom[c].description + " " + currentRoom.charactersInRoom[c].roomLocation);
                if (c < currentRoom.charactersInRoom.Length - 1) {
                    charSep[c] = ", ";
                } else {
                    if (c > 0) {
                        charSep[c - 1] = " and ";
                    } else {
                        charSep[c] = "";
                    }
                }
            }
            string[] charOutput = new string[currentRoom.charactersInRoom.Length];
            for (int chaOut = 0; chaOut < currentRoom.charactersInRoom.Length; chaOut++) {
                charOutput[chaOut] = roomCharacters[chaOut] + charSep[chaOut];
            }
            interactionDescriptionsInRoom.Add(charIntro + string.Join("", charOutput));
        }
    }

	void ClearCollectionsForNewRoom() {
		interactionDescriptionsInRoom.Clear ();
		roomNavigation.ClearExits ();
	}

	public void LogStringWithReturn(string stringToAdd) {
		actionLog.Add (stringToAdd + "\n");
	}

    public void ClearLoggedText() {
        actionLog.Clear();
        displayText.text = "";
        //Debug.Log("Cleared");
    }   
    //------------------------------------------------------------------------------------------------------------------------------
    public void CheckGameState (State state) {
        string stateName = state.ToString();
        switch (stateName) {
            case "explore":
                    //Fades in Placeholder
                    Color tmp = textInput.inputField.placeholder.GetComponent<CanvasRenderer>().GetColor();
                    tmp.a = 1f;
                    textInput.inputField.placeholder.GetComponent<CanvasRenderer>().SetColor(tmp);
                    textInput.inputField.interactable = true;
                    textInput.inputField.Select();
                Debug.Log("Exploring");
                ClearLoggedText();
                DisplayPhoto();
                DisplayRoomText();
                DisplayLoggedText();
                break;
            case "pause":
                //Book is lowered
                break;

            case "battle":
                //Fades out Placeholder
                Color tmp2 = textInput.inputField.placeholder.GetComponent<CanvasRenderer>().GetColor();
                tmp2.a = 0f;
                textInput.inputField.placeholder.GetComponent<CanvasRenderer>().SetColor(tmp2);
                textInput.inputField.interactable = false; //causing problems with optionInput
                display.sprite = null;
                Debug.Log("Attacking");
                

                BattleController currentCombat = Instantiate(battleController, new Vector3(0, 0,0), Quaternion.identity);
                currentCombat.controller = this;
                currentCombat.playerController = playerController;
                ClearLoggedText();
                break;

            case "talk":
                Debug.Log("Talking!");
                ClearLoggedText();
                break;

            case "death":
                Debug.Log("You died");
                ClearLoggedText();
                break;
        }
    }
		
	void Update() {
        
    }

    void OnDestroy() {
        //if (display != null) {
            //display.sprite = null;
        //}
    }
}
