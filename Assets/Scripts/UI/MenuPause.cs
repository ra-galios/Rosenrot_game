using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MenuPause : MonoBehaviour
{

    [SerializeField]
    private Button pauseGame;
    [SerializeField]
    private Button resumeGame;
    [SerializeField]
    private Button returnToMain;

    // Use this for initialization
    void OnEnable()
    {
        pauseGame.onClick.AddListener(GameController.Instance.PauseGame);
        resumeGame.onClick.AddListener(GameController.Instance.ResumeGame);
        returnToMain.onClick.AddListener(GameController.Instance.LoadMainScene);
        returnToMain.onClick.AddListener(GameController.Instance.ResumeGame);
    }

    void OnDisable()
    {
        pauseGame.onClick.RemoveListener(GameController.Instance.PauseGame);
        resumeGame.onClick.RemoveListener(GameController.Instance.ResumeGame);
        returnToMain.onClick.RemoveListener(GameController.Instance.LoadMainScene);
    }

	public void Some()
	{
		print("click");
	}
}
