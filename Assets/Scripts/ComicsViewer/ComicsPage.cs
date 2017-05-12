using UnityEngine;

[RequireComponent(typeof(PageTransition))]
public class ComicsPage : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private float PageTimer = 3f;

    [SerializeField]
    private float NextPageTimer = 3f;

    private PageTransition transition;

    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    void Start ()
    {
        GetComponent<SpriteRenderer>().sortingOrder = id;
        transition = GetComponent<PageTransition>();
        ComicsController.Instance.Pages.Add(this);
        gameObject.SetActive(false);
    }

    public void Show()
    {
        transition.ChangeAcrossAlfa(1f, true);
        Invoke("Wait", PageTimer);
    }

    public void Wait ()
    {
        ShowNextPage();
        Stop();        
	}

    public void Stop()
    {
        if (transition.SpriteTransition)
        {
            transition.ChangeAcrossImage(PageTimer);
            return;
        }
        
        transition.ChangeAcrossAlfa(1f, false);
    }

    private void ShowNextPage()
    {
        ComicsController.Instance.ShowNextPage();
    }
}
