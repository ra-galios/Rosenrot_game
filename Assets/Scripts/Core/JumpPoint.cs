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
}