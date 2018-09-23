/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    public BattleMap battleMap;
    public GameController controller;
    public PlayerController playerController;
    public List<KeyValuePair<string, int>> TurnOrder = new List<KeyValuePair<string, int>>();

    public void Start() {
        BattleMap map = Instantiate(battleMap, new Vector3(Screen.width / 2, Screen.height / 2, 490), Quaternion.identity);
        map.controller = controller;
        map.playercontroller = playerController;
        map.turnOrder = CheckTurnOrder();
        for (int l = 0; l < TurnOrder.Count; l++) {
            Debug.Log(TurnOrder[l]);
        }
    }

    public int DamageCalc(int strength, int weaponBonus = 0, int statusBonus = 0) {
        System.Random random = new System.Random();
        int damage = (random.Next(1, strength) + weaponBonus + statusBonus);
        return damage;
    }

    public List<KeyValuePair<string, int>> CheckTurnOrder() {
        System.Random rnd = new System.Random();
        int rndNum;
        TurnOrder.Clear();
        TurnOrder.Add(new KeyValuePair<string, int>(playerController.player.characterName, playerController.player.stats.speed));
        for (int p = 0; p < playerController.player.party.Length; p++) {
            TurnOrder.Add(new KeyValuePair<string, int>(playerController.player.party[p].characterName, playerController.player.party[p].stats.speed));
        }
        TurnOrder.Add(new KeyValuePair<string, int>(playerController.player.target.characterName, playerController.player.target.stats.speed));
        for (int r = 0; r < controller.roomNavigation.currentRoom.charactersInRoom.Length; r++) {
            rndNum = rnd.Next(0, 100);
            if (rndNum < playerController.player.threat) {
                if (controller.roomNavigation.currentRoom.charactersInRoom[r].characterName != playerController.player.target.characterName) {
                    TurnOrder.Add(new KeyValuePair<string, int>(controller.roomNavigation.currentRoom.charactersInRoom[r].characterName, controller.roomNavigation.currentRoom.charactersInRoom[r].stats.speed));
                }
            }
        }
        playerController.player.target = null;
        TurnOrder.Sort((a, b) => (b.Value.CompareTo(a.Value)));
        return TurnOrder;
    }
}
