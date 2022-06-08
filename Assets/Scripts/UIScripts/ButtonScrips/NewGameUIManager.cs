using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public void ChangeHeaderText(string text)
    {
        header.text = text;
    }
}
