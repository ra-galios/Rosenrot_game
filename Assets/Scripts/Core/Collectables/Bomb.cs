using UnityEngine;
using System.Collections;

public class Bomb : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Bomb++;
    }
}
