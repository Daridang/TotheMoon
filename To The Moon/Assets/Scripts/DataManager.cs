using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private SavedGame CreateSaveGameObject()
    {
        SavedGame save = new SavedGame();
        save.LeveName = GameManager.Instance.GetLevelName();
        //save.StarBonus = UIManager.Instance
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

    public void LoadGame()
    {
        // 1
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //ClearBullets();
            //ClearRobots();
            //RefreshRobots();

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SavedGame save = (SavedGame)bf.Deserialize(file);
            file.Close();

            // 3
            //for(int i = 0; i < save.livingTargetPositions.Count; i++)
            //{
            //    int position = save.livingTargetPositions[i];
            //    Target target = targets[position].GetComponent<Target>();
            //    target.ActivateRobot((RobotTypes)save.livingTargetsTypes[i]);
            //    target.GetComponent<Target>().ResetDeathTimer();
            //}

            //// 4
            //shotsText.text = "Shots: " + save.shots;
            //hitsText.text = "Hits: " + save.hits;
            //shots = save.shots;
            //hits = save.hits;

            Debug.Log("Game Loaded");

            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
