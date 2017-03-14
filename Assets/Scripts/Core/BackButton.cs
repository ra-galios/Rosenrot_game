using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour
{

    public enum MenuLocation { Map, Market, Achievements, MainMenu, Info }

    private MenuLocation currentLocation = MenuLocation.MainMenu;

    [HeaderAttribute("From Map To Menu Button")]
    [SerializeField]
    private Button m_MapButton;
    [HeaderAttribute("From Market To Menu Button")]
    [SerializeField]
    private Button m_MarketButton;
    [HeaderAttribute("From Achievements To Menu Button")]
    [SerializeField]
    private Button m_AchievementsButton;
    [HeaderAttribute("From Info To Menu Button")]
    [SerializeField]
    private Button m_InfoButton;


    //private int nesting = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            GameController.Instance.LoadMainScene();
            GameController.Instance.ResumeGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "menu")
        {
            switch (currentLocation)
            {
                case MenuLocation.MainMenu:
                    DataManager.Instance.SaveGameData();
                    Application.Quit();
                    break;
                case MenuLocation.Map:
                    StartCoroutine(BackToMainMenu(m_MapButton));
                    currentLocation = MenuLocation.MainMenu;
                    break;
                case MenuLocation.Market:
                    StartCoroutine(BackToMainMenu(m_MarketButton));
                    currentLocation = MenuLocation.MainMenu;
                    break;
                case MenuLocation.Achievements:
                    StartCoroutine(BackToMainMenu(m_AchievementsButton));
                    currentLocation = MenuLocation.MainMenu;
                    break;
                case MenuLocation.Info:
                    StartCoroutine(BackToMainMenu(m_InfoButton));
                    currentLocation = MenuLocation.MainMenu;
                    break;
            }

        }
    }

    private IEnumerator BackToMainMenu(Button button)
    {
        yield return new WaitForSeconds(0.1f);
        button.onClick.Invoke();
    }

    public void SetMenuStanding(int location)
    {
        currentLocation = (MenuLocation)location;
    }
}
