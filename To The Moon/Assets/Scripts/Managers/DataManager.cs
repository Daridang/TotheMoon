using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
   
    private RocketsArray _array;

    private const string SELECTED_ROCKET_INDEX = "SelectedRocketIndex";
    private const string LAST_UNLOCKED_LEVEL = "LastUnlockedLevel";

    private const string STARBONUS_COUNT = "StarBonusCount";

    private const string ROCKET_01 = "Rocket_01(Clone)";
    private const string ROCKET_02 = "Rocket_02(Clone)";
    private const string ROCKET_03 = "Rocket_03(Clone)";

    private const string LEVEL_01 = "Level1";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _array = GameManager.Instance.GetComponent<RocketsArray>();
        if(!PlayerPrefs.HasKey("HasPlayedGameBefore"))
        {
            PlayerPrefs.SetInt("HasPlayedGameBefore", 0);
            PlayerPrefs.SetInt(STARBONUS_COUNT, 0);
            PlayerPrefs.SetInt(SELECTED_ROCKET_INDEX, 0);

            PlayerPrefs.SetInt(LEVEL_01, 1);
            PlayerPrefs.SetInt(LAST_UNLOCKED_LEVEL, 1);

            PlayerPrefs.SetInt(ROCKET_01, 1);
            PlayerPrefs.SetInt(ROCKET_02, 0);
            PlayerPrefs.SetInt(ROCKET_03, 0);
        }
    }

    public int CheckLevelIsUnlocked(string name)
    {
        return PlayerPrefs.GetInt(name, 0);
    }

    public void SetLevelUnlocked(string name)
    {
        PlayerPrefs.SetInt(name, 1);
    }

    public int CheckRocketIsUnlocked(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    public void SetUnlocked(string name)
    {
        PlayerPrefs.SetInt(name, 1);
    }

    public int StarBonus
    {
        get
        {
            return PlayerPrefs.GetInt(STARBONUS_COUNT);
        }
        set
        {
            PlayerPrefs.SetInt(STARBONUS_COUNT, value);
        }
    }

    public int SelectedRocketIndex
    {
        get
        {
            return PlayerPrefs.GetInt(SELECTED_ROCKET_INDEX);
        }
        set
        {
            PlayerPrefs.SetInt(SELECTED_ROCKET_INDEX, value);
        }
    }

    public int LastUnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetInt(LAST_UNLOCKED_LEVEL);
        }
        set
        {
            PlayerPrefs.SetInt(LAST_UNLOCKED_LEVEL, value);
        }
    }

    private SavedGame CreateSaveGameObject()
    {
        SavedGame save = new SavedGame();
        //save.LeveName = GameManager.Instance.GetLevelName();
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
