using UnityEngine;
using System.Collections;

public class CollectableGO : MonoBehaviour {
    public void EnterBonus()
    {
        Market.Instance.Health++;
        Destroy(this.gameObject);
    }

    class ActionCollectedObject
    {
        
    }
}