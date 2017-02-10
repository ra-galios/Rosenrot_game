
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchievementUI_Leveled : AchievementUI_Base
{
    //protected AchievementsController.Type m_Achievement;
    [SerializeField]
    protected Image m_Image;
    [SerializeField]
    protected Text m_Title;
    [SerializeField]
    protected Text m_Description;
    [SerializeField]
    protected Text m_Reward;
    [SerializeField]
    protected Image m_RewardImage;

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

    protected override void SetFields(int indexInResource)
    {
        base.SetFields(indexInResource);

        Achievement[] achievements = GameController.Instance.AchievementRevards.Achievements;

        m_Image.sprite = GetSprite(indexInResource);

        m_Title.text = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_Title;
        m_Description.text = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_Description;


        m_Reward.text = "+" + achievements[indexInResource].m_LeveledRevards[GetAchievementLevel(indexInResource)].ToString();
        m_RewardImage.sprite = GameController.Instance.AchievementRevards.RewardSprites[(int)achievements[indexInResource].m_RevardType];

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
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForSecondsRealtime(m_TimeBetweenShows);
        }

        if (isWin)
        {
            GameController.Instance.WinGame();
        }
        else
        {
            GameController.Instance.FailGame();
        }
        GameController.Instance.AchievementsToShow.Clear();
    }
}

