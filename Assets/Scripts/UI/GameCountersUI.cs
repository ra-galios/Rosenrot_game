using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCountersUI : MonoBehaviour
{

    [SerializeField]
    private GameObject m_SeedsCounter;
    [SerializeField]
    private GameObject m_BombsCounter;

    private Text m_SeedsCounterText;
    private Text m_BombsCounterText;



    // Use this for initialization
    void Start()
    {
        m_SeedsCounterText = m_SeedsCounter.GetComponentInChildren<Text>();
        m_BombsCounterText = m_BombsCounter.GetComponentInChildren<Text>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        print(currentSceneIndex);
        if (currentSceneIndex > 6)
            m_BombsCounter.SetActive(true);
    }

    private void OnEnable()
    {
        Victory.m_OnStartBonusLevelAction += EnableSeedsCounter;
    }

    private void OnDisable()
    {
        Victory.m_OnStartBonusLevelAction -= EnableSeedsCounter;
    }

    // Update is called once per frame
    void Update()
    {
        m_SeedsCounterText.text = Market.Instance.Seeds.ToString();
        m_BombsCounterText.text = Market.Instance.Bomb.ToString();
    }

    private void EnableSeedsCounter()
    {
        m_SeedsCounter.SetActive(true);
    }
}
