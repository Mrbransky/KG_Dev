using UnityEngine;
using System.Collections;

public class HumanDeath : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeSpeed = 1.2f;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
	    if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            newColor.a -= fadeSpeed * Time.deltaTime;

            spriteRenderer.color = newColor;

            if (newColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
	}
}
