using UnityEngine;

[System.Serializable]

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private float timeCreate=0;

    [SerializeField]
    private int line;

    [SerializeField]
    private int collumn;

    private float speed;

    [SerializeField]
    private CollectableGO prefBonus;
    private GameObject bonus;
    private bool isCreateBonus=false;

    void Start()
    {
        if (timeCreate != 0)
        {
            LevelGenerator.Instance.AltPushers.Add(this);
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (LevelGenerator.Instance.IsRunLevel)
        {
            MovePusher();
        }
        if (gameObject.activeSelf && prefBonus && !isCreateBonus)//если пушер активировался, у него есть бонус и он ещё не инициализирован
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
        Speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * Speed * Time.deltaTime);
    }

    //свойства
    public int Line
    {
        get{ return this.line; }
        set{ this.line = value; }
    }
    public int Collumn
    {
        get { return this.collumn; }
        set { this.collumn = value; }
    }
    public float Speed
    {
        get { return this.speed; }
        set { this.speed = value; }
    }
    public float TimeCreate
    {
        get { return this.timeCreate; }
        set { this.timeCreate = value; }
    }
    public GameObject Bonus
    {
        get { return this.bonus; }
        set { this.bonus = value; }
    }
    public CollectableGO PrefBonus
    {
        get { return this.prefBonus; }
        set { this.prefBonus = value; }
    }
}