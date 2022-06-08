using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public TextMeshProUGUI gameName;
    public TextMeshProUGUI gameNamePlaceholder;
    public TextMeshProUGUI gameNameInput;

    public TextMeshProUGUI amountOfTurnsText;
    public TextMeshProUGUI amountOfTurnsCounter;

    private string gameNameString;

    public void ChangeHeaderText(string text)
    {
        header.text = text;
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

    public void ChangeAmountOfTurnsCounter(int number)
    {
        amountOfTurnsCounter.text = $"{number}";
    }
}
