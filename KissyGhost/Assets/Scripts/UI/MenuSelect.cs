using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuSelect : MonoBehaviour
{
    public VideoOnDelay _VideoOnDelay;
    public HeartZoomTransition _HeartZoomTransition;
    public Text option1, option2, option3, option4;
    //public AudioSource audio;
    Transform Controls, Instructions, WinCondition;
    public GameObject heartZoom;
    public Color orgColor;
    public Color altColor;

    const int MAX_CHOICES = 4;

    float menuSelectTimer = 0.2f;
    float instructScreensTimer = 1;

    bool canMove = false;
    bool HasSelectedPlay = false;
    bool canAdvance = false;

    int choice = 1;
    int WhichInstructScreen = 0;

    void Awake()
    {
        Time.timeScale = 1;

        Controls = transform.Find("Controls");
        Instructions = transform.Find("Instructions");
        WinCondition = transform.Find("WinCondition");

        Controls.gameObject.SetActive(false);
        Instructions.gameObject.SetActive(false);
        WinCondition.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_HeartZoomTransition.enabled || _VideoOnDelay.IsMoviePlaying)
        {
            return;
        }

        DelayMenuSelect();
        HandleInput();
        ColorSelectedText();

        if (HasSelectedPlay)
            ProceedThroughInstructions();
    }

    void DelayMenuSelect()
    {
        if (canMove == false)
        {
            menuSelectTimer -= Time.deltaTime;
            if (menuSelectTimer <= 0)
            {
                canMove = true;
                menuSelectTimer = 0.2f;
            }
        }
    }

    void HandleInput()
    {
        float JoystickMove = Input.GetAxisRaw("Menu");

        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickMove >= .7f && canMove == true && !HasSelectedPlay)
        {
            //Menu Movement Sound
            soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);

            if (choice >= MAX_CHOICES)
            {
                canMove = false;
                choice = 1;            
            }
            else
            {
                canMove = false;
                choice++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickMove <= -.7f && canMove == true && !HasSelectedPlay)
        {
            //Menu Movement Sound
            soundManager.SOUND_MAN.playSound("Play_Menu_Up", gameObject);
            if (choice <= 1)
            {
                canMove = false;
                choice = MAX_CHOICES;
            }
            else
            {
                canMove = false;
                choice--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {
            //Menu Confirm Sound
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);

            if (!HasSelectedPlay)
            {
                Scene scene = SceneManager.GetActiveScene();
                switch (choice)
                {
                    case 1:
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(scene.buildIndex + 1);
                        break;

                    case 2:
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(scene.buildIndex + 11);
                        break;

                    case 3:
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(scene.buildIndex + 5);
                        break;

                    case 4:
#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(-1);
#endif
                        break;

                    default:
                        break;
                }
            }
        }
    }
    public void StartGame()
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Settings()
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(SceneManager.GetActiveScene().buildIndex + 11);
    }
    public void Credits()
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(SceneManager.GetActiveScene().buildIndex + 5);
    }
    public void End()
    {
        Application.Quit();
    }
    void ColorSelectedText()
    {
        switch (choice)
        {
            case 1:
                option1.color = altColor;
                option2.color = orgColor;
                option3.color = orgColor;
                option4.color = orgColor;
                break;

            case 2:
                option1.color = orgColor;
                option2.color = altColor;
                option3.color = orgColor;
                option4.color = orgColor;
                break;

            case 3:
                option1.color = orgColor;
                option2.color = orgColor;
                option3.color = altColor;
                option4.color = orgColor;
                break;
            case 4:
                option1.color = orgColor;
                option2.color = orgColor;
                option3.color = orgColor;
                option4.color = altColor;
                break;
            default:
                break;
        }
    }

    void ProceedThroughInstructions()
    {
        DelayInstructScreenAdvance();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
        {
            if (canAdvance)
            {
                switch (WhichInstructScreen)
                {
                    case 1:
                        WhichInstructScreen++;
                        instructScreensTimer = 1;
                        canAdvance = false;

                        Controls.gameObject.SetActive(false);
                        Instructions.gameObject.SetActive(true);
                        break;

                    case 2:
                        WhichInstructScreen++;
                        instructScreensTimer = 1;
                        canAdvance = false;

                        Instructions.gameObject.SetActive(false);
                        WinCondition.gameObject.SetActive(true);
                        break;

                    case 3:
                        WhichInstructScreen++;
                        instructScreensTimer = 1;
                        canAdvance = false;

                        Instructions.gameObject.SetActive(false);
                        WinCondition.gameObject.SetActive(true);
                        break;

                    case 4:
                        Scene scene_2 = SceneManager.GetActiveScene();
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(scene_2.buildIndex + 1);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void DelayInstructScreenAdvance()
    {
        if (instructScreensTimer > 0)
            instructScreensTimer -= Time.deltaTime;
        else
            canAdvance = true;
    }
}
