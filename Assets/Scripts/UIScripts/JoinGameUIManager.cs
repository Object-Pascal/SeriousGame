using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI JoinGame;

    public void ChangeJoinGameText(string text)
    {
        JoinGame.text = text;
    }
}
