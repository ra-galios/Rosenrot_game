using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {

    private List<int> achievementsToShow = new List<int>();

    void OnEnable()
    {
        PlayerBehaviour.PlayerChangeLine += CheckWin;
    }

    void OnDisable()
    {
        PlayerBehaviour.PlayerChangeLine -= CheckWin;
    }

    void CheckWin(int playerIdLine)
    {
        if (playerIdLine == LevelGenerator.Instance.MaxLines)
        {
            GameObject achievementsPrefab = Resources.Load("Achievements", typeof(GameObject)) as GameObject;


            GameController.Instance.WinGame();
        }
    }
}
