using System;
using System.Collections.Generic;
using UnityEngine;

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
    private AlertType curAlert;

    public FarmSquare(float maxX, float minZ)
    {
        rightBound = maxX;
        lowerBound = minZ;
        position = new Vector3(rightBound - length / 2, 0.3f, lowerBound + length / 2);

        objects = new();
        state = FarmSquareState.Untilled;
        curAlert = AlertType.None;
    }

    public void Update()
    {
        if (CheckIfWatered())
        {
            ShowAlert(AlertType.Water);
        }

        if (CheckIfDead())
        {
            ShowAlert(AlertType.Dead);
            Kill();
        }

        if (CheckIfMature())
        {
            ShowAlert(AlertType.Harvest);
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
        growTime = GetGrowTime();

        int curTime = Clock.GetTotalHours();
        timePlanted = curTime;
        timeLastWatered = curTime;
    }

    public void Water()
    {
        timeLastWatered = Clock.GetTotalHours();
        HideAlert();
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
        HideAlert();
    }

    public void ShowAlert(AlertType alert)
    {
        if (curAlert == alert) return;

        HideAlert();
        Alert.Activate(alert, position + new Vector3(0, 3f));
        curAlert = alert;
    }

    public void HideAlert()
    {
        Debug.Log($"Hiding {curAlert}");
        curAlert = AlertType.None;
        Alert.Remove(position);
    }

    private int GetGrowTime()
    {
        int days;

        if (seed == Seed.Carrot) days = 7;
        else if (seed == Seed.Corn) days = 9;
        else if (seed == Seed.Eggplant) days = 7;
        else if (seed == Seed.Pumpkin) days = 10;
        else if (seed == Seed.Tomato) days = 12;
        else days = 7;

        days = 3;
        return days * 24;
    }


    private bool CheckIfWatered()
    {
        var o = timePlanted > 0
            && curAlert == AlertType.None
            && Clock.GetTotalHours() - timeLastWatered > 24; // need water every day

        if (o) Debug.Log("checking if watered");
        return o;
    }
    private bool CheckIfDead()
    {
        var o = timePlanted > 0
            && Clock.GetTotalHours() - timeLastWatered > 72; // die if unwatered for 3 days

        if (o) Debug.Log("checking if dead");
        return o;
    }

    private bool CheckIfMature()
    {
        var o = timePlanted > 0
            && curAlert != AlertType.Dead
            && Clock.GetTotalHours() - timePlanted > growTime;
        
        if (o) Debug.Log("checking if mature");
        return o;
    }

}