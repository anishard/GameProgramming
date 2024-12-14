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
    public Seed seed;
    public int timePlanted;
    public int timeLastWatered;
    public float rightBound;
    public float lowerBound;
    public readonly Vector3 position;

    private int growTime;

    public FarmSquare(float maxX, float minZ)
    {
        rightBound = maxX;
        lowerBound = minZ;
        position = new Vector3(rightBound - length / 2, 0.3f, lowerBound + length / 2);

        objects = new();
        state = FarmSquareState.Untilled;
        growTime = GetGrowTime();
    }

    public void Update()
    {
        int curTime = Clock.GetTotalHours();

        if (CheckIfWatered())
        {
            if (curTime - timeLastWatered > 72) // die if unwatered for 3 days
            {
                ShowAlert(CropAlert.Dead);
                Kill();
            }

            if (timePlanted > 0 && curTime - timeLastWatered > 3) // need water every day
            {
                ShowAlert(CropAlert.Water);
            }
        }
        if (CheckIfMature())
        {
            ShowAlert(CropAlert.Harvest);
            state = FarmSquareState.Mature;
        }
    }

    public void Till()
    {
        state = FarmSquareState.Tilled;
    }

    public void PlantSeed(string item)
    {
        Enum.TryParse(item[..^4], out seed);

        state = FarmSquareState.Seeds;

        int curTime = Clock.GetTotalHours();
        timePlanted = curTime;
        timeLastWatered = curTime;
    }

    public void Water()
    {
        timeLastWatered = Clock.GetTotalHours();
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

    private int GetGrowTime()
    {
        if (seed == Seed.Carrot) return 7;
        if (seed == Seed.Corn) return 9;
        if (seed == Seed.Eggplant) return 7;
        if (seed == Seed.Pumpkin) return 10;
        if (seed == Seed.Tomato) return 12;
        else return 7;
    }

    private bool CheckIfWatered()
    {
        return timeLastWatered > 0 && timePlanted > 0;

    }

    private bool CheckIfMature()
    {
        return (timePlanted > 0) && (Clock.GetTotalHours() - timePlanted > growTime * 24);
    }

}