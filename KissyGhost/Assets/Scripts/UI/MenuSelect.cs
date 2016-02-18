using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{

    public Image option1, option2, option3;
    public AudioSource audio;
    Transform Controls, Instructions, WinCondition;

    float menuSelectTimer = 0.2f;
    float instructScreensTimer = 1;

    bool canMove = false;
    bool HasSelectedPlay = false;
    bool canAdvance = false;

    int choice = 1;
    int WhichInstructScreen = 0;

    void Awake()
    {
        Controls = transform.Find("Controls");
        Instructions = transform.Find("Instructions");
        WinCondition = transform.Find("WinCondition");

        Controls.gameObject.SetActive(false);
        Instructions.gameObject.SetActive(false);
        WinCondition.gameObject.SetActive(false);
    }

    void Update()
    {
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
            if (!HasSelectedPlay)
            {
                switch (choice)
                {
                    case 1:
                        HasSelectedPlay = true;

                        Application.LoadLevel(1);
                        //WhichInstructScreen = 1;
                        //Controls.gameObject.SetActive(true);

                        break;

                    case 2:
                        //Application.LoadLevel(3);
                        break;

                    case 3:
                        Application.Quit();
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
                        Application.LoadLevel(1);
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
