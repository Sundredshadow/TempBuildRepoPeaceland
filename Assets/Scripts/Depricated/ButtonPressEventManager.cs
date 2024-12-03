using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonPressEventManager : MonoBehaviour
{
    //not this does not order events merely holds them


    //keybindings
    public KeyCode interactWithObjKeyBinding = KeyCode.E;//keybinding for interacting with object


    //closest interactable
    private GameObject closestInteractable;
    private float distance=Globals.distanceToInteractWithObject+1;

    void Update()
    {
        //this works because PressEvent is added when an individual comes into range of an object
        //Note make sure there is a system to establish if there is two objects which gets priority 
        if (Input.GetKeyDown(interactWithObjKeyBinding))
        {
            closestInteractable.SendMessage("InteractButton");
        }
    }

    public void SetClosestInteractable(GameObject newobj, float newdis){
        if (newdis < distance) {
            closestInteractable = newobj;
            distance = newdis;
        }
    }
}
