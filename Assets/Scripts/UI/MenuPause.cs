using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MenuPause : MonoBehaviour
{

    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button returnToMainButton;

    // Use this for initialization
    void OnEnable()
    {
        pauseButton.onClick.AddListener(GameController.Instance.PauseGame);
        resumeButton.onClick.AddListener(GameController.Instance.ResumeGame);
        returnToMainButton.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        pauseButton.onClick.RemoveListener(GameController.Instance.PauseGame);
        resumeButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.LoadMainScene);
        returnToMainButton.onClick.RemoveListener(GameController.Instance.ResumeGame);
    }
}
