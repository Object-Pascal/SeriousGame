using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public TextMeshProUGUI qualityPreset;

    public void SetHeaderText(string text)
    {
        header.text = text;
    }

    public void SetQualityPresetText(string text)
    {
        qualityPreset.text = text;
    }
}
