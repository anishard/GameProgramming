using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizGameManager : MonoBehaviour
{
    //array of questions
    Question[] _questions = null;
    public Question[] Questions { get {return _questions;} }

    [SerializeField] GameEvents events = null;


    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    //put used questions in this list
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    //start game
    void Start() 
    {
        LoadQuestions();

        foreach (var question in Questions) 
        {
            Debug.Log(question.Info);
        }

        // Display();
    }

    public void EraseAnswers() {
        PickedAnswers = new List<AnswerData>();
    }

    void Display() 
    {
        EraseAnswers();
        //randomize questions
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null) {
            events.UpdateQuestionUI(question);
        }
        else {
            Debug.Log("update question ui in events is null and occured in gamemanager.display()");
        }
    }

    Question GetRandomQuestion () {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }

    int GetRandomQuestionIndex () {
        var random = 0;
        //get question if not done and is not current question
        if (FinishedQuestions.Count < Questions.Length) {
            do {
                random = UnityEngine.Random.Range(0, Questions.Length);
            }
            while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    void LoadQuestions () 
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];

        for (int i = 0; i < objs.Length; i++) 
        {
            _questions[i] = (Question)objs[i];
        }
    }
}
