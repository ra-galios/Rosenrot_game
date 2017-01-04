using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    private GameObject hitObject;
    private JumpPoint hitJumpPoint;
    private Rigidbody2D rig2D;
    [SerializeField]
    private int idLine = 0;
    [SerializeField]
    private int idCollumn = 1;
    private bool isPlayerFall = false;
    private bool playerStaticPush = true;
    private Animator animator;
    private Coroutine LerpCoroutine; //здесь будем хранить выполняющуюся корутину лерпа движения игрока
    private Vector3 playerOffset;

    [RangeAttribute(0f, 15f)]
    [SerializeField]
    private float m_SpeedMultiplayer = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
        playerOffset = new Vector3(0f, 1f, 0f);    //приподнять игрока
    }

    void OnEnable()
    {
        GameInput.Instance.PlayerInputAction += JumpToNext; //подписываемся на эфир PlayerInputAction и ждём когда он скажет чё нам делать
    }

    void OnDisable()
    {
        GameInput.Instance.PlayerInputAction -= JumpToNext; //отписываемся от эфира
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>())
        {
            PlayerFall();
        }
    }

    void JumpToNext(GameInput.PlayerAction action) //Когда в эфире PlayerInputAction что-то "прозвучит", запускается JumpToNext
    {
        if (!isPlayerFall)
        {
            this.hitObject = GameInput.Instance.HitObject;
            this.hitJumpPoint = hitObject != null ? hitObject.GetComponent<JumpPoint>() : null;
            if (hitObject && hitJumpPoint)// && LevelGenerator.Instance.IsRunLevel)//если есть объект на который нажали мышкой
            {
                if (hitJumpPoint)
                {
                    if (hitJumpPoint.Line - 1 == idLine)
                    {
                        if (action == hitJumpPoint.Action)
                        {
                            if (LerpCoroutine == null)
                                StartCoroutine("Lerp");
                        }
                        else if (hitJumpPoint.Action == GameInput.PlayerAction.question)
                        {
                            GameInput.PlayerAction questionPusherAction = GetQuestionPusherType(hitJumpPoint);
                            print("Expected: " + questionPusherAction);
                            if (questionPusherAction == action)
                            {
                                if (LerpCoroutine == null)
                                    StartCoroutine("Lerp");
                            }
                            else
                            {
                                if (!playerStaticPush)
                                    PlayerFall();
                            }
                        }
                    }
                }
            }
            else
            {
                PlayerFall();
            }
        }

    }

    void PlayerFall()
    {//падение игрока
        StopCoroutine("Lerp");
        isPlayerFall = true;
        rig2D.isKinematic = false;
        transform.parent = null;
        StartCoroutine(Fall());

    }

    IEnumerator Lerp()
    {
        transform.parent = hitObject.transform;
        if (LevelGenerator.Instance.IsRunLevel == false)
            LevelGenerator.Instance.StartLevel();
        var boxColl = GetComponent<BoxCollider2D>().enabled;
        boxColl = false;
        Vector2 _from = transform.localPosition;
        Vector2 _to = Vector3.zero;
        float _t = 0f;
        while (_t < 1)
        {
            _t += Time.deltaTime * m_SpeedMultiplayer;
            transform.localPosition = Vector2.Lerp(_from, _to, _t); //перемещаем тело в позицию объекта, на который нажали
            yield return null;
        }

        if (hitJumpPoint.Bonus)
        {
            CollectableGO bonus = hitJumpPoint.Bonus.GetComponent<CollectableGO>();
            if (!bonus.collected)
            {
                bonus.transform.parent = transform;
                bonus.transform.position += new Vector3(0.04f, 1.5f, 0f);      //бонус над головой Якова
                bonus.EnterBonus();
            }
        }

        
        idLine = hitJumpPoint.Line;
        idCollumn = hitJumpPoint.Collumn;
        playerStaticPush = hitObject.layer == 10 ? true : false; // 10 - это layer StaticPushers, со статического пушера упасть нельзя
        LerpCoroutine = null;
        boxColl = true;
        isPlayerFall = false; //уже не падаем если падали
    }

    IEnumerator Fall()//пока падаем, отслеживаем нажатие на кнопку мыши и целимся в ближайший пушер
    {
        GameObject[] _pushers = GameObject.FindGameObjectsWithTag("Pusher"); //берём все созданные на данный момент пушеры
        float _minDist = 100f; //немного чисел с неба
        float _dist;//дистанция до ближайшего пушера
        while (isPlayerFall) //пока мы падаем
        {

            if (Input.GetMouseButtonDown(0)) //нажали кнопку мыши
            {
                foreach (GameObject _push in _pushers) //какой пушер ближе всех?
                {
                    _dist = Vector2.Distance(_push.transform.position, transform.position); //дистанция от игрока до пушера
                    if (_dist < _minDist)
                    {
                        _minDist = _dist; //минимальная
                        hitObject = _push; //а вот и он, наш спаситель
                        hitJumpPoint = hitObject != null ? hitObject.GetComponent<JumpPoint>() : null;
                    }
                }
                rig2D.isKinematic = true;
                StartCoroutine("Lerp"); //перемещаемся к спасительному пушеру
            }
            yield return null;
        }
    }

    public GameInput.PlayerAction GetQuestionPusherType(JumpPoint pusher)
    {
        var diffV = Mathf.Abs(pusher.Collumn - idCollumn);
        var diffH = Mathf.Abs(pusher.Line - idLine);

        GameInput.PlayerAction pusherType = GameInput.PlayerAction.climb;

        if (diffV == 0 && diffH == 1)
        { //подтягивание
            pusherType = GameInput.PlayerAction.climb;
        }
        else if ((diffV == 1 && diffH == 1) || (diffV == 1 && diffH == 0))
        { //прыжок
            pusherType = pusher.Action = GameInput.PlayerAction.jump;
        }
        else if (diffV == 2 || diffH == 2)
        { //двойной прыжок
            pusherType = pusher.Action = GameInput.PlayerAction.doubleJump;
        }

        return pusherType;
    }

    //свойства
    public int IdLine
    {
        get { return this.idLine; }
    }

    public int IdCollumn
    {
        get { return this.idCollumn; }
    }
}
