using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Globals
{
    public const float distanceToInteractWithObject = 10;
}

/*public struct WayToMoveFuncVar {
    public GameObject obj;
    public Vector2 originalPos;
    public Vector2 loc;
    public float elapsedTime;
    public float totalTime;
    public float dir;

    private AnimationCurve movementCurve;

    public AnimationCurve MovementCurve
    { 
        set{ movementCurve = value; } 
        get{ return movementCurve; }
    }
    
};*/

public class PlayerControllerDepr : MonoBehaviour
{
    //objects
    private Sprite sprite = null;
    private Camera playerCamera = null;

    //movement variables
    private IEnumerator movementCoroutine;
    public AnimationCurve walk;
    public AnimationCurve run;

    //We need to manage the movement position based on animation curve to make it more seemless
        //start slow speed up slow down 
    public WayToMove currentWayToMove;
    WayToMoveFuncVar vars = new WayToMoveFuncVar();//current set variables for movement basically injected into function
    public delegate void WaytoMoveHandler(WayToMoveFuncVar a);

    
    public enum WayToMove
    {
        LerpToLoc,
        WalkToLoc,
        WalkButtonPress,
        BezierMove,
        Standing,
    }

    private WaytoMoveHandler GetWayToMoveFunc(WayToMove wayToMove)
    {
        switch (wayToMove)
        {
            case WayToMove.LerpToLoc:
                return LerpMove;
            case WayToMove.WalkToLoc:
                return MoveBasedOnAnimCurve;
            case WayToMove.WalkButtonPress:
                return MoveButtonPress;
            case WayToMove.BezierMove:
                return MoveBasedOnBezier;
            default:
                return LerpMove;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        currentWayToMove = WayToMove.Standing;
        //GetAndSetAnimCurve(currentWayToMove);

        sprite = gameObject.transform.GetChild(0).gameObject.GetComponent<Sprite>();
        playerCamera = gameObject.transform.GetChild(1).gameObject.GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        Inputs(); 
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    /// NEED MANAGER
    /// </summary>
    private void Inputs()
    {
        
        //if inputs are hit do movement
        float move=Input.GetAxis("Horizontal");
        if (move != 0)
        {
            if (movementCoroutine != null) { StopCoroutine(movementCoroutine); }
            currentWayToMove = WayToMove.WalkButtonPress;
            vars.dir = move;
            vars.obj=gameObject;
            //should not last that length of time
            GetWayToMoveFunc(currentWayToMove)(vars);
        }

    }

    /*//each function may have its own animation curve set to it.
    private AnimationCurve GetAnimationCurve(WayToMove wayToMove)
    {
        switch (wayToMove)
        {
            case WayToMove.LerpToLoc:
                return null;
                break;
            case WayToMove.WalkToLoc:
                return null;
                break;
            default:
                return null;
        }
    }*/
   /* private void GetAndSetAnimCurve(WayToMove wayToMove)
    {
        vars.MovementCurve = GetAnimationCurve(wayToMove);
    }*/



    public void MoveToLocation(RaycastHit2D hit,float time)
    {
        //seperated this because we can do other things with the hit for certain cases
        MoveToLocation(hit.point,time);
    }
    private void MoveToLocation(Vector2 loc,float time) {
        if (movementCoroutine != null) { StopCoroutine(movementCoroutine);}
        movementCoroutine = Move(gameObject, loc, time);
        StartCoroutine(movementCoroutine);
    }
 
    private IEnumerator Move(GameObject obj,Vector2 loc,float t) {
        float totalTime = t;
        float elapsedTime = 0;

        vars.obj = obj;
        vars.originalPos = obj.transform.position;
        vars.loc = loc;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            //setup all the variables
            vars.elapsedTime = elapsedTime;
            vars.totalTime = totalTime;

            //then call the function of movement we want with vars with  WayToMoveFuncVar struct as a way to input programmatically
            GetWayToMoveFunc(currentWayToMove)(vars);

            yield return null;
        }
        
    }


    //multiple ways to move to a particular location these below functions provide for the varying ways
    private void LerpMove(WayToMoveFuncVar vars) {
        Debug.Log(vars.elapsedTime / vars.totalTime);
        vars.obj.transform.position = Vector2.Lerp(vars.originalPos, vars.loc, vars.elapsedTime / vars.totalTime);
        Debug.Log(vars.loc);
    }
    private void MoveButtonPress(WayToMoveFuncVar vars) {
        vars.obj.transform.position=new Vector2(0.1f*vars.dir+vars.obj.transform.position.x, vars.obj.transform.position.y);
    }

    private void MoveBasedOnAnimCurve(WayToMoveFuncVar vars) {
        
    }



    private void MoveBasedOnBezier(WayToMoveFuncVar vars) {

    }
}
