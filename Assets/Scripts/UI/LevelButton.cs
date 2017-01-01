using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelButton : MonoBehaviour
{

    private int currentLevelDiamonds;
    private int globalDiamonds;
    private Button levelButton;

    [SerializeField]
    private Text diamondsInform;
    [SerializeField]
    private int diamondsToOpen;
    [SerializeField]
    private int diamondsOnLevel;
    [SerializeField]
    private int levelNumber;


    void Start()
    {
        globalDiamonds = Market.Instance.Dimond;
        currentLevelDiamonds = GameController.Instance.levelsData[levelNumber].diamondsCollected;

        levelButton = GetComponent<Button>();
        if (globalDiamonds >= diamondsToOpen)       //если уровень открыт
        {
            levelButton.interactable = true;
            diamondsInform.text = currentLevelDiamonds.ToString() + "/" + diamondsOnLevel.ToString();
        }
        else        //если уровень не доступен
        {
            levelButton.interactable = false;
            diamondsInform.text = (diamondsToOpen - globalDiamonds).ToString() + " More To Open";
        }
    }

    void LoadGameLevel(string name)
    {
        GameController.Instance.CurrentLevel = levelNumber;

        GameController.Instance.LoadScene(name);
    }
}
