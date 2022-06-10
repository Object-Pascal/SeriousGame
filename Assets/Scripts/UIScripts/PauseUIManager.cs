using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public void changeHeaderText(string text)
    {
        header.text = text;
    }
}
