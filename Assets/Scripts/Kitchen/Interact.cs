using UnityEngine;
using TMPro;
using UnityEngine.Events; 

public class Interact : MonoBehaviour
{
    public TextMeshProUGUI interactText; 
    public float radius = 1f;          
    public string text;                 
    public UnityEvent onInteract; // Can be configured to any callback desired   

    private bool playerNear = false;

    void Start()
    {   
        // Collider detects when player is near
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; 
        sphereCollider.radius = radius;

        // Hide interaction text initially
        interactText.gameObject.SetActive(false);
        interactText.text = text;
    }

    void Update()
    {
        if (playerNear)
        {
            interactText.gameObject.SetActive(true);

            // Trigger the assigned action when "F" is pressed
            if (Input.GetKeyDown(KeyCode.F) && onInteract != null)
            {
                onInteract.Invoke();
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}