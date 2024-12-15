using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene; // Name of the target scene to load

    private void OnTriggerEnter(Collider other)
    {
        // Log what object entered the trigger
        Debug.Log($"Something entered the trigger: {other.name}");

        // Check if the object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger. Attempting to load scene...");
            LoadScene();
        }
        else
        {
            Debug.Log("The object that entered the trigger is not tagged as 'Player'.");
        }
    }

    private void LoadScene()
    {
        // Check if the target scene is set
        if (!string.IsNullOrEmpty(targetScene))
        {
            Debug.Log($"Loading scene: {targetScene}");
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.LogWarning("Target scene is not set in the SceneTransition script! Please set it in the Inspector.");
        }
    }
}
