using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicsController : MonoBehaviour
{
    public static ComicsController Instance;
    public string LoadScene;

    [HideInInspector]
    public List<ComicsPage> Pages = new List<ComicsPage>();

    private int currentPageId;
    private int countPages;

    private void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        ShowPages();
	}
	
    public void ShowPages()
    {
        StartCoroutine(ShowPageCoroutine());
    }

    public void ShowNextPage()
    {       
        if (countPages > Pages.Count)
        {
            
            return;
        }

        foreach (ComicsPage page in Pages)
        {
            if (page.ID == currentPageId)
            {
                page.gameObject.SetActive(true);
                page.Show();
                countPages++;
                currentPageId++;
                return;
            }
        }

        SceneManager.LoadScene(LoadScene);
        
    }

    IEnumerator ShowPageCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        ShowNextPage();
    }
}
