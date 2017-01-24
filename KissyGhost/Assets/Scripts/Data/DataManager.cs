using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataManager {

    //Character Unlocks
    static bool[] UnlockableCharacters = {false,false,false};
    public static int[] UnlockedFemaleSpriteColors = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1};
    public static int[] UnlockedMaleSpriteColors = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1};
}

/*
    Female:
 0: UNDEAD
 1: WONDERWOMAN
 2: BATGIRL
 3: MDMDRACULA
 4: STONE
 5: RAINBOW
 6: JOURNEY
 7: WIDOWMAKER
 8: ZARYA
 9: SYMMETRA
 10: DEFAULT

    Male:
 0: UNDEAD
 1: CAPMURICA
 2: GANDALF
 3: VIVI
 4: BRUCELEE
 5: RAINBOW
 6: BBALIEN
 7: LUCIO
 8: HANZO
 9: REAPER
 10: DEFAULT


Potential unlock bundles:
    Overwatch bundle
    Rainbow bundle
    Superhero bundle
     */
