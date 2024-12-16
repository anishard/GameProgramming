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
    private const int MAX_POURS = 10;
    private bool tutorialPlayed = true; // TODO false

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

        if (!tutorialPlayed)
        {
            GetComponent<Tutorial>().enabled = true;
            tutorialPlayed = true;
        }
    }

    void Update()
    {
        if (Game.ClickDetected() && !InteractableObjectFound() && !Dialogue.isActive)
            UseFarmTool();

        foreach (var square in farmland)
            square?.Update();
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

        if (square != null && square.IsDoneGrowing) return;

        GameObject interactable = InventoryUI.equippedInteractable;

        if (Player.equipped == Equippable.Hoe)
            Till(square);

        if (interactable != null && interactable.name.Contains("Seed"))
            PlantSeed(square);

        if (Player.equipped == Equippable.Can)
            Water(square);
    }

    public void Till(FarmSquare square)
    {
        Animator anim = GameObject.Find("Hoe").GetComponent<Animator>();
        anim.SetTrigger("isActive");

        if (square == null || square.state != FarmSquareState.Untilled)
        {
            StartCoroutine(Game.PlayAudio("Miss", 0.5f, 0.35f));
            StartCoroutine(Player.Pause(() => { }));
        }
        else
        {
            StartCoroutine(Game.PlayAudio("Till", 1f, 0.4f));
            StartCoroutine(Player.Pause(() =>
            {
                GameObject dirt = CreateObject(square, "Dirt_Pile");
                square.Till(dirt);
            }));
        }
    }

    public void PlantSeed(FarmSquare square)
    {
        if (square == null || square.state != FarmSquareState.Tilled)
        {
            StartCoroutine(Game.PlayAudio("Miss", 0.5f, 0.35f));
        }
        else
        {
            StartCoroutine(Game.PlayAudio("Seed", 0.6f, 0.4f));
            square.PlantSeed(InventoryUI.equippedInteractable.name);
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
            StartCoroutine(Player.Pause(() => square?.Water()));
        }
    }

    public static void EquipFruit(FarmSquare square)
    {
        GameObject prefab = prefabs.Find((e) => e.name == square.seed.ToString());

        if (prefab == null)
            Debug.LogError(square.seed.ToString() + " does not exist in Resources");

        GameObject crop = Instantiate(
            prefab,
            square.position + new Vector3(0, 2f),
            Quaternion.identity
        );

        InventoryUI.Interact(crop);
    }

    public static void ReplaceObject(FarmSquare square)
    {
        if (square.state == FarmSquareState.Tilled)
        {
            square.state = FarmSquareState.Seeds;
            var seed = CreateObject(square, "SeedOpened");
            square.objects.Add(seed);
        }

        else if (square.state == FarmSquareState.Seeds)
        {
            square.state = FarmSquareState.Seedling;
            var seed = square.objects.Find((o) => o.name.Contains("Seed"));
            if (seed != null)
            {
                square.objects.Remove(seed);
                Destroy(seed);

                var plant = CreateObject(square, $"{square.seed}_Plant");
                plant.transform.localScale *= 0.5f;
                square.objects.Add(plant);
            }
        }

        else if (square.state == FarmSquareState.Seedling)
        {
            square.state = FarmSquareState.Growing;
            var plant = square.objects.Find((o) => o.name.Contains("Plant"));
            if (plant != null) plant.transform.localScale *= 2f;
        }

        else if (square.IsDoneGrowing)
        {
            square.state = FarmSquareState.Untilled;
            foreach (var o in square.objects) Destroy(o);
            square.objects.Clear();
        }
    }

    private static GameObject CreateObject(FarmSquare square, string prefabName)
    {
        GameObject prefab = prefabs.Find((e) => e.name == prefabName);

        if (prefab == null)
            Debug.LogError(prefabName + " does not exist in Resources");

        GameObject obj = Instantiate(prefab, square.position, Quaternion.identity);

        Vector3 objBase = obj.gameObject.transform.GetChild(0).position;
        obj.transform.position += square.position - objBase;

        return obj;
    }
}
