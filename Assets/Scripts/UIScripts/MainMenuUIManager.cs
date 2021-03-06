using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI subtitle;
    public TextMeshProUGUI startGame;
    public TextMeshProUGUI joinGame;
    public TextMeshProUGUI exitGame;
    public TextMeshProUGUI descripionHeader;
    public TextMeshProUGUI description;

    public void ChangeSubtitleText(string text)
    {
        subtitle.text = text;
    }

    public void ChangeStartGameText(string text)
    {
        startGame.text = text;
    }

    public void ChangeJoinGameText(string text)
    {
        joinGame.text = text;
    }

    public void ChangeExitGameText(string text)
    {
        exitGame.text = text;
    }

    public void ChangeDescriptionHeaderText(string text)
    {
        descripionHeader.text = text;
    }

    public void ChangeDescriptionText(string text)
    {
        description.text = text;
    }
}
