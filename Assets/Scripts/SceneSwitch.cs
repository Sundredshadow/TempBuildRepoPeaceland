using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make sure to have this to switch scenes
using UnityEngine.SceneManagement;

/*Treat this script as a boiler plate for switching scenes on interaction*
 * Change sceneName to change the scene you want
 * SwitchScene() if you need to add addtional interactions
 * Update() manages the distance an object can be interacted with(may want to switch to collider test)
 * 
 * */
public class SceneSwitch : MonoBehaviour, EventClickInterface,EventInterface
{
    public string sceneName = "SceneTest";
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public void Interact(RaycastHit2D hit)
    {
        SwitchScene();
    }
    public void InteractButton()
    {
        SwitchScene();
    }

    private void SwitchScene()
    {
        Debug.Log("SceneSwitch");
        SceneManager.LoadScene(sceneName);
    }
}
