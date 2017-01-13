using UnityEngine;
using System.Collections;
using System;

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private int m_Line; //линия в которой находится пуш
    [SerializeField]
    private int m_Collumn; //колонка в которой находится пуш
    // [SerializeField]
    // private CollectableGO m_PrefBonus; //префаб колетблза на пушере
    [SerializeField]
    private GameInput.PlayerAction m_Action;
    [SerializeField]
    private bool isSeed = false;


    private float speed; //скорость 
    // private GameObject bonus; //колектблз на пушере
    // private bool isCreateBonus=false;
    private Animator anim;
    private bool CreatePusher = false;

    void Start()
    {
        if (isSeed)
        {
            anim = GetComponent<Animator>();
        }
    }

    void OnBecameVisible()
    {
        if (isSeed)
        {

            if (!CreatePusher)
            {
                anim.SetBool("CreatePusher", true);
                Market.Instance.Seeds--;
                CreatePusher = true;
            }
        }
    }

    void Update()
    {


        if (LevelGenerator.Instance.IsRunLevel)
        {
             MovePusher();

        //     if(isSeed)
        //     {
        //         if(!CreatePusher && transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize)
        //         {
        //             anim.SetBool("CreatePusher", true);
        //             Market.Instance.Seeds--;
        //             CreatePusher = true;
        //         }
        //     }
        }
        // if (gameObject.activeSelf && m_PrefBonus && !isCreateBonus)//если пушер активировался, у него есть бонус и он ещё не инициализирован
        // {
        //     Vector3 bonusPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //     bonusPos.y += 0.6f;
        //     Bonus = Instantiate(PrefBonus.gameObject, bonusPos, transform.rotation) as GameObject;
        //     Bonus.transform.parent = transform;
        //     isCreateBonus = true;
        // }
    }

    void MovePusher()
    {
        speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }


    //свойства
    public int Line
    {
        get { return this.m_Line; }
        set { this.m_Line = value; }
    }
    public int Collumn
    {
        get { return this.m_Collumn; }
        set { this.m_Collumn = value; }
    }
    public float Speed
    {
        get { return this.speed; }
        set { this.speed = value; }
    }
    // public GameObject Bonus
    // {
    //     get { return this.bonus; }
    //     set { this.bonus = value; }
    // }
    // public CollectableGO PrefBonus
    // {
    //     get { return this.m_PrefBonus; }
    //     set { this.m_PrefBonus = value; }
    // }
    public bool IsSeed
    {
        get { return this.isSeed; }
    }
    public GameInput.PlayerAction Action
    {
        get { return this.m_Action; }
        set { this.m_Action = value; }
    }
}