using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickEventManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            //mask that all clickable objects have so raycast are ignored for say background objects
            LayerMask mask = LayerMask.GetMask("Clickable");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,50,mask);//note gets current camera

            if (hit)
            {
                hit.transform.gameObject.SendMessage("Interact", hit);
            }
        }
    }
}
