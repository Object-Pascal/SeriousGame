using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public TextMeshProUGUI resume;
    public TextMeshProUGUI settings;

    public void ChangeHeaderText(string text)
    {
        header.text = text;
    }

    public void ChangeResumeText(string text)
    {
        resume.text = text;
    }

    public void ChangeSettingsText(string text)
    {
        settings.text = text;
    }
}
