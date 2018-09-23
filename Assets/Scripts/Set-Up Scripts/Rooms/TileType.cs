using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {
    [HideInInspector] public int tileType;
    /*
     * 1 - Blocked
     * 2 - Slow
     * 3 - Normal
     * 4 - Blank
     */
    public GameObject tileVisual;
}
