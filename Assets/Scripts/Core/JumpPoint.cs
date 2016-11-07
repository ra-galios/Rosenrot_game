using UnityEngine;

[System.Serializable]

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private float timeCreate=0;

    private int line;
    private int collumn;
    private float speed;

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
        this.speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * this.speed * Time.deltaTime);
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