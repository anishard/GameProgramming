using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    // CODE FOR NPC AGENT FOLLOWING PLAYER (attach to agent)
    public Transform player;
    public float radiusToFollow;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distNPCToPlayer = Vector3.Distance(transform.position, player.position);

        //if the NPC comes within a certain radius of the player, start following the player
        if (distNPCToPlayer <= radiusToFollow) {
            agent.destination = player.position;
        }
    }
    /////////////////////////////////////////////////////

    //to see the radius around the player that the NPC has to be in to start following it
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, radiusToFollow);
    }
}
