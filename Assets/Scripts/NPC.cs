using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool isIdle;
    private bool isIntroduced;

    void Start()
    {
        isIdle = true;
        isIntroduced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isIdle) ActivateAlert();
        else RemoveAlert();
        
        if (ClickDetected() && isIdle && !Dialogue.isActive)
            Dialogue.ActivateNPC(gameObject.name, true);
    }

    public bool ClickDetected()
    {
        return Game.ClickDetected() && Player.ObjectDetected(gameObject.name);
    }

    public void ActivateAlert()
    {
        Alert.Activate(AlertType.Help, transform.position + new Vector3(0f, 2f));
    }

    public void RemoveAlert()
    {
        Alert.Remove(transform.position);
    }
}
