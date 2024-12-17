using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    
    // Farming
    public bool tutorialPlayed;
    public FarmSquare[,] farmland;

    // NPC
    public Dictionary<string, bool> isIntroduced;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        tutorialPlayed = true;//false;

        farmland = new FarmSquare[18, 10];
        var minBounds = new Vector3(-11.25f, float.MaxValue, -6.25f);
        var maxBounds = new Vector3(11.25f, float.MaxValue, 6.25f);
        for (int row = 0; row < farmland.GetLength(0); ++row)
            for (int col = 0; col < farmland.GetLength(1); ++col)
            {
                float rightBound = minBounds.x + FarmSquare.length * (row + 1);
                float lowerBound = maxBounds.z - FarmSquare.length * (col + 1);
                farmland[row, col] = new FarmSquare(rightBound, lowerBound);
            }
            
        isIntroduced = new() {
            {"Billy", false},
            {"Colobus", false},
            {"Echo", false},
            {"Nuru", false},
            {"Taipa", false}
        };
    }
}
