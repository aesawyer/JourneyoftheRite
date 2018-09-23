using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/Room")]
public class Room : ScriptableObject {
    public string tileName;
	public Sprite sprite;

    public RoomDimensions roomDimensions;
    public Descriptions roomDescriptions;

	public Exit[] exits;
	public List<BaseObject> objectsInRoom;
	public BaseCharacter[] charactersInRoom;

}

[System.Serializable]
public class RoomDimensions {
    [Header ("Max X: 9. Max Y: 6")]
    public Vector2[] blankSquares;
    public Vector2[] blockedSquares;
    public Vector2[] slowSquares;
}

[System.Serializable]
public class Descriptions {
    [TextArea]
    public string mainDescription;
    public bool researched;
    [TextArea]
    public string secondaryDescription;
    public bool changed;
    [TextArea]
    public string changeDescription;
}