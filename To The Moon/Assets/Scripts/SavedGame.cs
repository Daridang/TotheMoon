[System.Serializable]
public class SavedGame
{
    public int StarBonus { get; set; } = 0;
    public string LeveName { get; set; } = "Level1";
    public StoreItem.RocketType RocketType { get; set; } = StoreItem.RocketType.TYPE_1;
}