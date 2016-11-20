using UnityEngine;
using System.Collections;

public class CollectableGO : MonoBehaviour {
    public enum Type { dimond, ruby, health,  powder, seed}
    public Type typeBonus;

    public void EnterBonus()
    {
        switch (typeBonus)
        {
            case Type.dimond:
                Market.Instance.Dimond++;
                break;
            case Type.ruby:
                Market.Instance.Ruby++;
                break;
            case Type.powder:
                Market.Instance.Powder++;
                break;
            case Type.health:
                Market.Instance.Health++;
                break;
            case Type.seed:
                Market.Instance.Seeds++;
                break;
            default:
                break;
        }

        Destroy(this.gameObject);
    }

    class ActionCollectedObject
    {
        
    }
}