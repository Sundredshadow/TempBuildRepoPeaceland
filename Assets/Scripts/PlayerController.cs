using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using PathCreation;

public struct WayToMoveFuncVar
{
    public GameObject obj;
    public Vector2 originalPos;
    public Vector2 loc;
    public float elapsedTime;
    public float totalTime;
    public float dir;

    public float speed;

    private AnimationCurve movementCurve;

    public AnimationCurve MovementCurve
    {
        set { movementCurve = value; }
        get { return movementCurve; }
    }

};

/// <summary>
/// TODO
/// //need to editor gui layout different speeds for different movement enums
/// </summary>
public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction interactButton;
    private InputAction interactMouseTouch;

    public PathCreator bezierPath;

    //objects
    private Sprite sprite = null;
    private Camera playerCamera = null;

    //current interactable
    [System.NonSerialized]
    public GameObject currInteractable;
    [System.NonSerialized]
    public float closestDis = Mathf.Infinity;

    //movement variables
    private IEnumerator movementCoroutine;
    private float bezierDistTravelled;
    public float speed = 5;


    //many ways to move can lerp to a location use a specific animation curve, bezier curve, etc this handler manages the function switch
    public WayToMoveClick currentWayToMoveClick;
    WayToMoveFuncVar varsClick = new WayToMoveFuncVar();//current set variables for movement basically injected into function
    public delegate void WaytoMoveHandlerClick(WayToMoveFuncVar a);

    public WayToMoveButtonPress currentWayToMoveButton;
    WayToMoveFuncVar varsButton = new WayToMoveFuncVar();//current set variables for movement basically injected into function
    public delegate void WaytoMoveHandlerButton(WayToMoveFuncVar a);

    private void Start()
    {
        varsButton.obj =gameObject;
        varsButton.speed = speed;
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        interactButton = playerControls.Player.InteractButton;
        interactButton.Enable();
        interactButton.performed +=InteractButton;

        interactMouseTouch = playerControls.Player.InteractMouse;
        interactMouseTouch.Enable();
        interactMouseTouch.performed += InteractMouseTouch;
    }
    private void OnDisable()
    {
        move.Disable(); 
        interactButton.Disable();
        interactMouseTouch.Disable();
    }


    void Update()
    {
        Vector2 moveDirection=move.ReadValue<Vector2>();//WASD
        Debug.Log(moveDirection);
        if (moveDirection.x!=0) {
            if (movementCoroutine != null) { StopCoroutine(movementCoroutine); }
            varsButton.dir=moveDirection.x;
            movementCoroutine = MoveButton();
            StartCoroutine(movementCoroutine);

        }
        
    }

    private void InteractButton(InputAction.CallbackContext context) {
        Debug.Log("Button Interaction");
        if (currInteractable)
        {
            currInteractable.transform.parent.SendMessage("InteractButton");
        }
    }
    
    private void InteractMouseTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse/Touch Event");


        LayerMask mask = LayerMask.GetMask("Clickable");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 50, mask);//note gets current camera

        if (hit)
        {
            hit.transform.gameObject.SendMessage("Interact", hit);
        }

    }

    public void MoveToLocation(RaycastHit2D hit, float time)
    {
        //seperated this because we can do other things with the hit for certain cases
        MoveToLocation(hit.point, time);
    }
    private void MoveToLocation(Vector2 loc, float time)
    {
        if (movementCoroutine != null) { StopCoroutine(movementCoroutine); }
        movementCoroutine = MoveClick(gameObject, loc, time);
        StartCoroutine(movementCoroutine);
    }

    private IEnumerator MoveClick(GameObject obj, Vector2 loc, float t)
    {
        float totalTime = t;
        float elapsedTime = 0;

        varsClick.obj = obj;
        varsClick.originalPos = obj.transform.position;
        varsClick.loc = loc;

        if (currentWayToMoveClick==WayToMoveClick.BezierMove) { varsClick.originalPos= bezierPath.path.GetClosestPointOnPath(varsClick.originalPos); }

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            //setup all the variables
            varsClick.elapsedTime = elapsedTime;
            varsClick.totalTime = totalTime;

            //then call the function of movement we want with vars with  WayToMoveFuncVar struct as a way to input programmatically
            GetWayToMoveFunc(currentWayToMoveClick)(varsClick);

            yield return null;
        }

    }

    private IEnumerator MoveButton() {
        GetWayToMoveFunc(currentWayToMoveButton)(varsButton);
        yield return null;
    }









    public enum WayToMoveClick
    {
        LerpToLoc,
        WalkToLoc,
        BezierMove,
    }

    public enum WayToMoveButtonPress 
    {
        Walk,
        BezierMove,
    }

    private WaytoMoveHandlerClick GetWayToMoveFunc(WayToMoveClick wayToMove)
    {
        switch (wayToMove)
        {
            case WayToMoveClick.LerpToLoc:
                return LerpMove;
            case WayToMoveClick.WalkToLoc:
                return MoveBasedOnAnimCurve;
            case WayToMoveClick.BezierMove:
                return MoveBasedOnBezierClick;
            default:
                return null;
        }
    }
    private WaytoMoveHandlerButton GetWayToMoveFunc(WayToMoveButtonPress wayToMove)
    {
        switch (wayToMove)
        {
            case WayToMoveButtonPress.Walk:
                return MoveButtonPress;
            case WayToMoveButtonPress.BezierMove:
                return MoveBasedOnBezierPress;
            default:
                return null;
        }
    }



    /// <summary>
    /// Multiple ways to move to a particular location these below functions provide for the varying ways
    /// </summary>
    /// <param name="vars"></param>

    private void LerpMove(WayToMoveFuncVar vars)
    {
        vars.obj.transform.position = Vector2.Lerp(vars.originalPos, vars.loc, vars.elapsedTime / vars.totalTime);
    }
    

    private void MoveBasedOnAnimCurve(WayToMoveFuncVar vars)
    {

    }
    private void MoveBasedOnBezierClick(WayToMoveFuncVar vars)
    {
        if (bezierPath != null)
        {
            //temporary needs refactor
            vars.loc=bezierPath.path.GetClosestPointOnPath(vars.loc);
            //need to setup bezierDist Travelled
            //might simply not want to lerp screws up a lot need a speed component
            vars.obj.transform.position = bezierPath.path.GetClosestPointOnPath(Vector2.Lerp(vars.originalPos, vars.loc, vars.elapsedTime / vars.totalTime));
            //transform.rotation = bezierPath.path.GetRotationAtDistance(bezierDistTravelled, EndOfPathInstruction.Stop);
        }
    }




    private void MoveButtonPress(WayToMoveFuncVar vars)
    {
        vars.obj.transform.position = new Vector2(0.1f * vars.dir + vars.obj.transform.position.x, vars.obj.transform.position.y);
    }

    private void MoveBasedOnBezierPress(WayToMoveFuncVar vars)
    {
        if (bezierPath != null)
        {
            bezierDistTravelled += speed* vars.dir * Time.deltaTime;
            gameObject.transform.position = bezierPath.path.GetPointAtDistance(bezierDistTravelled, EndOfPathInstruction.Stop);
            //transform.rotation = bezierPath.path.GetRotationAtDistance(bezierDistTravelled, EndOfPathInstruction.Stop);
        }
    }
}
