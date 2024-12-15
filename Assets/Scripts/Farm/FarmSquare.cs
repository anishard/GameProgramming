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
            ShowAlert(AlertType.Water);

        // has been planted
        if (timePlanted > 0)
        {
            CheckForGrowth();

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
    }

    public void Till(GameObject dirt)
    {
        state = FarmSquareState.Tilled;
        objects.Add(dirt);
    }

    public void PlantSeed(string seedInteractable)
    {
        ShowAlert(AlertType.Water);

        Enum.TryParse(seedInteractable[..^4], out seed);
        growTime = GetGrowTime();
        timeLastWatered = Clock.TotalHours;

        Farming.ReplaceObject(this);
        InventoryUI.DestroyInteractable();
    }

    public void Water()
    {
        HideAlert();

        int curTime = Clock.TotalHours;
        timeLastWatered = curTime;

        // first time watering        
        if (state == FarmSquareState.Tilled && timePlanted == 0)
            timePlanted = curTime;
    }

    private void CheckForGrowth()
    {
        int timeElapsed = Clock.TotalHours - timePlanted;

        if (state == FarmSquareState.Seeds && timeElapsed > 48)
            Farming.ReplaceObject(this);

        else if (state == FarmSquareState.Seedling && timeElapsed > growTime * 0.75f)
            Farming.ReplaceObject(this);
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
        HideAlert();
        state = FarmSquareState.Untilled;
    }

    private void ShowAlert(AlertType alert)
    {
        if (curAlert == alert) return;

        HideAlert();
        Alert.Activate(alert, position + new Vector3(0, 2.7f));
        curAlert = alert;
    }

    private void HideAlert()
    {
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

        return days * 24;
    }

    private bool CheckIfWatered()
    {
        // need water every day
        return timePlanted > 0
            && curAlert == AlertType.None
            && Clock.TotalHours - timeLastWatered > 24;
    }
    private bool CheckIfDead()
    {
        // die if unwatered for 3 days
        return Clock.TotalHours - timeLastWatered > 72
            && curAlert != AlertType.Harvest;
    }

    private bool CheckIfMature()
    {
        return curAlert != AlertType.Dead
            && Clock.TotalHours - timePlanted > growTime;
    }
}