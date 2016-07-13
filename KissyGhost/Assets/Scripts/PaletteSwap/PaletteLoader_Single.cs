using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PaletteLoader_Single : MonoBehaviour
{

    private List<ColorPalette> palettesToLoad;
    private Random rand;
    public PaletteSwapper palSwap;

    public Sprite OldieStartSprite;
    public Sprite WomanStartSprite;

    void Start()
    {
        GameObject CharSelectMan_GO = GameObject.Find("CharacterSelectManager");
        palettesToLoad = CharSelectMan_GO.GetComponent<CharacterSelectManager_Single>().AvailablePalettesList;
        int[] playerPosInPalettesList = CharSelectMan_GO.GetComponent<CharacterSelectManager_Single>().PlayerPosInPaletteList;

        palSwap.currentPalette = palettesToLoad[0];

        foreach (ColorPalette colPal in palettesToLoad)
        {
            if (colPal.name.Contains("Lady") && palSwap.spriteRenderer.sprite != WomanStartSprite)
                palSwap.spriteRenderer.sprite = WomanStartSprite;

            else if (colPal.name.Contains("Man") && palSwap.spriteRenderer.sprite != OldieStartSprite)
                palSwap.spriteRenderer.sprite = OldieStartSprite;

            palSwap.SwapColors_Custom(colPal);
        }

        List<PaletteSwapper> playerPaletteSwappers = CharSelectMan_GO.GetComponent<CharacterSelectManager_Single>().PlayerPaletteSwapperArray.ToList();
        List<ColorPalette> tempPalettes = new List<ColorPalette>();

        foreach (ColorPalette colPal in palettesToLoad)
        {
            tempPalettes.Add(colPal);
        }

        for (int i = 0; i < playerPaletteSwappers.Count; i++)
        {
            int rand = Random.Range(0, tempPalettes.Count);
            ColorPalette randomPalette = tempPalettes[rand];
            playerPaletteSwappers[i].currentPalette = randomPalette;

            playerPosInPalettesList[i] = palettesToLoad.FindIndex(a => a == randomPalette);

            if (playerPaletteSwappers[i].currentPalette.name.Contains("Lady") && !playerPaletteSwappers[i].GetComponent<Animator>().GetBool("IsWoman"))
                playerPaletteSwappers[i].GetComponent<Animator>().SetBool("IsWoman", true);

            if (!playerPaletteSwappers[i].currentPalette.name.Contains("Lady") && playerPaletteSwappers[i].GetComponent<Animator>().GetBool("IsWoman"))
                playerPaletteSwappers[i].GetComponent<Animator>().SetBool("IsWoman", false);

            tempPalettes.Remove(randomPalette);
        }

    }

}
