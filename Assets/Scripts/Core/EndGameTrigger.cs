using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    //private bool endGame = false;
    private AchievementUI_Leveled LevelAchievementPanel;
    private Victory m_VictoryObj;

    [SerializeField]
    private Animator m_AdsFailAnimator;
    private bool needRunAdsPanel = true;

    private void Start()
    {
        LevelAchievementPanel = FindObjectOfType<AchievementUI_Leveled>();
        m_VictoryObj = FindObjectOfType<Victory>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerBehaviour>();

        if (player)
        {
            if (m_VictoryObj.OnBonusLevel)
            {
                m_VictoryObj.SetWin();
            }
            else
            {
                player.enabled = false;
                if (needRunAdsPanel)
                {
                    StartCoroutine("RunAdsPanel");
                }
                else
                {
                    AchievementsController.AddToAchievement(AchievementsController.Type.SelfDestructive, 1);
                    AchievementsController.DiscardAchievement(AchievementsController.Type.Survivor);
                    StartCoroutine("StopGame");
                }
            }
        }
    }

    private IEnumerator RunAdsPanel()
    {
        yield return new WaitForSeconds(0.1f);
        m_AdsFailAnimator.SetTrigger("Fail");
        needRunAdsPanel = false;
        GameController.Instance.PauseGame();
    }

    private IEnumerator StopGame()
    {
        yield return new WaitForSeconds(0.1f);

        if (LevelAchievementPanel)
            LevelAchievementPanel.ShowBonusPanel(false);
        else
            GameController.Instance.FailGame();
    }
}
