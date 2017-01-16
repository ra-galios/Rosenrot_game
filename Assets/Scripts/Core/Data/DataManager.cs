using UnityEngine;
using System.IO;
using System;

public class DataManager : CreateSingletonGameObject<DataManager>
{

    [SerializeField, HideInInspector]
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
            gameData.LevelsData = new LevelData[GameController.Instance.totalGameLevels];
            SetData();
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
        if(gameData.LevelsData.Length != GameController.Instance.totalGameLevels)
        {
            Array.Resize(ref gameData.LevelsData, GameController.Instance.totalGameLevels);
            GameController.Instance.levelsData = gameData.LevelsData;
        }
        else
        {
            GameController.Instance.levelsData = gameData.LevelsData;
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
        gameData.LevelsData = GameController.Instance.levelsData;
    }
}
