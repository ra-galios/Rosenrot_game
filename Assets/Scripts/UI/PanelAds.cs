using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Advertisements;

public class PanelAds : AchievementUI_Base
{
    private AchievementUI_Leveled LevelAchievementPanel;
    private Coroutine ads;

    private void Start()
    {
        LevelAchievementPanel = FindObjectOfType<AchievementUI_Leveled>();
    }

    public void RestartAfterFall()
    {
        if (ads == null)
            ads = StartCoroutine(StartAds());
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

    IEnumerator StartAds()
    {
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Show();
        ads = null;

        while (!Advertisement.isShowing)
        {
            yield return null;
        }

        gameObject.SetActive(false);

        PlayerBehaviour player = GameObject.FindObjectOfType<PlayerBehaviour>();
        player.enabled = true;
        GameController.Instance.ResumeGame();
        player.GrabAfterFall();
    }
}
