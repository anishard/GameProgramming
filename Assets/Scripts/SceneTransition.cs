using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string targetScene; // Name of the target scene to load

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is tagged as "Player"
        if (other.CompareTag("Player"))
            LoadScene();
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            if (Note.isActive) Note.Remove();
            SceneManager.LoadScene(targetScene);
        }
        else
            Debug.LogWarning("Target scene is not set in the SceneTransition script! Please set it in the Inspector.");
    }
}
