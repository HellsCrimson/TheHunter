using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    [SerializeField] TMP_Text curRes;
    [SerializeField] TMP_Text curQual;
    [SerializeField] TMP_Text framesDisp;

    Resolution[] resolutions;

    private void Start()
    {
        ResolutionInit();
        curQual.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }
    
    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log10(volume)*20);
    }
    
    public void SetFx(float volume)
    {
        audioMixer.SetFloat("fxVol", Mathf.Log10(volume)*20);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        curRes.text = resolution.width + " x " + resolution.height;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        curQual.text = QualitySettings.names[qualityIndex];
    }

    public void ResolutionInit()
    {
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 59).ToArray();
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFramerate(float frames)
    {
        Application.targetFrameRate = (int)frames;
        framesDisp.text = frames.ToString();
    }

    public void SetVsync(bool Vsync)
    {
        if (Vsync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
}
