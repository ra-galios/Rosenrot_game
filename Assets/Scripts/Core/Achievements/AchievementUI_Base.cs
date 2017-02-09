using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI_Base : MonoBehaviour
{

    // protected void Start()
    // {
    //     //SetFields();
    // }

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
        int level = GetAchievementLevel(indexInResource);

        if (level >= 0)
        {
            return GameController.Instance.AchievementRevards.Achievements[indexInResource].m_LeveledSprites[level];
        }
        else
        {
            return GameController.Instance.AchievementRevards.Achievements[indexInResource].m_LockedSprite;
        }
    }

    protected virtual int GetAchievementLevel(int indexInResource)
    {
        int[] needToAchieve = GameController.Instance.AchievementRevards.Achievements[indexInResource].m_NeedToAchieve;
        int currentValue = AchievementsController.GetAchievement(GetType(indexInResource));

        int level = -1;  //locked

        for (int i = (needToAchieve.Length - 1); i >= 0; i--)
        {
            if (currentValue >= needToAchieve[i])
            {
                return i;
            }
        }

        return level;
    }

    protected virtual void SetFields(int indexInResource)
    {

    }

    public virtual void Show()
    {

    }
}
