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
    private bool isChasing = false;
    public float agentSpeed = 3.5f;
    public GameObject muskrat;
    //public bool hasBeenCaught;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing && player != null) {
            // increase speed so that "chase" speed is faster than roam speed
            agent.speed = agentSpeed;
            float distNPCToPlayer = Vector3.Distance(transform.position, player.position);

            //if the NPC comes within a certain radius of the player, start following the player
            if (distNPCToPlayer <= radiusToFollow) {
                agent.destination = player.position;
            }
        }
    }
    /////////////////////////////////////////////////////

    public void ChasePlayer() {
        isChasing = true;
    }

    public void StopChasePlayer() {
        isChasing = false;
    }
    // if monkey collides with player
    void OnCollisionEnter(Collision collision) {
        LostFoundGame lfScript = muskrat.GetComponent<LostFoundGame>();

        // if game has started and player collides with monkey, end game
        if (collision.gameObject.CompareTag("Player") && lfScript.hasPlayedIntroDialogue) {
            // lfScript.StopLostFoundGame();
            lfScript.StealFromInventory();
        }
    }
    //to see the radius around the player that the NPC has to be in to start following it
    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(player.position, radiusToFollow);
    // }
}
