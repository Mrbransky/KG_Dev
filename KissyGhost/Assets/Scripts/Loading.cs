using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour {

    private Text loadingText;
    public float timeBetweenChange;
    private float currentTime;
    private string startingText;

	// Use this for initialization
	void Start () 
    {
        startingText = "Loading";
        loadingText = GetComponent<Text>();

        loadingText.text = startingText;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = timeBetweenChange;
            loadingText.text = ChangeLoadingText(loadingText.text);
        }

    }

    string ChangeLoadingText(string stringToChange)
    {
        switch(stringToChange)
        {
            case "Loading":
                return "Loading.";

            case "Loading.":
                return "Loading..";

            case "Loading..":
                return "Loading...";

            case "Loading...":
                return "Loading";

            default:
                return "Loading";
        }
    }
}
