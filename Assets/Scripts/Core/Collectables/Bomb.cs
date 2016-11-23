using UnityEngine;
using System.Collections;

public class Bomb : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Bomb++;
        Destroy(this.gameObject);
    }
}
