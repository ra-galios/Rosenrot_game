using UnityEngine;
using System.Collections;

public class Ruby : CollectableGO
{

    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Ruby++;
        AchievementsController.AddToAchievement(AchievementsController.Type.RubyRubyRubyRuby, 1);
        GameController.Instance.RubiesCollectedOnLevel++;
    }
}
