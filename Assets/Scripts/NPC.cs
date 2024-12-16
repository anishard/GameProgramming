using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool isIntroduced;
    public bool isMidQuest;
    public bool CanOfferQuest {
        get { return isIntroduced && !isMidQuest; }
    }

    private bool showAlert;

    void Start()
    {
        showAlert = false;
        isMidQuest = false;
        isIntroduced = true;// false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showAlert) ActivateAlert();
        else RemoveAlert();

        if (ClickDetected() && !showAlert && !isMidQuest && !Dialogue.isActive)
            Dialogue.ActivateNPC(gameObject.name, true);
    }

    public bool ClickDetected()
    {
        return Game.ClickDetected() && Player.ObjectDetected(gameObject.name);
    }

    public void ActivateAlert()
    {
        if (!showAlert)
            Alert.Activate(AlertType.Help, transform.position + new Vector3(0f, 2.5f));

        showAlert = true;
    }

    public void RemoveAlert()
    {
        if (showAlert)
            Alert.Remove(transform.position);

        showAlert = false;
    }
}
