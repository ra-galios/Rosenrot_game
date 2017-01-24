using UnityEngine.UI;
using UnityEngine;

public class PanelVictory : MonoBehaviour
{

    [SerializeField]
    private Button returnToMainButton;
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private Button restartButton;

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


    void Start()
    {
        GameController.Instance.VictoryPanelAnim = GetComponent<Animator>();
    }

    public void Update()
    {
        restartButton.interactable = true;
        nextLevelButton.interactable = true;

        if (Market.Instance.Health < 1)
        {
            restartButton.interactable = false;
            nextLevelButton.interactable = false;
        }


        if(GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].diamondsCollected != GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].isCollected.Length)
        {
            nextLevelButton.interactable = false;
        }


        health.text = Market.Instance.Health.ToString();

        rubiesCollected.text = "+" + GameController.Instance.RubiesCollectedOnLevel.ToString();
        diamondsCollected.text = "+" + GameController.Instance.DiamondsCollectedOnLevel.ToString();
        bombsCollected.text = "+" + GameController.Instance.BombsCollectedOnLevel.ToString();
        seedsCollected.text = "+" + GameController.Instance.SeedsCollectedOnLevel.ToString();
    }

    void OnEnable()
    {
        restartButton.onClick.AddListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.AddListener(GameController.Instance.ResumeGame);

        nextLevelButton.onClick.AddListener(GameController.Instance.LoadNextLevel);

        returnToMainButton.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        restartButton.onClick.RemoveListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.RemoveListener(GameController.Instance.ResumeGame);

        nextLevelButton.onClick.RemoveListener(GameController.Instance.LoadNextLevel);

        returnToMainButton.onClick.RemoveListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
    }
}
