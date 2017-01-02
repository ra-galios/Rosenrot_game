using UnityEngine;
using System.Collections;

public class Dimond : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Dimond++;
        Market.Instance.LocalDiamond++;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);
    }
}
