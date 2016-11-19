using UnityEngine;
using System.Collections;

public class CollectableGO : MonoBehaviour {
    public enum Type { star, health,  powder}
    public Type typeBonus;

    public void EnterBonus()
    {
        switch (typeBonus)
        {
            case Type.star:
                Market.Instance.Start++;
                break;
            case Type.powder:
                Market.Instance.Powder++;
                break;
            case Type.health:
                Market.Instance.Health++;
                break;
        }

        Destroy(this.gameObject);
    }

    class ActionCollectedObject
    {
        
    }
}