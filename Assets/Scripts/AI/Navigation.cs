using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    // CODE FOR NPC AGENT FOLLOWING PLAYER (attach to agent)
    // public Transform player;
    // private NavMeshAgent agent;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     agent = GetComponent<NavMeshAgent>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     agent.destination = player.position;
    // }
    /////////////////////////////////////////////////////

    //NPCS ROAMING RANDOMLY AROUND SCENE
    public float roamRadius = 10f;
    private NavMeshAgent agent;
    //private Vector3 dest;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandPosition();
    }

    void Update() {
        //if the npc is close enough to the destination, generate a new random destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            MoveToRandPosition();
        }
    }

    void MoveToRandPosition() {
        // generate random position in a sphere of radius roamRadius around the NPC's position to move to
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += transform.position;

        // find location on navmesh that is closest to the random point (if that point is on navmesh)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, roamRadius, NavMesh.AllAreas)) {
            //dest = hit.position;
            agent.SetDestination(hit.position);
        }
    }

}
