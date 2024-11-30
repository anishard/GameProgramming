using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage; // back of card image, haven't added any pic yet

    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    void Awake() {
        puzzles = Resources.LoadAll<Sprite>("MemoryGameSprites/Fruits");
    }

    void Start() {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    void GetButtons() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for(int i=0; i<objects.Length; i++) {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles() {
        int looper = btns.Count;
        int index = 0;

        for(int i=0; i<looper; i++) {
            if (index == looper/2) {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners() {
        foreach(Button btn in btns) {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle() {
        // string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        // Debug.Log("You are clicking a button named: " + name);
        if(!firstGuess) {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess) {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(CheckIfThePuzzlesMatch());
            // if(firstGuessPuzzle == secondGuessPuzzle) {

            // }
        }
    }

    IEnumerator CheckIfThePuzzlesMatch() {
        yield return new WaitForSeconds(1f);
        if (firstGuessPuzzle == secondGuessPuzzle) {
            yield return new WaitForSeconds(0.5f);

            // make it so the buttons can't be clicked
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            // if want buttons to disappear once they are correctly guessed (can comment out if just want them to stay showing)
            // btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            // btns[secondGuessIndex].image.color = new Color(0,0,0,0);

            CheckIfTheGameIsFinished();
        }
        else {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }

    void CheckIfTheGameIsFinished() {
        countCorrectGuesses++;

        //CHANGE SCENE BACK TO WHATEVER SCENE PREVIOUSLY WAS IN HERE
        if (countCorrectGuesses == gameGuesses) {
            Debug.Log("Game Finished!");
            Debug.Log("It took you: " + countGuesses + " guesses to finish the game");
        }
    }

    void Shuffle(List<Sprite> list) {
        for(int i=0; i<list.Count; i++) {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count); //bet [i, list.Count)
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
