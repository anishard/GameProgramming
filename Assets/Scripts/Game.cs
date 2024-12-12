using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Player player;
    private GameObject[] walkthroughs;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        walkthroughs = Resources.LoadAll<GameObject>("Walkthroughs");
    }

    public void ActivateWalkthrough(string name)
    {
        GameObject walkthrough = Array.Find(walkthroughs, (w) => w.name == name);

        if (walkthrough == null)
            throw new Exception(name + " does not exist in the given array");

        Instantiate(walkthrough, Vector3.zero, Quaternion.identity);
    }
}
