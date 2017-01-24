using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDiamondKeeper : MonoBehaviour
{

    private static LevelDiamondKeeper instance;
    [SerializeField, HeaderAttribute("алмазы на сцене")]
    private Dimond[] diamonds;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
		if(diamonds.Length != GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].isCollected.Length)
		{
			GameController.Instance.LevelsData[GameController.Instance.CurrentLevel] = new LevelData(diamonds.Length);
		}

        for (int i = 0; i < diamonds.Length; i++)
        {
            if (GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].isCollected[i])
            {
                diamonds[i].gameObject.SetActive(false);
            }
        }

    }

    public void SetCollected(Dimond colDiamond)
    {
        for (int i = 0; i < diamonds.Length; i++)
        {
            if (colDiamond == diamonds[i])
            {
                GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].isCollected[i] = true;
            }
        }
    }

    public static LevelDiamondKeeper Instance
    {
        get { return instance; }
    }

    public Dimond[] Diamonds
    {
        get { return diamonds; }
    }
}
