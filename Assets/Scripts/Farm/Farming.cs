using System;
using System.Collections.Generic;
using UnityEngine;

public class Farming : MonoBehaviour
{
    public static List<GameObject> prefabs;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private FarmSquare[,] farmland;

    private int numPours;
    private const int MAX_POURS = 5;

    void Start()
    {
        // load resources
        prefabs = new List<GameObject>();
        prefabs.AddRange(Resources.LoadAll<GameObject>("InventorySprites"));
        prefabs.AddRange(Resources.LoadAll<GameObject>("Farming"));

        // bounds of farmland
        minBounds = new Vector3(-11.25f, float.MaxValue, -6.25f);
        maxBounds = new Vector3(11.25f, float.MaxValue, 6.25f);

        // create a grid of FarmSquares
        // farmland[0, 0] is by the well, farmland[17, 9] is in the opposite corner
        farmland = new FarmSquare[18, 10];

        for (int row = 0; row < farmland.GetLength(0); ++row)
            for (int col = 0; col < farmland.GetLength(1); ++col)
            {
                float rightBound = minBounds.x + FarmSquare.length * (row + 1);
                float lowerBound = maxBounds.z - FarmSquare.length * (col + 1);
                farmland[row, col] = new FarmSquare(rightBound, lowerBound);
            }

        numPours = MAX_POURS;

        // Dialogue.Activate("GameIntro");
    }

    void Update()
    {
        if (Game.ClickDetected() && !InteractableObjectFound())
            UseFarmTool();

        foreach (var square in farmland)
            if (square != null) square.Update();
    }

    void EnterHouse()
    {
        StartCoroutine(Game.PlayAudio("OpenDoor", 0.5f));
    }

    void EnterTown() { }

    FarmSquare GetFarmSquare()
    {
        if (IsOnFarmland())
        {
            Vector3 pos = Player.activeArea.transform.position;
            FarmSquare square;

            for (int row = 0; row < farmland.GetLength(0); ++row)
                for (int col = 0; col < farmland.GetLength(1); ++col)
                {
                    square = farmland[row, col];

                    if (pos.x <= square.rightBound && pos.z >= square.lowerBound)
                    {
                        return square;
                    }
                }
        }

        return null;
    }

    bool InteractableObjectFound()
    {
        // TODO stop click from dropping an object
        bool objectFound = true;

        if (Player.ObjectDetected("Town")) EnterTown();
        else if (Player.ObjectDetected("Door")) EnterHouse();
        else if (Player.ObjectDetected("ShippingBin")) OpenBin();
        else objectFound = false;

        return objectFound;
    }

    bool IsOnFarmland()
    {
        Vector3 pos = Player.activeArea.transform.position;
        return pos.x >= minBounds.x
            && pos.x <= maxBounds.x
            && pos.z >= minBounds.z
            && pos.z <= maxBounds.z;
    }

    void OpenBin()
    {
        StartCoroutine(Game.PlayAudio("ShippingBin", 0.15f));
    }

    void UseFarmTool()
    {
        FarmSquare square = GetFarmSquare();

        GameObject interactable = InventoryUI.equippedInteractable;

        if (interactable != null && interactable.name.Contains("Seed"))
            PlantSeed(square);

        if (Player.equipped == Equippable.Hoe)
            Till(square);

        if (Player.equipped == Equippable.Can)
            Water(square);
    }

    public void Till(FarmSquare square)
    {
        Animator anim = GameObject.Find("Hoe").GetComponent<Animator>();
        anim.SetTrigger("isActive");

        if (square.state != FarmSquareState.Untilled)
        {
            StartCoroutine(Game.PlayAudio("Miss", 0.5f, 0.35f));
            StartCoroutine(Player.Pause(() => { }));
        }
        else
        {
            StartCoroutine(Game.PlayAudio("Till", 0.6f, 0.4f));
            StartCoroutine(Player.Pause(() =>
            {
                var obj = InstantiateByName("Dirt_Pile", prefabs, square.position);
                square.objects.Add(obj);
                square.Till();
            }));
        }
    }

    public void PlantSeed(FarmSquare square)
    {
        if (square.state != FarmSquareState.Tilled)
        {
            StartCoroutine(Game.PlayAudio("Miss", 0.5f, 0.35f));
        }
        else
        {
            StartCoroutine(Game.PlayAudio("Seed", 0.6f, 0.4f));

            Vector3 pos = square.position;
            pos.y += 0.05f;
            square.objects.Add(InstantiateByName("SeedOpened", prefabs, pos));
            square.PlantSeed(InventoryUI.equippedInteractable.name);

            InventoryUI.DestroyInteractable();
        }
    }

    public void Water(FarmSquare square)
    {
        Animator anim = GameObject.Find("Can").GetComponent<Animator>();
        anim.SetTrigger("isActive");

        if (Player.ObjectDetected("Well")) // refill
        {
            numPours = MAX_POURS;
            StartCoroutine(Game.PlayAudio("FillCan", 0.2f, 0f));
            StartCoroutine(Player.Pause(() => { }));
        }
        else if (numPours <= 0) // can is empty
        {
            --numPours;
            StartCoroutine(Game.PlayAudio("Miss", 0.5f, 0.2f));
            StartCoroutine(Player.Pause(() => { }));
        }
        else
        {
            --numPours;
            StartCoroutine(Game.PlayAudio("PourCan", 0.3f, 0.2f));
            StartCoroutine(Player.Pause(() =>
            {
                square.Water();
            }));
        }
    }

    GameObject InstantiateByName(string name, List<GameObject> array, Vector3 position)
    {
        GameObject obj = array.Find((e) => e.name == name);

        if (obj == null)
            throw new Exception(name + " does not exist in Resources");

        return Instantiate(obj, position, Quaternion.identity);
    }
}
