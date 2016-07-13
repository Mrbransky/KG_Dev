using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class STUPIDCATS : MonoBehaviour {

    GameObject[] allObjects;
    public Sprite catSprite;
	void LateUpdate () {
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<SpriteRenderer>())
            {
                obj.GetComponent<SpriteRenderer>().sprite = catSprite;
            }
        }
	}
}
