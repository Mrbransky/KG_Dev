using UnityEngine;
using System.Collections;

public class HumanSpriteFlasher : MonoBehaviour 
{
    public Color DefaultColor = Color.white;
    public Color FlashColorA = Color.white;
    public Color FlashColorB = Color.white;
    private float FlashRate = 0.07f;

    private SpriteRenderer spriteRenderer;
    private bool isFlashColorA = false;

    IEnumerator flashSprite()
    {
        while (true)
        {
            isFlashColorA = !isFlashColorA;

            if (isFlashColorA)
            {
                spriteRenderer.color = FlashColorA;
            }
            else
            {
                spriteRenderer.color = FlashColorB;
            }

            yield return new WaitForSeconds(FlashRate);
        }
    }

	void Start () 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    public void StartFlashing()
    {
        StartCoroutine("flashSprite");
    }

    public void StopFlashing()
    {
        StopCoroutine("flashSprite");
        spriteRenderer.color = DefaultColor;
    }
}
