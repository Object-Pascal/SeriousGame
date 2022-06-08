using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public TextMeshProUGUI subtitle;
    public TextMeshProUGUI startGame;

    public void ChangeSubtitleText(string text)
    {
        subtitle.text = text;
    }

    public void ChangeStartGameText(string text)
    {
        startGame.text = text;
    }
}
