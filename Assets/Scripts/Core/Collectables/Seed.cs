using UnityEngine;
using System.Collections;

public class Seed : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Seeds++;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);    }
}
