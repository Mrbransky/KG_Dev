using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class SettingManager : MonoBehaviour
{

    private bool showOptions = false;
    public float shadowDrawDistance;
    public int ResX;
    public int ResY;
    public bool Fullscreen;

    public float aaSetting;
    Toggle SixtyH;
    Toggle OneTwentyH;

    Resolution[] resolutions;
    Dropdown resDropdown;
    List<string> resList;

    Toggle fullButt;
    void Start()
    {
        showOptions = false;
        //SixtyH = GameObject.Find("SixtyH").GetComponent<Toggle>();
        //OneTwentyH = GameObject.Find("OneTwentyH").GetComponent<Toggle>();
        resDropdown = GameObject.Find("Res").GetComponent<Dropdown>();
        fullButt = GameObject.Find("Fullscreen").GetComponent<Toggle>();
        ChangeSettingsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void ChangeTo120()
    //{
    //    Screen.SetResolution(ResX, ResY, Fullscreen, 120);
    //}
    //public void ChangeTo60()
    //{
    //    Screen.SetResolution(ResX, ResY, Fullscreen, 60);
    //}
    
    //fullscreen
    public void SetFullscreen(bool setter)
    {
        Screen.fullScreen = setter;
    }

    //Anti-Alias
    public void ChangeAA(float newAA)
    {
        aaSetting = newAA;
        Text AAtext = GameObject.Find("AAtext").GetComponent<Text>();
        switch ((int)aaSetting)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                AAtext.text = "0x AA";
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                AAtext.text = "2x AA";
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                AAtext.text = "4x AA";
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                AAtext.text = "8x AA";
                break;
        }
    }
    public void ChangeSettingsOnStart()
    {
        Text AAtext = GameObject.Find("AAtext").GetComponent<Text>();
        Slider AAslider = GameObject.Find("antiAliasing").GetComponent<Slider>();
        AAtext.text = QualitySettings.antiAliasing.ToString() + "x AA";
        switch (QualitySettings.antiAliasing)
        {
            case 0:
                AAslider.value = 0;
                break;
            case 2:
                AAslider.value = 1;
                break;
            case 4:
                AAslider.value = 2;
                break;
            case 8:
                AAslider.value = 3;
                break;
        }

        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        for (int i = 0; i < resolutions.Length; i++)
        {
            resDropdown.options.Add(new Dropdown.OptionData(resolutions[i].ToString()));
            resDropdown.options[i].text = ResToString(resolutions[i]);
            resDropdown.value = i;
        }
        resDropdown.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[resDropdown.value].width, resolutions[resDropdown.value].height, Screen.fullScreen); });
        fullButt.isOn = Screen.fullScreen;
        //foreach(Resolution res in resolutions)
        //{
        //    //Debug.Log(res);
        //    resDropdown.options.Add(new Dropdown.OptionData(res.ToString()));
        //}
    }
    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    //        if (GUI.Button(new Rect(500, 0, 140, 100), "Vsync On"))
    //        {
    //            QualitySettings.vSyncCount = 1;
    //        }
    //        if (GUI.Button(new Rect(645, 0, 140, 100), "Vsync Off"))
    //        {
    //            QualitySettings.vSyncCount = 0;
    //        }
    //}
}
