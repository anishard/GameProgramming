using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeliverDish : MonoBehaviour
{
    // attach to Player game object
    public Clock clockObject;
    public GameObject diningHallExit; // the object that triggers entering dining hall scene
    public GameObject player;
    private NavMeshAgent agent;
    private bool hasPlayedDeliverIntro;
    private int currHour;
    private bool startMovingToExit;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        agent = player.GetComponent<NavMeshAgent>();
        hasPlayedDeliverIntro = false;
        startMovingToExit = false;
    }

    // Update is called once per frame
    void Update()
    {
        currHour = Clock.hour;
        //if (currHour == 22) { // 10pm -> meal time 
        if (currHour == 22 && !hasPlayedDeliverIntro) {   // for debugging purposes, 10am
            PlayDialogue();
        }
        // once the dialogue telling player to go to dining hall has played, AUTOMATICALLY direct player to exit to dining hall (use navmesh)
        if (hasPlayedDeliverIntro && !startMovingToExit) {
            Debug.Log("has played deliver intro");
            MoveToExit();
        }
    }
    
    void PlayDialogue() {
        SceneTransition stScript = diningHallExit.GetComponent<SceneTransition>();
        stScript.enabled = false;
        Dialogue.Activate("DeliverDishIntro" + sceneName);
        hasPlayedDeliverIntro = true;
    }

    void MoveToExit() {
        SceneTransition stScript = diningHallExit.GetComponent<SceneTransition>();
        stScript.enabled = true;
        agent.SetDestination(diningHallExit.transform.position);
        startMovingToExit = true;
    }
}
