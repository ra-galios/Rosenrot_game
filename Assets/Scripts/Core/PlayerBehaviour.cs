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
    private bool onPlatformAfterFall = true;
    private bool playerStaticPush = true;
    private Coroutine LerpCoroutine; //здесь будем хранить выполняющуюся корутину лерпа движения игрока
    private PlayerAnimationController animController;

    [RangeAttribute(0f, 15f)]
    [SerializeField]
    private float m_SpeedMultiplayer = 2f;

    [SerializeField]
    private GameObject staffObj;

    void Awake()
    {
        GameInput.Instance.playerBeh = this;
        GameController.Instance.playerBeh = this;
        animController = GetComponentInChildren<PlayerAnimationController>();
        rig2D = GetComponent<Rigidbody2D>();
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

        CollectableGO bonus = collider.gameObject.GetComponent<CollectableGO>();
        if (bonus)
        {
            bonus.EnterBonus();
        }
    }

    void JumpToNext(GameInput.PlayerAction action) //Когда в эфире PlayerInputAction что-то "прозвучит", запускается JumpToNext
    {
        if (LerpCoroutine == null)
        {
            this.hitObject = GameInput.Instance.HitObject;
            this.hitJumpPoint = hitObject != null ? hitObject.GetComponent<JumpPoint>() : null;
            if (!isPlayerFall && onPlatformAfterFall)
            {
                if (hitObject && hitJumpPoint)// && LevelGenerator.Instance.IsRunLevel)//если есть объект на который нажали мышкой
                {
                    if (hitJumpPoint.Line - 1 == idLine)
                    {
                        if (action == hitJumpPoint.Action)
                        {
                            LerpCoroutine = StartCoroutine("Lerp");
                            animController.SetJump(action);
                        }
                        else if (hitJumpPoint.Action == GameInput.PlayerAction.question)
                        {
                            GameInput.PlayerAction questionPusherAction = GetQuestionPusherType(hitJumpPoint);
                            if (questionPusherAction == action)
                            {
                                LerpCoroutine = StartCoroutine("Lerp");
                                animController.SetJump(action);
                            }
                            else
                            {
                                if (!playerStaticPush)
                                    PlayerFall();
                            }
                        }
                    }
                    else
                    {
                        PlayerFall();
                    }
                }
            }
            else if (action == GameInput.PlayerAction.climbAfterFall)
            {
                LerpCoroutine = StartCoroutine("ClimbAfterFall");
            }
        }

        JumpPoint jumpPoint = GameInput.Instance.HitObject != null ? hitObject.GetComponent<JumpPoint>() : null;
        if (!jumpPoint)
            PlayerFall();
    }

    void PlayerFall()
    {//падение игрока
        StopCoroutine("Lerp");
        LerpCoroutine = null;
        isPlayerFall = true;
        onPlatformAfterFall = false;
        rig2D.bodyType = RigidbodyType2D.Dynamic;
        rig2D.gravityScale = 0.8f;
        transform.parent = null;
        animController.SetFall(true);
        //StartCoroutine(Fall());

    }

    IEnumerator Lerp()
    {
        if (onPlatformAfterFall)
            yield return new WaitForSeconds(.3f);

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
        onPlatformAfterFall = true;
        idLine = hitJumpPoint.Line;
        idCollumn = hitJumpPoint.Collumn;
        playerStaticPush = hitObject.layer == 10 ? true : false; // 10 - это layer StaticPushers, со статического пушера упасть нельзя
        LerpCoroutine = null;
        boxColl = true;
    }

    private IEnumerator ClimbAfterFall()
    {
        animController.SetFall(true);
        //staffObj.transform.position = transform.position;
        StaffBehaviour staffBeh = staffObj.GetComponent<StaffBehaviour>();
        staffBeh.moveCoroutine = StartCoroutine(staffBeh.MoveStaff(hitObject));

        while (staffBeh.moveCoroutine != null)
        {
            yield return null;
        }

        isPlayerFall = false;
        rig2D.bodyType = RigidbodyType2D.Static;
        animController.SetFall(false);
        StartCoroutine("Lerp");
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

    public bool IsPlayerFall
    {
        get { return this.isPlayerFall; }
    }
}
