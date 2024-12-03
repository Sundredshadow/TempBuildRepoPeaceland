using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerPlateClickObj : MonoBehaviour,EventClickInterface
{

    // Start is called before the first frame update

    public void Interact(RaycastHit2D hit)
    {
        //place code here will run when object is clicked on
        //NOTE object layermask must be set to clickable and must have collider
    }


}
