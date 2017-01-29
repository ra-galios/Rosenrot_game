using UnityEngine;
using System.IO;
using System;

public class DataManager : CreateSingletonGameObject<DataManager>
{

    [SerializeField]
    private GameData gameData;
    private string saveFileName = "gameData.json";

    public void SaveGameData()
    {
        string outputPath = Application.persistentDataPath + "/" + saveFileName;     //путь к файлу сохранений
        GetData();                                                                          //получаем данные

        StreamWriter writer = new StreamWriter(outputPath);
        writer.WriteLine(JsonUtility.ToJson(gameData));                                     //записываем в файл
        writer.Close();
    }

    public void LoadGameData()
    {
        string outputPath = Application.persistentDataPath + "/" + saveFileName;

        if (File.Exists(outputPath))
        {
            StreamReader reader = new StreamReader(outputPath);
            string jsonString = reader.ReadToEnd();
            gameData = JsonUtility.FromJson<GameData>(jsonString);                  //забираем данные из файла
            SetData();                                                              //записываем данные в переменные
            reader.Close();
        }
        if (gameData == null)
        {
            gameData = new GameData();
            gameData.LevelsData = new LevelData[GameController.Instance.TotalGameLevels];
            SetData();
            Market.Instance.Health = 5;
        }
    }

    private void SetData()
    {
        Market.Instance.CurentlyAddHealth = gameData.curentlyAddHealth;
        Market.Instance.Health = gameData.health;
        Market.Instance.Seeds = gameData.seeds;
        Market.Instance.Bomb = gameData.bombs;
        Market.Instance.Dimond = gameData.diamonds;
        Market.Instance.Ruby = gameData.rubies;
        if (gameData.LevelsData.Length != GameController.Instance.TotalGameLevels)
        {
            Array.Resize(ref gameData.LevelsData, GameController.Instance.TotalGameLevels);
            GameController.Instance.LevelsData = gameData.LevelsData;
        }
        else
        {
            GameController.Instance.LevelsData = gameData.LevelsData;
        }
    }

    private void GetData()
    {
        gameData.curentlyAddHealth = Market.Instance.CurentlyAddHealth;
        gameData.health = Market.Instance.Health;
        gameData.seeds = Market.Instance.Seeds;
        gameData.bombs = Market.Instance.Bomb;
        gameData.diamonds = Market.Instance.Dimond;
        gameData.rubies = Market.Instance.Ruby;
        gameData.LevelsData = GameController.Instance.LevelsData;
    }

    public void SetAchievement(int index, int val)
    {
        if (index > (gameData.Achievements.Length - 1))
        {
            Array.Resize(ref gameData.achievements, index + 1);
        }
        gameData.Achievements[index] = val;
    }

    public int GetAchievement(int index)
    {
        if (index > (gameData.Achievements.Length - 1))
        {
            Array.Resize(ref gameData.achievements, index + 1);
        }
        return gameData.Achievements[index];
    }

    public int GetAchievementsDataLength()
    {
        return gameData.Achievements.Length;
    }

#if UNITY_EDITOR
    public void ClearData()
    {
        gameData = new GameData();
        gameData.LevelsData = new LevelData[GameController.Instance.TotalGameLevels];

        SetData();
    }
#endif
}
