using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public struct Achievement
{
    public AchievementsController.Type m_Achievement;
    public int m_Revard;
    public int[] m_NeedToAchieve;
    public int[] m_LeveledRevards;
    public MarketItem RevardItem;
    public int RevardCount;

}

public class AchievementRevards : MonoBehaviour
{
    [SerializeField]
    private Achievement[] m_Achievements;

    public Achievement[] Achievements
    {
        get { return m_Achievements; }
    }
}
