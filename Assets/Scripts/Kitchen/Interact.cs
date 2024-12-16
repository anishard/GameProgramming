using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.AI;

public class Interact : MonoBehaviour
{
    public TextMeshProUGUI interactText; 
    public float radius = 1f;          
    public string text;                 
    public UnityEvent onInteract; // Can be configured to any callback desired   

    private bool playerNear = false;
    private bool trigger = true;

    void Start()
    {   
        // Collider detects when player is near
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true; 
        sphereCollider.radius = radius;
    }

    internal void SetText() {
        interactText.text = text;
    }

    internal void SetText(string newText) {
        interactText.text = newText;
    }

    internal void ClearText() {
        interactText.text = "";
    }

    void Update()
    {
        if (playerNear)
        {   
            // Trigger the assigned action when interact key is pressed
            if (Input.GetKeyDown(KeyCode.F) && onInteract != null)
            {
                onInteract.Invoke();
            }
        }
    }

    public void Toggle() {
        if (this.enabled) {
            ClearText();
            trigger = false;
            this.enabled = false;
        }
        else {
            SetText();
            trigger = true;
            this.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger && other.CompareTag("Player")) 
        {
            SetText();
            playerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trigger && other.CompareTag("Player"))
        {
            ClearText();
            playerNear = false;
        }
    }
}