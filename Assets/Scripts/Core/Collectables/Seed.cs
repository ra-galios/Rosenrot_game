using UnityEngine;
using System.Collections;

public class Seed : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Seeds++;
        AchievementsController.AddToAchievement(AchievementsController.Type.JacobAndTheBeanstalk, 1);
        GameController.Instance.SeedsCollectedOnLevel++;
    }
}
