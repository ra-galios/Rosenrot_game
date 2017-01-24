using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI_Base : MonoBehaviour
{

    protected AchievementsController.Type m_Achievement;
    protected Image m_Image;
    protected Text m_NeedToNextLeveledAch;
    protected Text m_Revards;
    protected Text m_Title;
    protected Text m_Description;


    protected void Start()
    {
        //SetFields();
    }

#if UNITY_EDITOR
    protected void Update()
    {
        //SetFields();
    }

#endif

    protected virtual AchievementsController.Type GetType(int indexInResource)
    {
        return GameController.Instance.AchievementRevards.Achievements[indexInResource].m_Achievement;
    }

    protected virtual Sprite GetSprite(int indexInResource)
    {
        int[] needToAchieve = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_NeedToAchieve;
        int currentValue = AchievementsController.GetAchievement(GetType(indexInResource));

        for (int i = (needToAchieve.Length - 1); i >= 0; i--)
        {
            if (currentValue > needToAchieve[i])
            {
                return GameController.Instance.AchievementRevards.Achievements[indexInResource].m_LeveledSprites[i];
            }
        }

        return GameController.Instance.AchievementRevards.Achievements[indexInResource].m_LockedSprite;
    }

    protected virtual void SetFields(int indexInResource)
    {
        //m_Achievement = GetType(indexInResource);

        m_Image.sprite = GetSprite(indexInResource);

        m_Title.text = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_Title;
        m_Description.text = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_Description;
    }

	protected virtual void GetReward(AchievementsController.RewardType rewardType)
	{

	}

	public virtual void Show()
	{
		
	}
}
