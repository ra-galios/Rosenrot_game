using UnityEngine;
using System.Collections;

public class Seed : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Seeds++;
        Destroy(this.gameObject);
    }
}
