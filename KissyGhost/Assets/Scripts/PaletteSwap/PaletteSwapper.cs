using UnityEngine;
using System.Collections;

public class PaletteSwapper : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public ColorPalette[] paletteRandSelection;

	private MaterialPropertyBlock block;

	void Awake()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () 
    {
		if (paletteRandSelection.Length > 0)
        {
            SwapColors_Custom(paletteRandSelection[Random.Range(0, paletteRandSelection.Length)]);
        }
	}

    void LateUpdate()
    {
        spriteRenderer.SetPropertyBlock(block);
        
    }

    void SwapColors_Custom(ColorPalette palette)
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
        block.AddTexture("_MainTex", palette.cachedTexture);
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
        block.AddTexture("_MainTex", palette.cachedTexture);
    }
}
