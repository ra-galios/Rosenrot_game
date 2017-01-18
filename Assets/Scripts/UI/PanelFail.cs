using UnityEngine.UI;
using UnityEngine;

public class PanelFail : MonoBehaviour
{

    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button returnToMainButton;
    [SerializeField]
    private Animator deadJacobAnim;

    [SerializeField]
    private Text health;

    void Start()
    {
        GameController.Instance.FailPanelAnim = GetComponent<Animator>();
        GameController.Instance.FailDeadJacobAnim = deadJacobAnim;
    }

    void Update()
    {
        if (Market.Instance.Health < 1)
        {
            restartButton.interactable = false;
        }
        else
        {
            restartButton.interactable = true;
        }

        health.text = Market.Instance.Health.ToString();
    }

    void OnEnable()
    {
        restartButton.onClick.AddListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.AddListener(GameController.Instance.ResumeGame);

        returnToMainButton.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        restartButton.onClick.RemoveListener(GameController.Instance.LoadActiveScene);
        restartButton.onClick.RemoveListener(GameController.Instance.ResumeGame);

        returnToMainButton.onClick.RemoveListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
    }
}
