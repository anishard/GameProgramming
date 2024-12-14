using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public Transform target;  
    public float moveSpeed = 2f;  
    public float turnSpeed = 10f;  

    void Update()
    {
        Vector3 directionToTarget = new Vector3(target.position.x - transform.position.x, 0f, target.position.z - transform.position.z).normalized;

        // Move towards the target
        transform.position += moveSpeed * Time.deltaTime * directionToTarget;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {   
        // Check if spider reached fireplace
        if (collision.gameObject.CompareTag("Fireplace"))
        {
            Debug.Log("Spider collided with the fireplace!");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Spider collided with the player!");
            Destroy(gameObject);
        }
    }

}