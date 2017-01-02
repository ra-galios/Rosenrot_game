using UnityEngine;
using System.Collections;

public class Bomb : CollectableGO
{
    public override void EnterBonus()
    {
        Market.Instance.Bomb++;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);
    }
}
