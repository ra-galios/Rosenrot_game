using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private int m_Line; //линия в которой находится пуш
    [SerializeField]
    private int m_Collumn; //колонка в которой находится пуш
    [SerializeField]
    private CollectableGO m_PrefBonus; //префаб колетблза на пушере
    [SerializeField]
    private GameInput.PlayerAction m_Action;
    [SerializeField, HeaderAttribute("скала из семечки")]
    private bool isSeed = false;
    [SerializeField, HeaderAttribute("обваливающаяся скала")]
    private bool canFall = false;
    private bool isFalling = false;
    private Rigidbody2D rBody;
    private bool isCreateBonus = false;


    private float speed; //скорость 
    private Animator anim;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();

        if (isSeed)
        {
            anim = GetComponent<Animator>();
        }
    }

    void OnEnable()
    {
        if (canFall)
        {
            PlayerBehaviour.PlayerChangeLine += SetFallPusher;
        }
    }

    void OnDisable()
    {
        if (canFall)
        {
            PlayerBehaviour.PlayerChangeLine -= SetFallPusher;
        }
    }

    void OnBecameVisible()
    {
        if (isSeed)
        {
            anim.SetBool("CreatePusher", true);
        }
    }

    void Update()
    {
        if (LevelGenerator.Instance.IsRunLevel && !isFalling)
        {
            MovePusher();
        }

        if (gameObject.activeSelf && m_PrefBonus && !isCreateBonus)//если пушер активировался, у него есть бонус и он ещё не инициализирован
        {
            Vector3 bonusPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            bonusPos.y += 1.4f;
            GameObject Bonus = Instantiate(m_PrefBonus.gameObject, bonusPos, transform.rotation) as GameObject;
            Bonus.transform.parent = transform;
            isCreateBonus = true;
        }
    }

    void MovePusher()
    {
        speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    public void SetFallPusher(int playerIdLine)
    {
        if (m_Line < playerIdLine && rBody)
        {
            isFalling = true;
            rBody.bodyType = RigidbodyType2D.Dynamic;
            Animator anim = GetComponent<Animator>();
            anim.SetBool("falling 0", true);
            anim.SetBool("falling", true);
                 rBody.gravityScale = 1f;
        }
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