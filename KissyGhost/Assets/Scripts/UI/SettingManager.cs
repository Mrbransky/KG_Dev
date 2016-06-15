using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    void Start()
    {
        showOptions = false;
        SixtyH = GameObject.Find("SixtyH").GetComponent<Toggle>();
        OneTwentyH = GameObject.Find("OneTwentyH").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSettingsOnStart();
    }

    //Resolution
    public void ChangeTo120()
    {
        Screen.SetResolution(ResX, ResY, Fullscreen, 120);
    }
    public void ChangeTo60()
    {
        Screen.SetResolution(ResX, ResY, Fullscreen, 60);
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
    }
    //void OnGUI()
    //{
    //        //1080p
    //        if (GUI.Button(new Rect(500, 430, 93, 100), "1080p"))
    //        {
    //            Screen.SetResolution(1920, 1080, Fullscreen);
    //            ResX = 1920;
    //            ResY = 1080;
    //            Debug.Log("1080p");
    //        }
    //        //720p
    //        if (GUI.Button(new Rect(596, 430, 93, 100), "720p"))
    //        {
    //            Screen.SetResolution(1280, 720, Fullscreen);
    //            ResX = 1280;
    //            ResY = 720;
    //            Debug.Log("720p");
    //        }
    //        //480p
    //        if (GUI.Button(new Rect(692, 430, 93, 100), "480p"))
    //        {
    //            Screen.SetResolution(640, 480, Fullscreen);
    //            ResX = 640;
    //            ResY = 480;
    //            Debug.Log("480p");
    //        }
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
