using UnityEngine;
using System.Collections;

public class CharacterSelectDemoController : MonoBehaviour
{
    [SerializeField] private int playerNumber = -1;

    void Start()
    {
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");
        if (characterSelectData != null)
        {
            bool isPlayerReady = characterSelectData.GetComponent<CharacterSelectData>().GetIsPlayerReady(playerNumber);
            if (!isPlayerReady)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("CharacterSelectDemoController: Could not find game object with tag \"CharacterSelectData\"");
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetButton("P" + playerNumber + "pad_A"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
