using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class AchievementElement : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{ 
    private ScrollRect m_ScrollRect;
    private Button m_Button;
    private Animator m_Animator;

    private float timer = 0f;
    private bool m_PointerDown = false;
    private bool m_PointerUp = false;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_ScrollRect = GetComponentInParent<ScrollRect>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_PointerDown)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                m_Animator.SetBool("Highlighted", true);
                m_PointerDown = false;
            }
            else if (Mathf.Abs(m_ScrollRect.velocity.y) > 0.01f)
            {
                m_PointerDown = false;
            }
        }
        else if (m_PointerUp)
        {
            m_Animator.SetBool("Highlighted", false);
            m_PointerUp = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        m_PointerDown = true;
        timer = 0f;
    }

    public void OnPointerExit(PointerEventData data)
    {
        print("up");
        m_PointerUp = true;
    }
}
