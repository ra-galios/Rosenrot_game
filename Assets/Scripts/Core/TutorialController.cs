using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Button;

    [SerializeField]
    private Sprite m_BombSprite;
    [SerializeField]
    private Sprite m_ChooseSprite;
    [SerializeField]
    private Sprite m_FallSprite;
    [SerializeField]
    private Sprite m_GuessSprite;
    [SerializeField]
    private Sprite m_OnlyUpSprite;
    [SerializeField]
    private Sprite m_SwipeSprite;
    [SerializeField]
    private Sprite m_ToTheTopSprite;
    [SerializeField]
    private Sprite m_OneByOneSprite;

    private Image m_TutorialImage;
    private Animator m_TutorialAnimator;
    private static PlayerBehaviour m_PlayerBeh;
    private bool[] m_CanShow = new bool[] { true, true, true, true, true, true };

    private string m_SceneName;

    // Use this for initialization
    void Start()
    {
        m_TutorialImage = GetComponent<Image>();
        m_TutorialAnimator = GetComponent<Animator>();
        m_SceneName = SceneManager.GetActiveScene().name.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerBeh)
        {
            if (DataManager.Instance.GetTutorialDisplays(0) < 2 && m_CanShow[0] && m_PlayerBeh.IdLine == 0 && m_SceneName == "GameScene0")
            {
                m_CanShow[0] = false;
                DataManager.Instance.SetTutorialDisplays(0);
                StartCoroutine(ShowButton(new Sprite[] { m_OnlyUpSprite, m_ToTheTopSprite }));
            }
            else if (DataManager.Instance.GetTutorialDisplays(1) < 2 && m_CanShow[1] && m_PlayerBeh.IdLine == 1 && m_SceneName == "GameScene1")
            {
                m_CanShow[1] = false;
                DataManager.Instance.SetTutorialDisplays(1);
                StartCoroutine(ShowButton(new Sprite[] { m_SwipeSprite }));
            }
            else if (DataManager.Instance.GetTutorialDisplays(2) < 2 && m_CanShow[2] && m_PlayerBeh.IdLine == 0 && m_SceneName == "GameScene2")
            {
                m_CanShow[2] = false;
                DataManager.Instance.SetTutorialDisplays(2);
                StartCoroutine(ShowButton(new Sprite[] { m_ChooseSprite }));
            }
            else if (DataManager.Instance.GetTutorialDisplays(3) < 2 && m_CanShow[3] && m_PlayerBeh.IdLine == 11 && m_SceneName == "GameScene2")
            {
                m_CanShow[3] = false;
                DataManager.Instance.SetTutorialDisplays(3);
                StartCoroutine(ShowButton(new Sprite[] { m_GuessSprite }));
            }
            else if (DataManager.Instance.GetTutorialDisplays(5) < 2 && m_CanShow[5] && m_PlayerBeh.IdLine > 21 && m_SceneName == "GameScene1" 
                && GameController.Instance.m_DiesInARow > 2)
            {
                m_CanShow[5] = false;
                DataManager.Instance.SetTutorialDisplays(5);
                StartCoroutine(ShowButton(new Sprite[] { m_OneByOneSprite }));
            }

            if (DataManager.Instance.GetTutorialDisplays(4) < 2 && m_CanShow[4] && m_PlayerBeh.IsPlayerFall)
            {
                m_CanShow[4] = false;
                DataManager.Instance.SetTutorialDisplays(4);
                StartCoroutine(ShowButton(new Sprite[] { m_FallSprite }));
            }

            if (!m_CanShow[4] && !m_PlayerBeh.IsPlayerFall)
                m_CanShow[4] = true;
        }
    }

    private IEnumerator ShowButton(Sprite[] buttonSprite)
    {
        GameController.Instance.PauseGame();
        int i = 0;
        while (i < buttonSprite.Length)
        {
            m_TutorialImage.sprite = buttonSprite[i];
            m_TutorialAnimator.SetTrigger("tutorial");

            yield return new WaitUntil(() => EventSystem.current.currentSelectedGameObject == m_Button);
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForSecondsRealtime(0.4f);
            i++;
        }

        EventSystem.current.SetSelectedGameObject(null);
        GameController.Instance.ResumeGame();
    }

    public static PlayerBehaviour PlayerBeh
    {
        get { return m_PlayerBeh; }
        set { m_PlayerBeh = value; }
    }
}
