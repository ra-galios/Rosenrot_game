using UnityEngine;
using System.Collections;

public class Dimond : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Dimond++;
        Destroy(this.gameObject);
    }
}
