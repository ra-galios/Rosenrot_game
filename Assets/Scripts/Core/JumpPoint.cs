using UnityEngine;

[System.Serializable]

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private float m_TimeCreate=0; //время создания

    [SerializeField]
    private int m_Line; //линия в которой находится пуш

    [SerializeField]
    private int m_Collumn; //колонка в которой находится пуш

    [SerializeField]
    private CollectableGO m_PrefBonus; //префаб колетблза на пушере

    private float speed; //скорость 
    private GameObject bonus; //колектблз на пушере
    private bool isCreateBonus=false;
    //private GameObject HelpPush; //изображение действия
    void Start()
    {
        if (m_TimeCreate != 0)
        {
            LevelGenerator.Instance.AltPushers.Add(this);
            this.gameObject.SetActive(false);
        }

        //HelpPush = transform.FindChild("HelpPush").gameObject;
        
    }

    void Update()
    {
        if (LevelGenerator.Instance.IsRunLevel)
        {
            MovePusher();
        }
        if (gameObject.activeSelf && m_PrefBonus && !isCreateBonus)//если пушер активировался, у него есть бонус и он ещё не инициализирован
        {
            Vector3 bonusPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            bonusPos.y += 0.6f;
            Bonus = Instantiate(PrefBonus.gameObject, bonusPos, transform.rotation) as GameObject;
            Bonus.transform.parent = transform;
            isCreateBonus = true;
        }
    }

    //пользовательские методы
    void MovePusher()
    {
        speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    //свойства
    public int Line
    {
        get{ return this.m_Line; }
        set{ this.m_Line = value; }
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
    public float TimeCreate
    {
        get { return this.m_TimeCreate; }
        set { this.m_TimeCreate = value; }
    }
    public GameObject Bonus
    {
        get { return this.bonus; }
        set { this.bonus = value; }
    }
    public CollectableGO PrefBonus
    {
        get { return this.m_PrefBonus; }
        set { this.m_PrefBonus = value; }
    }
}