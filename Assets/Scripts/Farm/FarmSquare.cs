public class FarmSquare
{
    public FarmSquareState state;
    public bool isWatered;
    public float rightBound;
    public float lowerBound;
    public float? timePlanted;
    public readonly float length = 2.5f;

    public FarmSquare(float maxX, float minZ)
    {
        state = FarmSquareState.Untilled;
        isWatered = false;
        rightBound = maxX;
        lowerBound = minZ;
    }
}