using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class Farming : MonoBehaviour
{
    private Player player;
    private AudioSource audioSource;
    private GameObject activeArea;
    private EventSystem eventSys;

    private Vector3 minBounds;
    private Vector3 maxBounds;
    private FarmSquare[,] farmland;

    private int numPours;
    private const int MAX_POURS = 5;

    private GameObject[] crops;
    private AudioClip[] audioClips;

    void Start()
    {
        player = gameObject.GetComponent<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
        activeArea = GameObject.Find("ActiveArea");
        eventSys = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        // load resources
        crops = Resources.LoadAll<GameObject>("Farming/Crops");
        audioClips = Resources.LoadAll<AudioClip>("Farming/SFX");

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
    }

    void Update()
    {
        if (player.DetectObject("Town"))
        {
            EnterDiningRoom();
            return;
        }

        // on left click
        if (Input.GetMouseButtonDown(0))
        {
            // ignore clicks over uGUI
            if (eventSys.IsPointerOverGameObject()) return;

            if (player.DetectObject("Door"))
            {
                EnterHouse();
                return;
            }

            FarmSquare square = GetFarmSquare();

            if (player.equipped == Equippable.Hoe)
                TillSquare(square);

            if (player.equipped == Equippable.Can)
                UseCan(square);
        }
    }

    void EnterDiningRoom()
    {
        Debug.Log("Switch scenes here");
    }

    void EnterHouse()
    {
        StartCoroutine(PlayAudio("OpenDoor", 0.3f));
    }

    bool DetectExitScene()
    {
        bool exitFound = true;

        if (player.DetectObject("Door")) EnterHouse();
        else if (player.DetectObject("Town")) EnterDiningRoom();
        else exitFound = false;

        return exitFound;
    }

    FarmSquare GetFarmSquare()
    {
        if (IsOnFarmland())
        {
            Vector3 pos = activeArea.transform.position;
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

    GameObject InstantiateByName(string name, GameObject[] array, Vector3 position)
    {
        GameObject obj = Array.Find(array, (e) => e.name == name);

        if (obj == null)
            throw new Exception(name + " does not exist in the given array");

        return Instantiate(obj, position, Quaternion.identity);

    }

    bool IsOnFarmland()
    {
        Vector3 pos = activeArea.transform.position;
        return pos.x >= minBounds.x
            && pos.x <= maxBounds.x
            && pos.z >= minBounds.z
            && pos.z <= maxBounds.z;
    }

    IEnumerator PausePlayer(Action callback)
    {
        player.pauseMovement = true;
        yield return new WaitForSeconds(1);
        callback();
        player.pauseMovement = false;
    }

    IEnumerator PlayAudio(string clipName, float? volumeScale = 1f, float? delay = 0f)
    {
        AudioClip clip = Array.Find(audioClips, (e) => e.name == clipName);

        if (clip == null)
            throw new Exception(name + " does not exist in the given array");

        yield return new WaitForSeconds((float)delay);

        audioSource.PlayOneShot(clip, (float)volumeScale);
    }

    void TillSquare(FarmSquare square)
    {
        Animator anim = GameObject.Find("Hoe").GetComponent<Animator>();
        anim.SetTrigger("isActive");

        if (square == null || square.state != FarmSquareState.Untilled)
        {
            StartCoroutine(PlayAudio("Miss", 0.5f, 0.35f));
            StartCoroutine(PausePlayer(() => { }));
        }
        else
        {
            StartCoroutine(PlayAudio("Till", 0.5f, 0.35f));
            StartCoroutine(PausePlayer(() =>
            {
                square.gameObject = InstantiateByName("Dirt_Pile", crops, square.position);
                square.state = FarmSquareState.Tilled;
            }));
        }
    }

    void UseCan(FarmSquare square)
    {
        Animator anim = GameObject.Find("Can").GetComponent<Animator>();
        anim.SetTrigger("isActive");

        if (player.DetectObject("Well")) // refill
        {
            numPours = MAX_POURS;
            StartCoroutine(PlayAudio("FillCan", 0.3f, 0f));
            StartCoroutine(PausePlayer(() => { }));
        }
        else if (numPours <= 0) // can is empty
        {
            --numPours;
            StartCoroutine(PlayAudio("Miss", 0.5f, 0.2f));
            StartCoroutine(PausePlayer(() => { }));
        }
        else
        {
            --numPours;
            StartCoroutine(PlayAudio("PourCan", 0.3f, 0.2f));
            StartCoroutine(PausePlayer(() => { }));
        }
    }
}
