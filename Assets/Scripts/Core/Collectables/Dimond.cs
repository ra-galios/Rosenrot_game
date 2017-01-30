using UnityEngine;
using System.Collections;

public class Dimond : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Dimond++;
        AchievementsController.AddToAchievement(AchievementsController.Type.EverybodysBestFriend, 1);
        GameController.Instance.DiamondsCollectedOnLevel++;

        GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].diamondsCollected++;

        if (LevelDiamondKeeper.Instance != null)
            LevelDiamondKeeper.Instance.SetCollected(this);
    }
}
