using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI_Base : MonoBehaviour 
{

	[SerializeField]
	protected AchievementsController.Type m_Achievement;

	[SerializeField]
	protected Image m_Image;

	[SerializeField]
	protected Sprite m_LockSprite;

	[SerializeField]
	protected Sprite m_UnlockSprite;

	[HeaderAttribute("необязательные поля для редактора")]
	[SerializeField]
	protected Text m_NameField;


	protected void Start()
	{
		SetFields();

		Sprite sprite = m_LockSprite;
		int curValue = AchievementsController.GetAchievement(m_Achievement);
		if(AchievementsController.CheckAchievementComplete(m_Achievement)
		|| AchievementsController.CheckAchievement(m_Achievement, curValue))
		{
			m_Image.sprite = m_UnlockSprite;
		}
	}

#if UNITY_EDITOR
	protected void Update()
	{
		SetFields();
	}

#endif

	protected virtual void SetFields()
	{		
#if UNITY_EDITOR
		if(m_NameField) 
			m_NameField.text = m_Achievement.ToString();
#endif
	}
}
