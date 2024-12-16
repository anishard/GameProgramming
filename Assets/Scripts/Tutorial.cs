using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private int step;
    private bool isWaiting;
    private InventoryUI inventory;
    private GameObject seed;

    void Start()
    {
        step = 1;
        isWaiting = false;
        inventory = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();
        seed = Resources.Load<GameObject>("InventorySprites/CarrotSeed");

        SpawnSeed();
        Clock.Pause();
        Dialogue.Activate("TutorialIntro");    
    }

    void Update()
    {
        if (Dialogue.isActive) return;

        if (step < 7 && GameObject.Find("CarrotSeed(Clone)") == null && inventory.IsEmpty)
            SpawnSeed();

        if (step == 1)
            ExecuteStep
            (
                "Use WASD to move around.",
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D)
            );

        if (step == 2)
            ExecuteStep
            (
                "Left-click to use an object. Select the hoe button and dig a hole.",
                GameObject.Find("Dirt_Pile(Clone)") != null
            );

        if (step == 3)
            ExecuteStep
            (
                "Right-click to (un)equip an object. I've left some seeds by your doorstep, grab them.",
                Player.equipped == Equippable.Interactable
            );

        if (step == 4)
            ExecuteStep
            (
                "Click the plus by the chest button to stash it in your inventory.",
                !inventory.IsEmpty
            );

        if (step == 5)
            ExecuteStep
            (
                "Click the chest button to open your inventory and equip them again.",
                Player.equipped == Equippable.Interactable
            );

        if (step == 6)
            ExecuteStep
            (
                "Plant the seeds in the hole you made.",
                GameObject.Find("SeedOpened(Clone)") != null
            );

        if (step == 7)
            ExecuteStep
            (
                "Select the watering can button. Use the well to refill the can.",
                Player.ObjectDetected("Well") && Game.ClickDetected()
            );

        if (step == 8)
            ExecuteStep
            (
                "Water the seeds.",
                GameObject.Find("Water(Clone)") == null
            );

        if (step == 9)
        {
            Tip.Remove();
            Clock.Resume();
            Dialogue.Activate("TutorialOutro");
            GetComponent<Tutorial>().enabled = false;
        }
    }

    void ExecuteStep(string message, bool condition)
    {
        Tip.Activate(message);
        if (condition) StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        if (!isWaiting)
        {
            isWaiting = true;
            yield return new WaitForSeconds(1);
            step++;
            isWaiting = false;
        }
    }

    void SpawnSeed()
    {
        Instantiate(seed, new Vector3(7.3f, 0.45f, 11.5f), Quaternion.identity);
    }
}
