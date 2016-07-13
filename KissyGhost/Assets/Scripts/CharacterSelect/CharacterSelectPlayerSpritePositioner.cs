using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectPlayerSpritePositioner : MonoBehaviour 
{
    public Image playerSpriteReferencePointImage;

    void Start()
    {
        playerSpriteReferencePointImage.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
