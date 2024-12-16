using System;
using UnityEngine;

public class Alert : MonoBehaviour
{
    private static GameObject[] alerts;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, Game.alertRotation, 0f);
    }

    public static void Activate(AlertType alertType, Vector3 position)
    {
        Remove(position);

        alerts = Resources.LoadAll<GameObject>("Alerts");

        GameObject alert = Array.Find(alerts, (d) => d.name == alertType.ToString());

        if (alert == null)
            Debug.LogError(alertType + " does not exist in Assets/Resources/Alerts");

        Instantiate(alert, position, Quaternion.identity);
    }

    public static void Remove(Vector3 position)
    {
        GameObject alert;
        
        alerts = GameObject.FindGameObjectsWithTag("Alert");

        alert = Array.Find(alerts, (a) => {
            var pos = a.transform.position;
            return pos.x == position.x && pos.z == position.z;
        });

        if (alert != null) Destroy(alert);
    }
}
