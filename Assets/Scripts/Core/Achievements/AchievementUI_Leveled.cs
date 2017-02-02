
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchievementUI_Leveled : AchievementUI_Base
{

    [SerializeField]
    public Button hidePanelButton;
    [SerializeField]
    private float m_TimeBeforeFirstShow = 1f;
    [SerializeField]
    public float m_TimeBetweenShows = 1f;

    public override void Show()
    {
        base.Show();

        GameController.Instance.PauseGame();

        if (GameController.Instance.AchievementsToShow.Count > 0)
        {
            StartCoroutine(ShowCoroutine());
        }
        else
        {
            GameController.Instance.WinGame();
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

            yield return new WaitUntil(() => EventSystem.current.currentSelectedGameObject == hidePanelButton.gameObject);
            yield return new WaitForSecondsRealtime(m_TimeBetweenShows);
        }

        GameController.Instance.AchievementsToShow.Clear();
        GameController.Instance.WinGame();
    }
}

