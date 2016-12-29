using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelButton : MonoBehaviour
{

    private int currentLevelDiamonds;
    private int globalDiamonds;
    private Button levelButton;

    public Text levelNumber;
    public Text diamondsInform;
    public int diamondsToOpen;
    public int diamondsOnLevel;

    // Use this for initialization
    void OnEnable()
    {
        globalDiamonds = Market.Instance.Dimond;
        //currentLevelDiamonds = GameController.Instance.levelsData[Int32.Parse(levelNumber.text) - 1].diamondsCollected;
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
}
