using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public GameObject activeArea;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        minBounds = new Vector3(-11f, float.MaxValue, -11f);
        maxBounds = new Vector3(12, float.MaxValue, 2.5f);
    }

    void Update()
    {
        Debug.Log(IsOnFarmland());
    }

    bool IsOnFarmland()
    {
        Vector3 pos = activeArea.transform.position;
        return pos.x > minBounds.x
            && pos.x < maxBounds.x
            && pos.z > minBounds.z
            && pos.z < maxBounds.z;
    }
}
