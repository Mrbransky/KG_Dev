using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PaletteLoader : MonoBehaviour 
{
    private const int FEM_WIZ_TARGET_INDEX = 4;
    private const int MAN_WIZ_TARGET_INDEX = 10;

    public List<ColorPalette> palettesToChooseFrom_FemWiz;
    public List<ColorPalette> palettesToChooseFrom_ManWiz;
    private List<ColorPalette> palettesToLoad;
    private Random rand;
    public PaletteSwapper palSwap;

    public Sprite OldieStartSprite;
    public Sprite WomanStartSprite;

	void Start () 
    {
        GameObject CharSelectMan_GO = GameObject.Find("CharacterSelectManager");
        CharacterSelectManager charSelectManager = CharSelectMan_GO.GetComponent<CharacterSelectManager>();

        if (charSelectManager != null)
        {
            Debug.Log("charSelectManager loaded");

            int randIndex = Random.Range(0, palettesToChooseFrom_FemWiz.Count);
            charSelectManager.AvailablePalettesList[FEM_WIZ_TARGET_INDEX] = palettesToChooseFrom_FemWiz[randIndex];
            palettesToChooseFrom_FemWiz.RemoveAt(randIndex);
            randIndex = Random.Range(0, palettesToChooseFrom_FemWiz.Count);
            charSelectManager.AvailablePalettesList[FEM_WIZ_TARGET_INDEX + 1] = palettesToChooseFrom_FemWiz[randIndex];

            randIndex = Random.Range(0, palettesToChooseFrom_ManWiz.Count);
            charSelectManager.AvailablePalettesList[MAN_WIZ_TARGET_INDEX] = palettesToChooseFrom_ManWiz[randIndex];
            palettesToChooseFrom_ManWiz.RemoveAt(randIndex);
            randIndex = Random.Range(0, palettesToChooseFrom_ManWiz.Count);
            charSelectManager.AvailablePalettesList[MAN_WIZ_TARGET_INDEX + 1] = palettesToChooseFrom_ManWiz[randIndex];

            palettesToLoad = charSelectManager.AvailablePalettesList;
            int[] playerPosInPalettesList = charSelectManager.PlayerPosInPaletteList;

            palSwap.currentPalette = palettesToLoad[0];

            foreach (ColorPalette colPal in palettesToLoad)
            {
                if (colPal.name.Contains("Lady") && palSwap.spriteRenderer.sprite != WomanStartSprite)
                    palSwap.spriteRenderer.sprite = WomanStartSprite;

                else if (colPal.name.Contains("Man") && palSwap.spriteRenderer.sprite != OldieStartSprite)
                    palSwap.spriteRenderer.sprite = OldieStartSprite;

                palSwap.SwapColors_Custom(colPal);
            }

            List<PaletteSwapper> playerPaletteSwappers = charSelectManager.PlayerPaletteSwapperArray.ToList();
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
        else
        {
            CharacterSelectManager_Single charSelectManager_Single = CharSelectMan_GO.GetComponent<CharacterSelectManager_Single>();

            if (charSelectManager_Single != null)
            {
                Debug.Log("charSelectManager_Single loaded");

                int randIndex = Random.Range(0, palettesToChooseFrom_FemWiz.Count);
                charSelectManager_Single.AvailablePalettesList[FEM_WIZ_TARGET_INDEX] = palettesToChooseFrom_FemWiz[randIndex];
                palettesToChooseFrom_FemWiz.RemoveAt(randIndex);
                randIndex = Random.Range(0, palettesToChooseFrom_FemWiz.Count);
                charSelectManager_Single.AvailablePalettesList[FEM_WIZ_TARGET_INDEX + 1] = palettesToChooseFrom_FemWiz[randIndex];

                randIndex = Random.Range(0, palettesToChooseFrom_ManWiz.Count);
                charSelectManager_Single.AvailablePalettesList[MAN_WIZ_TARGET_INDEX] = palettesToChooseFrom_ManWiz[randIndex];
                palettesToChooseFrom_ManWiz.RemoveAt(randIndex);
                randIndex = Random.Range(0, palettesToChooseFrom_ManWiz.Count);
                charSelectManager_Single.AvailablePalettesList[MAN_WIZ_TARGET_INDEX + 1] = palettesToChooseFrom_ManWiz[randIndex];

                palettesToLoad = charSelectManager_Single.AvailablePalettesList;
                int[] playerPosInPalettesList = charSelectManager_Single.PlayerPosInPaletteList;

                palSwap.currentPalette = palettesToLoad[0];

                foreach (ColorPalette colPal in palettesToLoad)
                {
                    if (colPal.name.Contains("Lady") && palSwap.spriteRenderer.sprite != WomanStartSprite)
                        palSwap.spriteRenderer.sprite = WomanStartSprite;

                    else if (colPal.name.Contains("Man") && palSwap.spriteRenderer.sprite != OldieStartSprite)
                        palSwap.spriteRenderer.sprite = OldieStartSprite;

                    palSwap.SwapColors_Custom(colPal);
                }

                List<PaletteSwapper> playerPaletteSwappers = charSelectManager_Single.PlayerPaletteSwapperArray.ToList();
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
	}
}
