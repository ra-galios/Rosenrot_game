using UnityEngine;
using System.Collections;

public class Ruby : CollectableGO {

    public override void EnterBonus()
    {
        Market.Instance.Ruby++;
        Destroy(this.gameObject);
    }
}
