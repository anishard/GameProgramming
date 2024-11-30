using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable()]
public struct UIManagerParameters
{
    [Header("Answer Options")]
    [SerializeField] float margins;
    public float Margins { get {return margins;} }
}

[Serializable()]
public struct UIElements
{
    //where we store answers
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea {get {return answersContentArea;} }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject {get {return questionInfoTextObject;} }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText {get {return scoreText;} }

    [Space]
    // [SerializeField] Image resolutionsBG;
    // public Image ResolutionBG {get {return resolutionBG;} }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText {get {return resolutionScoreText;} }
    [Space]

    //highscore text
    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText {get {return highScoreText;} }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup {get {return mainCanvasGroup;} }

    // [SerializeField] RectTransform finishUIElements;;
    // public TextMeshProUGUI FinishUIElements {get {return finishUIElements;} }

}

public class UIManager : MonoBehaviour
{
    // public enum ResolutionScreenType { Correct, Incorrect, Finish}

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswerData answerPrefab;

    [SerializeField] UIElements uIElements;

    [Space]

    [SerializeField] UIManagerParameters parameters;

    List<AnswerData> currentAnswer = new List<AnswerData>();

    void OnEnable() 
    {

    }

    void OnDisable()
    {

    }

    void UpdateQuestionUI(Question question) 
    {
        uIElements.QuestionInfoTextObject.text = question.Info;

        //create answers
        CreateAnswers(question);
    }

    void CreateAnswers(Question question) 
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++) 
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uIElements.AnswersContentArea);

        }
    }

    void EraseAnswers ()
    {
        foreach (var answer in currentAnswer)
        {
            Destroy(answer.gameObject);
        }
        currentAnswer.Clear();
    }
}
