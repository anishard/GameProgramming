using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public float moveSpeed = 2f;      
    public float thresholdDistance = 1f;

    private GameObject spawnZone;  
    private Vector3 targetPosition;  

    void Start()
    {
        spawnZone = GameObject.Find("SpawnZone");
        SetRandomTarget();
    }

    void Update()
    {
        MoveTowardsTarget();

        // If the spider is within threshold distance of the target and the delay has passed, set a new target
        if (Vector3.Distance(transform.position, targetPosition) < thresholdDistance)
        {
            SetRandomTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += moveSpeed * Time.deltaTime * direction;

        // Rotate smoothly towards the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);  // Rotate smoothly
        }
    }

    void SetRandomTarget()
    {
        // Select a random point within the spawn zone to target
        Vector3 spawnZoneSize = spawnZone.GetComponent<Renderer>().bounds.size;
        Vector3 spawnZoneCenter = spawnZone.GetComponent<Renderer>().bounds.center;
        float randomX = Random.Range(spawnZoneCenter.x - spawnZoneSize.x / 2, spawnZoneCenter.x + spawnZoneSize.x / 2);
        float randomZ = Random.Range(spawnZoneCenter.z - spawnZoneSize.z / 2, spawnZoneCenter.z + spawnZoneSize.z / 2);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the spider collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            KitchenGame kitchenGame = FindObjectOfType<KitchenGame>();
            kitchenGame.GetComponent<AudioSource>().PlayOneShot(kitchenGame.crushSound, 0.1f);
            Destroy(gameObject);
        }
    }
}