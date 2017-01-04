using UnityEngine;
using System.Collections;

public class Seed : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Seeds++;
    }
}
