using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickBezierMovePlayer : MonoBehaviour, EventClickInterface
{

    // Start is called before the first frame update
    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void Interact(RaycastHit2D hit)
    {
        TellPlayerToMove(hit);
    }


    private void TellPlayerToMove(RaycastHit2D hit) {
        //place code here will run when object is clicked on
        //NOTE object layermask must be set to clickable
        playerController.MoveToLocation(hit, 10);
    }

}
