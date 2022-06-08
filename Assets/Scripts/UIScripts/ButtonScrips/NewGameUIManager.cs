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
}
