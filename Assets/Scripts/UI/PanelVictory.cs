using UnityEngine.UI;
using UnityEngine;

public class PanelVictory : AchievementUI_Base
{

    [SerializeField]
    private Button returnToMainButton;
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Text m_LevelNumber;
    [SerializeField]
    private Text health;
    [SerializeField]
    private Text rubiesCollected;
    [SerializeField]
    private Text diamondsCollected;
    [SerializeField]
    private Text bombsCollected;
    [SerializeField]
    private Text seedsCollected;

    [SerializeField]
    private Image[] m_AchievementImages;
    [SerializeField]
    private GameObject m_AchievementsPanel;



    void Start()
    {
        GameController.Instance.VictoryPanelAnim = GetComponent<Animator>();
    }

    public void UpdateVictoryPanel()
    {
        restartButton.interactable = true;
        nextLevelButton.interactable = true;

        if (Market.Instance.Health < 1)
        {
            restartButton.interactable = false;
            nextLevelButton.interactable = false;
        }


        if (GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].diamondsCollected != GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].IsCollected.Length)
        {
            nextLevelButton.interactable = false;
        }


        health.text = Market.Instance.Health.ToString();
        m_LevelNumber.text = (GameController.Instance.CurrentLevel + 1).ToString();

        rubiesCollected.text = "+" + GameController.Instance.RubiesCollectedOnLevel.ToString();
        diamondsCollected.text = "+" + GameController.Instance.DiamondsCollectedOnLevel.ToString();
        bombsCollected.text = "+" + GameController.Instance.BombsCollectedOnLevel.ToString();
        seedsCollected.text = "+" + GameController.Instance.SeedsCollectedOnLevel.ToString();

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
        GameController.m_VictoryAction += UpdateVictoryPanel;

        restartButton.onClick.AddListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.AddListener(GameController.Instance.ResumeGame);

        nextLevelButton.onClick.AddListener(GameController.Instance.LoadNextLevel);

        returnToMainButton.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        GameController.m_VictoryAction -= UpdateVictoryPanel;

        restartButton.onClick.RemoveListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.RemoveListener(GameController.Instance.ResumeGame);

        nextLevelButton.onClick.RemoveListener(GameController.Instance.LoadNextLevel);

        returnToMainButton.onClick.RemoveListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
    }
}
