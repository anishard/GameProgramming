using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeliverDish : MonoBehaviour
{

    public Clock clockObject;
    //public GameObject player;
    public GameObject diningHallExit; // the object that triggers entering dining hall scene
    private NavMeshAgent agent;
    private bool hasPlayedDeliverIntro;
    private int currHour;
    private bool startMovingToExit;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        hasPlayedDeliverIntro = false;
        startMovingToExit = false;
    }

    // Update is called once per frame
    void Update()
    {
        currHour = Clock.hour;
        //Debug.Log("current hour: " + currHour);
        //if (currHour == 22) { // 10pm -> meal time 
        if (currHour == 7 && !hasPlayedDeliverIntro) {   // for debugging purposes
            PlayDialogue();
            // Dialogue.Activate("DeliverDishInt ro");
            // hasPlayedDeliverIntro = true;
        }
        // once the dialogue telling player to go to dining hall has played, AUTOMATICALLY direct player to exit to dining hall (use navmesh)
        if (hasPlayedDeliverIntro && !startMovingToExit) {
            MoveToExit();
        }
    }
    
    void PlayDialogue() {
        Dialogue.Activate("DeliverDishIntro");
        hasPlayedDeliverIntro = true;
    }

    void MoveToExit() {
        agent.SetDestination(diningHallExit.transform.position);
        startMovingToExit = true;
    }
}
