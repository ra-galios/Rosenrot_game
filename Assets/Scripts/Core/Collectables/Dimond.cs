using UnityEngine;
using System.Collections;

public class Dimond : CollectableGO
{
    public override void EnterBonus()
    {
        base.EnterBonus();
        Market.Instance.Dimond++;
        
        GameController.Instance.levelsData[GameController.Instance.CurrentLevel].diamondsCollected++;

        LevelDiamondKeeper.Instance.SetCollected(this);
    }
}
