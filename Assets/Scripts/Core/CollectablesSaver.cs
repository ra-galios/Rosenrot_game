using UnityEngine;
using System.Collections;

public class CollectablesSaver : CreateSingletonGameObject<CollectablesSaver>
{

    public void LoadCollectables()
    {
        if (PlayerPrefs.HasKey("Seeds"))
            Market.Instance.Seeds = PlayerPrefs.GetInt("Seeds");
        else
            PlayerPrefs.SetInt("Seeds", Market.Instance.Seeds);

        if (PlayerPrefs.HasKey("Bombs"))
            Market.Instance.Bomb = PlayerPrefs.GetInt("Bombs");
        else
            PlayerPrefs.SetInt("Bombs", Market.Instance.Bomb);

        if (PlayerPrefs.HasKey("Diamonds"))
            Market.Instance.Dimond = PlayerPrefs.GetInt("Diamonds");
        else
            PlayerPrefs.SetInt("Diamonds", Market.Instance.Dimond);

        if (PlayerPrefs.HasKey("Rubies"))
            Market.Instance.Ruby = PlayerPrefs.GetInt("Rubies");
        else
            PlayerPrefs.SetInt("Rubies", Market.Instance.Ruby);

    }

    public void SaveCollectables()
    {
        PlayerPrefs.SetInt("Seeds", Market.Instance.Seeds);
        PlayerPrefs.SetInt("Bombs", Market.Instance.Bomb);
        PlayerPrefs.SetInt("Diamonds", Market.Instance.Dimond);
        PlayerPrefs.SetInt("Rubies", Market.Instance.Ruby);
    }
}
