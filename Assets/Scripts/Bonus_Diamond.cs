using UnityEngine;
using System.Collections;

public class Bonus_Diamond : Bonus {
    public override void GetBonus()
    {
        GameController.DiamondsCount++;
        base.GetBonus();
    }

    public override void SaveBonus()
    {
        GameController.SaveBonus(GameController.DiamondKey, GameController.DiamondsCount);
        Destroy(gameObject);
    }
}
