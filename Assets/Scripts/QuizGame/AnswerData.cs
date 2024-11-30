using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//every answer will have this data attached to it
public class AnswerData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject;
    [SerializeField] Image toggle;

    [Header("Textures")] 
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedToggle;

    [Header("References")]
    [SerializeField] GameEvents events;

    private int _answerIndex = -1;
    public int AnswerIndex { get {return _answerIndex;} }

    private bool Checked = false;

    public void UpdateData(string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        UpdateUI();

        //update current answers
        if (events.UpdateQuestionAnswer != null) 
        {
            events.UpdateQuestionAnswer(this);
        }
    }

    void UpdateUI() 
    {
        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
    }
}
