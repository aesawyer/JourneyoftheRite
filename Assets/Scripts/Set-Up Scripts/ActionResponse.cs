/* 
 * Copyright(c) 2018. Adam Sawyer. All Rights Reserved
 */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public class ActionResponse : MonoBehaviour
{
    GameController controller;
    [HideInInspector] public BaseObject inventoryObject;
    [HideInInspector] public BaseObject targetObject;

    public void DetermineMethod(GameController cont, string methodName)
    {
        controller = cont;
        Invoke(methodName, 0);
    }

    //-------------------------------------------------------------------------------------
    void DestroyObject()
    {
        controller.playerController.player.inventory.Remove(inventoryObject);
        controller.roomNavigation.currentRoom.objectsInRoom.Remove(targetObject);
    }

    void TestSpecial()
    {
        controller.roomNavigation.currentRoom.objectsInRoom.Remove(targetObject);
        Debug.Log("TestSpecialSuccess");
    }

}