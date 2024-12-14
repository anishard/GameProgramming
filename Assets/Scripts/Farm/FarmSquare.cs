using System;
using System.Collections.Generic;
using UnityEngine;

public enum CropAlert
{
    Water,
    Harvest,
    Dead
}

public class FarmSquare
{
    public static readonly float length = 1.25f;

    public List<GameObject> objects;
    public FarmSquareState state;
    public int timePlanted;
    public int timeLastWatered;
    public float rightBound;
    public float lowerBound;
    public readonly Vector3 position;

    private int growTime;
    private int missedDays;

    public FarmSquare(float maxX, float minZ)
    {

        objects = new List<GameObject>();
        state = FarmSquareState.Untilled;
        timeLastWatered = 0;
        timePlanted = 0;

        rightBound = maxX;
        lowerBound = minZ;
        position = new Vector3(rightBound - length / 2, 0.3f, lowerBound + length / 2);

        growTime = 300; // TODO
        missedDays = 0; // TODO
    }

    public void Update()
    {
        int curTime = Clock.GetTotalHours();

        if (curTime - timeLastWatered > 72) // die if unwatered for 3 days
        {
            ShowAlert(CropAlert.Dead);
            Kill();
        }

        if (curTime - timeLastWatered > 24) // need water every day
        {
            ShowAlert(CropAlert.Water);
            ChangeSoilColor(false);
        }

        if (curTime - timePlanted > growTime * 24)
        {
            ShowAlert(CropAlert.Harvest);
            state = FarmSquareState.Mature;
        }
    }

    public void Till()
    {
        state = FarmSquareState.Tilled;
    }

    public void PlantSeed()
    {
        state = FarmSquareState.Seeds;
        timePlanted = Clock.GetTotalHours();
    }

    public void Water()
    {
        timeLastWatered = Clock.GetTotalHours();
        ChangeSoilColor(true);
    }

    public void Harvest()
    {
        state = FarmSquareState.Untilled;
    }

    public void Kill()
    {
        state = FarmSquareState.Dead;
    }

    public void ClearDebris()
    {
        state = FarmSquareState.Untilled;
    }

    public void ShowAlert(CropAlert alert)
    {
        Debug.Log(alert);
    }

    private void ChangeSoilColor(bool toDark)
    {
        Renderer rend = objects?
            .Find((o) => o.name.Contains("Dirt_Pile"))?
            .GetComponent<MeshRenderer>();

        if (!rend) return;

        Color c = rend.material.color;
        float brightness = toDark ? 0.4f : 1f;
        rend.material.color = new Color(c.r * brightness, c.g * brightness, c.b * brightness);
    }

}