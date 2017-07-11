using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTransition : MonoBehaviour
{
    public Sprite SpriteTransition;
    [SerializeField]
    private float m_TransitionSpeed = 0.01f;

    private SpriteRenderer spriteRend;
    private Color color;

    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        color = spriteRend.color;
        color.a = 0;
        spriteRend.color = color;
    }

    public void ChangeAcrossAlfa(float timeStep, bool isShow)
    {
        StartCoroutine(ChangeAlfaCoroutine(timeStep, isShow));
    }

    public void ChangeAcrossImage(float timePageShown)
    {
        GameObject imageObj = new GameObject("Image");
        imageObj.AddComponent<SpriteRenderer>().sprite = SpriteTransition;
        Destroy(imageObj, timePageShown);

        StartCoroutine(ChangeAlfaCoroutine(1f, false));
    }

    public void ChangeAcrossAnimation()
    {
        StartCoroutine(ChangeAlfaCoroutine(1f, false));
    }

    IEnumerator ChangeAlfaCoroutine(float timeStep, bool isShow)
    {
        if (!isShow)
        {
            while (color.a > 0)
            {
                color.a -= timeStep * m_TransitionSpeed;
                spriteRend.color = color;
                yield return null;
            }

            //ComicsController.Instance.ShowNextPage();
            Destroy(gameObject);
        }
        else
        {
            while (color.a <= 1f)
            {
                color.a += timeStep * m_TransitionSpeed;
                spriteRend.color = color;
                yield return null;
            }
        }
    }
}
