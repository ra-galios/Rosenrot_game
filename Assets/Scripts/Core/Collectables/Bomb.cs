using UnityEngine;
using System.Collections;

public class Bomb : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        GameController.Instance.BombsCollectedOnLevel++;
        Market.Instance.Bomb++;
    }

    public void Action()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Use", true);
        Market.Instance.Bomb -= 1;
        Destroy(this.gameObject, 1f);
    } //действие колетблза
}
