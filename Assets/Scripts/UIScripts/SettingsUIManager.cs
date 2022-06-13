using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public TextMeshProUGUI qualityPreset;
    public TMP_Dropdown qualityDropdown;

    public TextMeshProUGUI resolution;
    public TMP_Dropdown resolutionDropdown;

    public TextMeshProUGUI volume;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeDisplay;

    private List<string> qualityOptions;
    private List<string> resolutionOptions;

    private void Start()
    {
        qualityOptions = new() { "good", "better", "best" };
        qualityDropdown.AddOptions(qualityOptions);

        resolutionOptions = new() { "1920X1080", "1280X1024", "1920X1200" };
        resolutionDropdown.AddOptions(resolutionOptions);
    }

    public void SetHeaderText(string text)
    {
        header.text = text;
    }

    public void SetQualityPresetText(string text)
    {
        qualityPreset.text = text;
    }

    public void SetQuality()
    {
        Debug.Log(qualityOptions[qualityDropdown.value]);
    }

    public void SetResolutionText(string text)
    {
        resolution.text = text;
    }

    public void SetResolution()
    {
        Debug.Log(resolutionOptions[resolutionDropdown.value]);
    }

    public void SetVolumeText(string text)
    {
        volume.text = text;
    }

    public void SetVolume()
    {
        Debug.Log(volumeSlider.value);
    }

    public void SetVolumeDisplay()
    {
        volumeDisplay.text = $"{volumeSlider.value}%";
    }
}
