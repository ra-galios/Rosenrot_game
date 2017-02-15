using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    [SerializeField]
    private Text lifesCounter;
    [SerializeField]
    private Text seedsStatus;
    [SerializeField]
    private Text bombsStatus;
    [SerializeField]
    private Text diamondsStatus;
    [SerializeField]
    private Text rubiesStatus;
    [SerializeField]
    private Text timeStatus;

    void Update()
    {
        ChangeStatus();
    }

    // Use this for initialization
    void ChangeStatus()
    {
        string secondsUntilHealth = Market.Instance.SecondsUntilHealth.ToString();
                print(Market.Instance.MinutesUntilHealth.ToString());

        if(secondsUntilHealth.Length < 2)
        {
            secondsUntilHealth = "0" + secondsUntilHealth;
        }
        timeStatus.text = Market.Instance.MinutesUntilHealth.ToString() + ":" + secondsUntilHealth;
        lifesCounter.text = Market.Instance.Health.ToString();
        seedsStatus.text = Market.Instance.Seeds.ToString();
        bombsStatus.text = Market.Instance.Bomb.ToString();
        diamondsStatus.text = Market.Instance.Dimond.ToString();
        rubiesStatus.text = Market.Instance.Ruby.ToString();
    }
}
