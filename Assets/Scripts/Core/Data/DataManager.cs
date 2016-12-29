using UnityEngine;
using System.IO;

public class DataManager : CreateSingletonGameObject<DataManager>
{

    public GameData gameData;
    private string saveFileName = "gameData.json";

    public void SaveGameData()
    {
        string outputPath = Path.Combine(Application.persistentDataPath, saveFileName);     //путь к файлу сохранений
        GetData();                                                                          //получаем данные

        StreamWriter writer = new StreamWriter(outputPath);
        writer.WriteLine(JsonUtility.ToJson(gameData));                                     //записываем в файл
        writer.Close();
    }

    public void LoadGameData()
    {
        string outputPath = Path.Combine(Application.persistentDataPath, saveFileName);

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
        }
    }

    private void SetData()
    {
        Market.Instance.Seeds = gameData.seeds;
        Market.Instance.Bomb = gameData.bombs;
        Market.Instance.Dimond = gameData.diamonds;
        Market.Instance.Ruby = gameData.rubies;
        GameController.Instance.levelsData = gameData.LevelsData;
    }

    private void GetData()
    {
        gameData.seeds = Market.Instance.Seeds;
        gameData.bombs = Market.Instance.Bomb;
        gameData.diamonds = Market.Instance.Dimond;
        gameData.rubies = Market.Instance.Ruby;
        gameData.LevelsData = GameController.Instance.levelsData;
    }
}
