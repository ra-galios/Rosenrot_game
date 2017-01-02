using UnityEngine;
using System.Collections;

public class Ruby : CollectableGO {

    public override void EnterBonus()
    {
        Market.Instance.Ruby++;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);    }
}
