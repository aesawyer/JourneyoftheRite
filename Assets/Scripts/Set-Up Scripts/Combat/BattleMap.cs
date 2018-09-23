using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleMap : MonoBehaviour {

    [HideInInspector] public GameController controller;
    [HideInInspector] public PlayerController playercontroller;
    public GameObject prefab;
    public TileType[] tileTypes;
    public CharacterTileType[] charTileTypes;
    int[,] tiles;
    [HideInInspector] const int mapSizeX = 9;
    [HideInInspector] const int mapSizeY = 6;
    int tileCheckx;
    int tileChecky;
    public List<KeyValuePair<string, int>> turnOrder = new List<KeyValuePair<string, int>>();

    void Start() {
        tiles = new int[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                tiles[x, y] = 3;
                //Blank Tiles
                for (int bla = 0; bla < controller.roomNavigation.currentRoom.roomDimensions.blankSquares.Length; bla++) {
                    tileCheckx = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.blankSquares[bla].x-1);
                    tileChecky = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.blankSquares[bla].y-1);
                    if (tiles[x,y] == tiles[tileCheckx,tileChecky]) {
                            tiles[x, y] = 4;
                    }
                }
                //Blocked Tiles
                for (int blo = 0; blo < controller.roomNavigation.currentRoom.roomDimensions.blockedSquares.Length; blo++) {
                    tileCheckx = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.blockedSquares[blo].x-1);
                    tileChecky = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.blockedSquares[blo].y-1);
                    if (tiles[x, y] == tiles[tileCheckx, tileChecky]) {
                        tiles[x, y] = 1;
                    }
                }
                //Slow Tiles
                for (int slow = 0; slow < controller.roomNavigation.currentRoom.roomDimensions.slowSquares.Length; slow++) {
                    tileCheckx = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.slowSquares[slow].x-1);
                    tileChecky = Convert.ToInt32(controller.roomNavigation.currentRoom.roomDimensions.slowSquares[slow].y-1);
                    if (tiles[x, y] == tiles[tileCheckx, tileChecky]) {
                        tiles[x, y] = 2;
                    }
                }
            }
        }

        GenerateMap();
        PlaceCharacters();
    }

    void GenerateMap() {
        for (int x = 0; x < mapSizeX; x++) {
            for (int y = 0; y < mapSizeY; y++) {
                TileType tt = tileTypes[tiles[x, y]]; //Determines prefab to load (would read as "tt = tileTypes[3]")
                int xOffset = (int)((tt.tileVisual.GetComponent<SpriteRenderer>().sprite.bounds.size.x) *3) + 1;
                int yOffset = (int)((tt.tileVisual.GetComponent<SpriteRenderer>().sprite.bounds.size.y) *3) + 1;
                Instantiate(tt.tileVisual, new Vector3(120 + (x*xOffset), (110 - (y*yOffset)), 0), Quaternion.identity);
                //Instantiate(tt.tileVisual, new Vector3(cam.width / 2 + spriteSize * 2 + (x * xOffset), (Screen.height / 2 + Screen.height / 4 + Screen.height / 5) + spriteSize - (y * yOffset), transform.position.z), Quaternion.identity);

            }
        }
    }

    void PlaceCharacters() {
        //TODO: Create script to place all characters in room
        System.Random rnd = new System.Random();
        for (int l = 0; l < turnOrder.Count; l++) {
            for (int r = 0; r < controller.roomNavigation.currentRoom.charactersInRoom.Length; r++) {
                if (controller.roomNavigation.currentRoom.charactersInRoom[r].characterName == turnOrder[l].Key ) {
                    //PlaceEnemy
                }
            }
            for (int p = 0; p < playercontroller.player.party.Length; p++) {
                //PlaceParty
            }
            //PlacePlayer
            CharacterTileType ctt = charTileTypes[1];
            Instantiate(ctt.tileVisual, new Vector3(120, 110, 0), Quaternion.identity);
            
        }
    }

}
