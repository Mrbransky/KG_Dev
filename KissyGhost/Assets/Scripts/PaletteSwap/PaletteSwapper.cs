using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PaletteSwapper : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	//public ColorPalette[] paletteRandSelection;
    public ColorPalette currentPalette;
    public Text playerNumText;

	private MaterialPropertyBlock block;

	void Awake()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () 
    {
        if(currentPalette != null)
            SwapColors_Custom(currentPalette);

		//if (paletteRandSelection.Length > 0)
        //{
            //SwapColors_Custom(paletteRandSelection[Random.Range(0, paletteRandSelection.Length)]);
            //SwapColors_Custom(paletteRandSelection[positionInPaletteArray]);
            
        //}
	}

    void LateUpdate()
    {
        spriteRenderer.SetPropertyBlock(block);
        
    }

    public void SwapColors_Custom(ColorPalette palette)
    {
        Texture2D t = spriteRenderer.sprite.texture;

        if (palette.cachedTexture == null)
        {
            var w = t.width;
            var h = t.height;

            var cloneTexture = new Texture2D(w, h);
            cloneTexture.wrapMode = TextureWrapMode.Clamp;
            cloneTexture.filterMode = FilterMode.Point;

            var colors = t.GetPixels();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = palette.GetColor(colors[i]);
            }

            cloneTexture.SetPixels(colors);
            cloneTexture.Apply();

            palette.cachedTexture = cloneTexture;
        }

        block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", palette.cachedTexture);
    }

    void SwapColors_Original(ColorPalette palette)
    {
        if (palette.cachedTexture == null)
        {
            Texture2D texture = spriteRenderer.sprite.texture;

            var w = texture.width;
            var h = texture.height;

            var cloneTexture = new Texture2D(w, h);
            cloneTexture.wrapMode = TextureWrapMode.Clamp;
            cloneTexture.filterMode = FilterMode.Point;

            var colors = texture.GetPixels();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = palette.GetColor(colors[i]);
            }

            cloneTexture.SetPixels(colors);
            cloneTexture.Apply();

            palette.cachedTexture = cloneTexture;
        }

        block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", palette.cachedTexture);
    }

    public void UpdatePlayerNumTextColor()
    {
        int targetPaletteIndex = (int)char.GetNumericValue(currentPalette.name[0]);
        playerNumText.color = currentPalette.newPalette[targetPaletteIndex];
    }

    public void UpdatePlayerNumTextColor(Color col)
    {
        playerNumText.color = col;
    }
}
