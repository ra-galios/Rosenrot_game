﻿using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    [SerializeField]
    private Text seedsStatus;
    [SerializeField]
    private Text bombsStatus;
    [SerializeField]
    private Text diamondsStatus;
    [SerializeField]
    private Text rubiesStatus;

    // Use this for initialization
    void Start()
    {
        seedsStatus.text = Market.Instance.Seeds.ToString();
        bombsStatus.text = Market.Instance.Bomb.ToString();
        diamondsStatus.text = Market.Instance.Dimond.ToString();
        rubiesStatus.text = Market.Instance.Ruby.ToString();
    }
}