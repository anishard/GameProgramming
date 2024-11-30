using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    //create delegates
    //updates new question ui
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    //display resolution screen for correct/incorrect/etc
    //video 2 15min in
    // public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    // public DisplayResolutionScreenCallback DisplayResolutionScreen;

    //update score in ui
    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated;

    public int CurrentFinalScore;
}
