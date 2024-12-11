using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Farming : MonoBehaviour
{
    public GameObject activeArea;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private FarmSquare[,] farmland;

    void Start()
    {
        // bounds of farmland
        minBounds = new Vector3(-11.25f, float.MaxValue, -6.25f);
        maxBounds = new Vector3(11.25f, float.MaxValue, 6.25f);

        // create a grid of FarmSquares
        // farmland[0, 0] is by the well, farmland[8, 4] is in the opposite corner
        farmland = new FarmSquare[9, 5];

        const float squareLength = 2.5f;
        for (int row = 0; row < farmland.GetLength(0); ++row)
            for (int col = 0; col < farmland.GetLength(1); ++col)
            {
                float rightBound = minBounds.x + squareLength * (row + 1);
                float lowerBound = maxBounds.z - squareLength * (col + 1);
                farmland[row, col] = new FarmSquare(rightBound, lowerBound);
            }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsOnFarmland())
            {
                FarmSquare square = GetFarmSquare();
            }
            else
            {
                Debug.Log("not on farmland");
            }
        }
    }

    bool IsOnFarmland()
    {
        Vector3 pos = activeArea.transform.position;
        return pos.x >= minBounds.x
            && pos.x <= maxBounds.x
            && pos.z >= minBounds.z
            && pos.z <= maxBounds.z;
    }

    FarmSquare GetFarmSquare()
    {
        Vector3 pos = activeArea.transform.position;
        FarmSquare square;

        for (int row = 0; row < farmland.GetLength(0); ++row)
            for (int col = 0; col < farmland.GetLength(1); ++col)
            {
                square = farmland[row, col];

                if (pos.x <= square.rightBound && pos.z >= square.lowerBound)
                    return square;
            }

        throw new Exception("Active area is not on farmland.");
    }
}
