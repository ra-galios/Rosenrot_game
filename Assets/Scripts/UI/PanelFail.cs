using UnityEngine.UI;
using UnityEngine;

public class PanelFail : AchievementUI_Base
{

    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button returnToMainButton;
    [SerializeField]
    private Animator deadJacobAnim;

    [SerializeField]
    private Text health;

    [SerializeField]
    private Image[] m_AchievementImages;
    [SerializeField]
    private GameObject m_AchievementsPanel;

    [SerializeField]
    private Animator AdsFailAnimator;

    void Start()
    {
        GameController.Instance.FailPanelAnim = GetComponent<Animator>();
        GameController.Instance.FailDeadJacobAnim = deadJacobAnim;
    }

    public void RestartAfterFall()
    {
        PlayerBehaviour player = GameObject.FindObjectOfType<PlayerBehaviour>();
        player.enabled = true;
        GameController.Instance.ResumeGame();
        player.GrabAfterFall();
        GetComponent<Animator>().SetTrigger("Fail");
    }

    public void UpdateFailPanel()
    {
        //вызов панельки просмотра рекламы - здесь нужно сделать проверку на первую смерть на уровне
        //AdsFailAnimator.SetTrigger("Fail");

        if (Market.Instance.Health < 1)
        {
            restartButton.interactable = false;
        }
        else
        {
            restartButton.interactable = true;
        }

        health.text = Market.Instance.Health.ToString();

        if (GameController.Instance.AchievementsToShow.Count < 1)
        {
            m_AchievementsPanel.SetActive(false);
        }
        else
        {
            for (int i = 0; i < m_AchievementImages.Length; i++)
            {
                if (GameController.Instance.AchievementsToShow.Count <= i)
                {
                    m_AchievementImages[i].gameObject.SetActive(false);
                }
                else
                {
                    m_AchievementImages[i].sprite = GetSprite(GameController.Instance.AchievementsToShow[i]);
                }
            }
        }
    }

    void OnEnable()
    {
        GameController.m_FailAction += UpdateFailPanel;

        restartButton.onClick.AddListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.AddListener(GameController.Instance.ResumeGame);

        returnToMainButton.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        GameController.m_FailAction -= UpdateFailPanel;

        restartButton.onClick.RemoveListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.RemoveListener(GameController.Instance.ResumeGame);

        returnToMainButton.onClick.RemoveListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
    }
}
