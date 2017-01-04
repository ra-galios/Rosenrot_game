using UnityEngine;
using System.Collections;

public class Bomb : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Bomb++;
    }

    public void Action()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Use", true);
        Destroy(this.gameObject, 1f);
    } //действие колетблза
}
