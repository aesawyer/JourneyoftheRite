using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterTileType {
    [HideInInspector] public int tileType;
    /*
     * 1 - player
     * 2 - party
     * 3 - enemy
     */
    public GameObject tileVisual;
    public string id;
}
