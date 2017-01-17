using UnityEngine;
using System.Collections;

public class Health : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Health++;
    }
}
