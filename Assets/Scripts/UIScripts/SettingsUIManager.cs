using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public void SetHeaderText(string text)
    {
        header.text = text;
    }
}
