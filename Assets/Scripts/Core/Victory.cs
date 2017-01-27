using UnityEngine;
using System;
using System.Collections.Generic;

public class Victory : MonoBehaviour
{

    //public static List<int> achievementsToShow = new List<int>();
    public AchievementUI_Base LevelAchievementPanel;

    void OnEnable()
    {
        PlayerBehaviour.PlayerChangeLine += CheckWin;
    }

    void OnDisable()
    {
        PlayerBehaviour.PlayerChangeLine -= CheckWin;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AchievementsController.AddToAchievement(AchievementsController.Type.RubyRubyRubyRuby, 1);
            AchievementsController.AddToAchievement(AchievementsController.Type.EverybodysBestFriend, 1);
            //DataManager.Instance.SetAchievement((int)AchievementsController.Type.RubyRubyRubyRuby, 0);
            //DataManager.Instance.SetAchievement((int)AchievementsController.Type.EverybodysBestFriend, 0);

            print("showList length: " + GameController.Instance.AchievementsToShow.Count);

            print("ach value: " + AchievementsController.GetAchievement(AchievementsController.Type.RubyRubyRubyRuby));
        }
    }

    void CheckWin(int playerIdLine)
    {
        if (playerIdLine == LevelGenerator.Instance.MaxLines)
        {
            Debug.Log("WinGame");

            if (LevelAchievementPanel)
                LevelAchievementPanel.Show();

            GameController.Instance.WinGame();
        }
    }
}