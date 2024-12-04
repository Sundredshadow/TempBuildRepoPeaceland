using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBoilerPlate : MonoBehaviour, EventClickInterface, EventInterface
{
    /*Treat this script as a boiler plate forinteraction*
     * 
     * Interact() if you need to add addtional interactions
     * Update() manages the distance an object can be interacted with(may want to switch to collider test)
     * 
     * */
    PlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void Interact(RaycastHit2D hit)
    {
        InteractButton();
    }

    public void InteractButton()
    {
        //rotates self
        //gameObject.transform.Rotate(new Vector3(0,0,90));
        Debug.Log("Interactable");
    }

    void Update()
    {
        /*float dist = Vector2.Distance(playerController.transform.position, gameObject.transform.position);
        if (Globals.distanceToInteractWithObject >= dist)
        {
            playerController.GetComponent<ButtonPressEventManager>().SetClosestInteractable(gameObject, dist);
        }*/
    }




}
