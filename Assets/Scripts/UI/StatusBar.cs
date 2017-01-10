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

    void OnEnable()
    {
        Market.Instance.ChangeStatusAction += ChangeStatus;
    }

    void OnDisable()
    {
        Market.Instance.ChangeStatusAction -= ChangeStatus;
    }

    // Use this for initialization
    void ChangeStatus()
    {
        timeStatus.text = Market.Instance.Health.ToString() + ":" + Market.Instance.Health.ToString();
        lifesCounter.text = Market.Instance.Health.ToString();
        seedsStatus.text = Market.Instance.Seeds.ToString();
        bombsStatus.text = Market.Instance.Bomb.ToString();
        diamondsStatus.text = Market.Instance.Dimond.ToString();
        rubiesStatus.text = Market.Instance.Ruby.ToString();
    }
}
