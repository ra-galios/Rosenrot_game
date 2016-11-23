using UnityEngine;
using System.Collections;

public class Health : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Health++;
        Destroy(this.gameObject);
    }
}
