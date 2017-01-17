using UnityEngine;
using System.Collections;

public class Ruby : CollectableGO
{

    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Ruby++;
        GameController.Instance.RubiesCollectedOnLevel++;
    }
}
