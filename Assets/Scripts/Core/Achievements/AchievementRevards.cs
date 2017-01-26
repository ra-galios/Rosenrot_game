using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public struct Achievement
{
    public AchievementsController.Type m_Achievement;
    public string m_Title;
    public string m_Description;
    public Sprite m_LockedSprite;   //только для списка в меню
    public Sprite[] m_LeveledSprites;
    public int[] m_NeedToAchieve;
    public AchievementsController.RewardType m_RevardType;   //тип награды
    public int[] m_LeveledRevards;
}

public class AchievementRevards : MonoBehaviour
{
    [SerializeField]
    private Sprite[] m_RewardSprites;

    [SerializeField]
    private Achievement[] m_Achievements;

    public Achievement[] Achievements
    {
        get { return m_Achievements; }
    }

    public Sprite[] RewardSprites
    {
        get { return m_RewardSprites; }
    }
}
