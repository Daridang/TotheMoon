using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    private SavedGame CreateSaveGameObject()
    {
        SavedGame save = new SavedGame();
        //save.LeveName = "CurrentUnlockedLevelName";
        //save.RocketType = StoreItem.RocketType.TYPE_1;
        //save.StarBonus = 0;
        return save;
    }

    public void SaveGame()
    {
        // 1
        SavedGame save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        // 3
        //hits = 0;
        //shots = 0;
        //shotsText.text = "Shots: " + shots;
        //hitsText.text = "Hits: " + hits;

        //ClearRobots();
        //ClearBullets();
        Debug.Log("Game Saved");
    }
}
