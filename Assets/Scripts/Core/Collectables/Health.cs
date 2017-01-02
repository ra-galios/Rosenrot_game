using UnityEngine;
using System.Collections;

public class Health : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Health++;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);    }
}
