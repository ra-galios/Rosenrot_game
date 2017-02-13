using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    private bool endGame = false;
    private AchievementUI_Leveled LevelAchievementPanel;
    private Victory m_VictoryObj;


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
                AchievementsController.AddToAchievement(AchievementsController.Type.SelfDestructive, 1);
                AchievementsController.DiscardAchievement(AchievementsController.Type.Survivor);
                Destroy(player);
                StartCoroutine("StopGame");
            }
        }
    }

    private IEnumerator StopGame()
    {
        yield return new WaitForSeconds(0.4f);
        if (LevelAchievementPanel)
            LevelAchievementPanel.ShowBonusPanel(false);
        else
            GameController.Instance.FailGame();
    }
}
