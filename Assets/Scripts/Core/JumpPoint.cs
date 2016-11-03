using UnityEngine;

[System.Serializable]

public class JumpPoint : MonoBehaviour
{
    [SerializeField]
    private float timeCreate=0;
	[SerializeField]
	public float timeLastCreate=0;
    private int line;
    private float speed;

	void isEnable()
	{
		TimeLastCreate = 0f;
	}

    void Update()
    {
        MovePusher();
    }

    //пользовательские методы
    void MovePusher()
    {
        this.transform.Translate(Vector2.down * this.speed * Time.deltaTime);
    }

    //свойства
    
    public int Line
    {
        get{ return this.line; }
        set{ this.line = value; }
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
    public float TimeLastCreate
    {
        get { return this.timeLastCreate; }
        set { this.timeLastCreate = value; }
    }
}