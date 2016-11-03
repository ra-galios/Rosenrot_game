using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]

public class JumpPoint : MonoBehaviour
{
    public enum actionPlayer { climb, jump, doubleJump };
    private float speed;

    public float Speed{
        get{return speed;}
        set{speed = value;}
    }

    private GameObject target;
    private actionPlayer action;
    private int line;

    void isEnable()
    {
        this.target = GameObject.FindGameObjectWithTag("Player");
    }
    void MovePusher()
    {
        this.transform.Translate(Vector2.down * this.speed * Time.deltaTime);
    }

    void Update()
    {
        MovePusher();
    }

    //свойства
    public int Line
    {
        get
        {
            return this.line;
        }
        set
        {
            this.line = value;
        }
    }
    public actionPlayer Action
    {
        get
        {
            //какое действие нужно произвести, чтобы попасть на этот пушер
            //если текущая линия следующая после линии, на которой находится игрок
            //и насколько игрок далеко от пушера

            return this.action;
        }
        set
        {
            this.action = value;
        }
    }
}