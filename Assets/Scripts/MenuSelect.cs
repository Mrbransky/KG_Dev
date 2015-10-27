using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour {

	public Text option1;
	public Text option2;
	public Text option3;

    public Text instructions;

    public AudioSource audio;

	int choice = 1;
	// Use this for initialization
	void Start () {
        instructions.GetComponent<Text>().enabled = false;

        audio.loop = true;
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {

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
                instructions.GetComponent<Text>().enabled = true;
				break;
			default:
				break;
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (choice >=3)
			{
				choice = 1;
			}
			else{
			choice++;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (choice <=1)
			{
				choice = 3;
			}
			else{
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
