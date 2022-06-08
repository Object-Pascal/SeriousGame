using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;
    public TextMeshProUGUI start;

    public TextMeshProUGUI gameName;
    public TextMeshProUGUI gameNamePlaceholder;
    public TextMeshProUGUI gameNameInput;

    public TextMeshProUGUI amountOfTurnsText;
    public TextMeshProUGUI amountOfTurnsCounter;
    public Slider amountOfTurnsSlider;

    private string gameNameString;
    private int amountOfTurnsValue;

    public void ChangeHeaderText(string text)
    {
        header.text = text;
    }

    public void ChangeStartText(string text)
    {
        start.text = text;
    }

    public void ChangeGameNameText(string text)
    {
        gameName.text = text;
    }

    public void ChangeGameNamePlaceholderText(string text)
    {
        gameNamePlaceholder.text = text;
    }

    public void GetGameNameInputText()
    {
        gameNameString = gameNameInput.text;
    }

    public void ChangeAmountOfTurnsText(string text)
    {
        amountOfTurnsText.text = text;
    }

    public void ChangeAmountOfTurnsCounter(int value)
    {
        amountOfTurnsCounter.text = $"{value}";
    }

    public void GetAmountOfTurnsValue()
    {
        amountOfTurnsValue = (int)amountOfTurnsSlider.value;
        ChangeAmountOfTurnsCounter(amountOfTurnsValue);
    }
}
