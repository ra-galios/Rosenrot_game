using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Advertisements;

public class PanelAds : AchievementUI_Base
{
    private AchievementUI_Leveled LevelAchievementPanel;
    private Coroutine ads;
    private bool needShowAds;
    private bool needRestart;
    private DateTime needRestartTime;

    private void Start()
    {
        LevelAchievementPanel = FindObjectOfType<AchievementUI_Leveled>();
    }

    private void Update()
    {
        if (needShowAds)
        {
            if (Advertisement.isInitialized && Advertisement.IsReady())
            {
                Advertisement.Show();
                needShowAds = false;
                needRestart = true;
                needRestartTime = DateTime.Now;
            }
        }
        if (needRestart && (DateTime.Now - needRestartTime).Seconds > 1)
        {
            gameObject.SetActive(false);

            PlayerBehaviour player = GameObject.FindObjectOfType<PlayerBehaviour>();
            player.enabled = true;
            GameController.Instance.ResumeGame();
            player.GrabAfterFall();
        }
    }

    public void RestartAfterFall()
    {
        needShowAds = true;
    }

    public void SetFailGame()
    {
        AchievementsController.AddToAchievement(AchievementsController.Type.SelfDestructive, 1);
        AchievementsController.DiscardAchievement(AchievementsController.Type.Survivor);
        if (LevelAchievementPanel)
            LevelAchievementPanel.ShowBonusPanel(false);
        else
            GameController.Instance.FailGame();
    }
}
