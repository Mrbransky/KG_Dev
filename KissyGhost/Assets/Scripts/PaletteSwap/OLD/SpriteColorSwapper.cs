using UnityEngine;
using System.Collections;

public class SpriteColorSwapper : MonoBehaviour 
{
    public Color[] OriginalColors;
    public Color[] TargetColors;

    // The sprite sheet(s) must be located in the Resources folder
    public string SpriteSheetPath;
    public string SpriteSheetName;

    public Texture2D SpriteTexture2D;

    private Sprite[] characterSprites;
    private string[] spriteNames;

    void Awake()
    {
        characterSprites = Resources.LoadAll<Sprite>(SpriteSheetPath + SpriteSheetName);
        spriteNames = new string[characterSprites.Length];

        updateTexture();
    }

    void Update()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < characterSprites.Length; ++i)
        {
            if (characterSprites[i].name == sprite.name)
            {
                GetComponent<SpriteRenderer>().sprite = characterSprites[i];
            }
        }
    }

    private void updateTexture()
    {
        Sprite[] loadSprite = Resources.LoadAll<Sprite>(SpriteSheetPath + SpriteSheetName);

        if (loadSprite.Length == 0)
        {
            return;
        }

        SpriteTexture2D = getNewTexture2D(loadSprite[0].texture);

        for (int i = 0; i < characterSprites.Length; ++i)
        {
            string tempName = characterSprites[i].name;
            characterSprites[i] = Sprite.Create(SpriteTexture2D, characterSprites[i].rect, new Vector2(0, 1));
            characterSprites[i].name = tempName;
            spriteNames[i] = tempName;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.mainTexture = SpriteTexture2D;
        sr.material.shader = Shader.Find("Sprites/Default");
        sr.material.color = new Color(1, 1, 1, 1);
    }

    private Texture2D getNewTexture2D(Texture2D copiedTexture)
    {
        Texture2D newTexture = new Texture2D(copiedTexture.width, copiedTexture.height);
        newTexture.filterMode = FilterMode.Point;
        newTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixelColors = copiedTexture.GetPixels(0, 0, copiedTexture.width, copiedTexture.height);

        for (int pixelColorIndex = 0; pixelColorIndex < pixelColors.Length; ++pixelColorIndex)
        {
            for (int originalColorIndex = 0; originalColorIndex < OriginalColors.Length; ++originalColorIndex)
            {
                if (pixelColors[pixelColorIndex] == OriginalColors[originalColorIndex])
                {
                    pixelColors[pixelColorIndex] = TargetColors[originalColorIndex];
                    break;
                }
            }
        }

        newTexture.SetPixels(pixelColors);
        newTexture.name = (SpriteSheetName + "test");
        newTexture.Apply();

        return newTexture;
    }
}
