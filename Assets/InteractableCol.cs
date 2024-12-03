using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCol : MonoBehaviour
{
    private PlayerController playerController; 
    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    //get closest interactable
    private void OnTriggerStay2D(Collider2D collision)
    {
        //interactable
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            GameObject collidedObj = collision.gameObject;
            float dist = Vector2.Distance(this.transform.position, collidedObj.transform.position);
            if (dist < playerController.closestDis)
            {
                playerController.currInteractable = collidedObj;
                playerController.closestDis = dist;
            }
            else if (collidedObj == playerController.currInteractable)
            {
                playerController.closestDis = dist;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if (collision.gameObject == playerController.currInteractable)
            {
                playerController.currInteractable = null;
                playerController.closestDis = Mathf.Infinity;
            }
        }
    }

}
