using UnityEngine;

public class Interactable : MonoBehaviour
{
    bool isFocus = false;
    bool hasInteracted = false;

    //this method meant to be overwritten
    public virtual void Interact() { }

    void Update()
    {
        if (isFocus && !hasInteracted)
        {
            Interact();
            hasInteracted = true;
        }
    }

    public void OnFocused()
    {
        isFocus = true;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        hasInteracted = false;
    }
}