using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour {

	public Image option1;
	public Image option2;
    public Image option3;

    public Text Credits;

    public AudioSource audio;

    float timer = 0.4f;

    bool canMove = false;

	int choice = 1;
	// Use this for initialization
	void Start () {
        Credits.GetComponent<Text>().enabled = false;

        audio.loop = true;
        audio.Play();
	}
	// Update is called once per frame

	void Update () {

        if (canMove == false)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canMove = true;
                timer = 0.4f;
            }
        }
        float JoystickMove = Input.GetAxisRaw("Menu");
		if (Input.GetKeyDown (KeyCode.Space)) {
			switch(choice)
			{
			case 1:
				Application.LoadLevel(1);
				break;
			case 2:
                Application.Quit();
				break;
			case 3:
                Credits.GetComponent<Text>().enabled = true;
				break;
			default:
				break;
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) || JoystickMove == 1 && canMove == true) {
			if (choice >=3)
			{
                canMove = false;
				choice = 1;
			}
			else{
            canMove = false;
			choice++;
			}
		}
        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickMove == -1 && canMove == true)
        {
			if (choice <=1)
			{
                canMove = false;
				choice = 3;
			}
			else{
                canMove = false;
				choice--;
			}
		}

		switch (choice) {
		
		case 1:
			option1.color = Color.green;
			option2.color = Color.white;
			option3.color = Color.white;
			break;
		case 2:
			option1.color = Color.white;
			option2.color = Color.green;
			option3.color = Color.white;
			break;
		case 3:
			option1.color = Color.white;
			option2.color = Color.white;
			option3.color = Color.green;
			break;
		default:
			break;
		}
	
	}
}
