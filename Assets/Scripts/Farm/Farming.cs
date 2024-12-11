using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Farming : MonoBehaviour
{
    private Player player;
    private AudioSource audioSource;
    private GameObject activeArea;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private FarmSquare[,] farmland;
    private GameObject[] crops;
    private AudioClip[] audioClips;

    void Start()
    {
        player = gameObject.GetComponent<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
        activeArea = GameObject.Find("ActiveArea");

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

        crops = Resources.LoadAll<GameObject>("Farming/Crops");
        audioClips = Resources.LoadAll<AudioClip>("Farming/SFX");

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FarmSquare square = GetFarmSquare();
            TillSquare(square);
        }
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

    IEnumerator PlayAudio(string clipName, float? volumeScale = 1f, float? delay = 0f)
    {
        AudioClip clip = Array.Find(audioClips, (e) => e.name == clipName);

        if (clip == null)
            throw new Exception(name + " does not exist in the given array");

        yield return new WaitForSeconds((float)delay);
        audioSource.PlayOneShot(clip, (float)volumeScale);
    }


    IEnumerator PausePlayer(Action callback)
    {
        player.pauseMovement = true;
        yield return new WaitForSeconds(1);
        callback();
        player.pauseMovement = false;
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
}
