using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataLoading : MonoBehaviour {

    public void SaveGameData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/KG/PlayerData.dat");
        PlayerData data = new PlayerData();

        //stuff
        data.UnlockedFemaleSpriteColors_ = DataManager.UnlockedFemaleSpriteColors;
        data.UnlockedMaleSpriteColors_ = DataManager.UnlockedMaleSpriteColors;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("file was saved");
    }
    public void LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/KG/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/KG/PlayerData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);

            //stuff
            DataManager.UnlockedFemaleSpriteColors = data.UnlockedFemaleSpriteColors_;
            DataManager.UnlockedMaleSpriteColors = data.UnlockedMaleSpriteColors_;

            file.Close();
            Debug.Log("File was loaded");
        }
        else { Debug.Log("File was not loaded"); }

    }

}

[Serializable]
class PlayerData
{
    //Unlocks
    public bool[] UnlockableCharacters_;
    public int[] UnlockedFemaleSpriteColors_;
    public int[] UnlockedMaleSpriteColors_;
}