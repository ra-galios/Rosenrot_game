
using UnityEngine;
using System.Collections;

public class AchievementUI_Leveled : AchievementUI_Base
{

    public float m_TimeBeforeFirstShow = 1f;
    public float m_TimeBetweenShows = 1f;

    public override void Show()
    {
        base.Show();

        print("there");

        if(GameController.Instance.AchievementsToShow.Count > 0)
        {
            StartCoroutine(ShowCoroutine());
        }
    }

    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSecondsRealtime(m_TimeBeforeFirstShow);

        Animator anim = GetComponent<Animator>();

        for (int i = 0; i < GameController.Instance.AchievementsToShow.Count; i++)
        {
            SetFields(GameController.Instance.AchievementsToShow[i]);

            anim.SetTrigger("Achievement");

            //yield return new WaitUntil;           //разобраться как работает
        }

        GameController.Instance.AchievementsToShow.Clear();
    }
}
