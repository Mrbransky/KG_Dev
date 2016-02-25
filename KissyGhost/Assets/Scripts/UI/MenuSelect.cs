using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{
    public HeartZoomTransition _HeartZoomTransition;
    public Image option1, option2, option3;
    public AudioSource audio;
    Transform Controls, Instructions, WinCondition;
    public GameObject heartZoom;

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
        if (_HeartZoomTransition.enabled)
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

            if (choice >= 3)
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
                choice = 3;
            }
            else
            {
                canMove = false;
                choice--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
        {
            //Menu Confirm Sound
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);

            if (!HasSelectedPlay)
            {
                switch (choice)
                {
                    case 1:
                        //  HasSelectedPlay = true;
                        //  heartZoom.SetActive(true);

                        //  if (heartZoom.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                        // {
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(Application.loadedLevel + 1);
                       // }
                        //WhichInstructScreen = 1;
                        //Controls.gameObject.SetActive(true);

                        break;

                    case 2:
                        //Application.LoadLevel(3);
                        break;

                    case 3:
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(-1);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void ColorSelectedText()
    {
        switch (choice)
        {
            case 1:
                option1.color = Color.black;
                option2.color = Color.white;
                option3.color = Color.white;
                break;

            case 2:
                option1.color = Color.white;
                option2.color = Color.black;
                option3.color = Color.white;
                break;

            case 3:
                option1.color = Color.white;
                option2.color = Color.white;
                option3.color = Color.black;
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
                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(Application.loadedLevel + 1);
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
