
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

    public void ShowBonusPanel(bool isWin)
    {
        base.Show();

        GameController.Instance.PauseGame();

        if (GameController.Instance.AchievementsToShow.Count > 0)
        {
            StartCoroutine(ShowCoroutine(isWin));
        }
        else
        {
            if (isWin)
            {
                GameController.Instance.WinGame();
            }
            else
            {
                GameController.Instance.FailGame();
            }
        }
    }

    private IEnumerator ShowCoroutine(bool isWin)
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
        if (isWin)
        {
            GameController.Instance.WinGame();
        }
        else
        {
            GameController.Instance.FailGame();
        }
    }
}

