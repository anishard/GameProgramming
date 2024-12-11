using UnityEngine;

public class FarmSquare
{
    public static readonly float length = 1.25f;
    public readonly Vector3 position;
    
    public GameObject gameObject;
    public FarmSquareState state;
    public bool isWatered;
    public float rightBound;
    public float lowerBound;
    public float timePlanted;

    public FarmSquare(float maxX, float minZ)
    {
        state = FarmSquareState.Untilled;
        isWatered = false;
        rightBound = maxX;
        lowerBound = minZ;

        position = new Vector3(rightBound - length / 2, 0.3f, lowerBound + length / 2);
    }
}