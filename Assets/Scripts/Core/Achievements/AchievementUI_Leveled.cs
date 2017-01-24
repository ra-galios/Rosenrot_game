
using UnityEngine;
using System.Collections;

public class AchievementUI_Leveled : AchievementUI_Base
{

    public float m_TimeBeforeFirstShow = 1f;
    public float m_TimeBetweenShows = 1f;

    public override void Show()
    {
        base.Show();

        if(GameController.Instance.AchievementsToShow.Count > 0)
        {
            StartCoroutine(ShowCoroutine());
        }
    }

    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(m_TimeBeforeFirstShow);

        Animator anim = GetComponent<Animator>();

        for (int i = 0; i < GameController.Instance.AchievementsToShow.Count; i++)
        {
            SetFields(GameController.Instance.AchievementsToShow[i]);
            GetReward(GameController.Instance.AchievementRevards.Achievements[i].m_RevardType);
            
            anim.SetTrigger("Achievement");

            yield return new WaitForSeconds(m_TimeBetweenShows);
        }
    }

    protected override void GetReward(AchievementsController.RewardType rewardType)
    {
        base.GetReward(rewardType);
    }
}
