using UnityEngine;
using System.Collections;
using System;

public class PlayerBehaviour : MonoBehaviour
{
    private GameObject hitObject;
    private JumpPoint hitJumpPoint;
    private Rigidbody2D rig2D;
    [SerializeField]
    private int idLine = 0;
    public static Action<int> PlayerChangeLine;
    [SerializeField]
    private int idCollumn = 1;
    private bool isPlayerFall = false;
    private bool onPlatformAfterFall = true;
    private Coroutine LerpCoroutine; //здесь будем хранить выполняющуюся корутину лерпа движения игрока
    private PlayerAnimationController animController;

    [RangeAttribute(0f, 15f)]
    [SerializeField]
    private float m_SpeedMultiplayer = 2f;

    void Awake()
    {
        GameInput.Instance.playerBeh = this;
        GameController.Instance.PlayerBeh = this;
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
                                PlayerFall();
                            }
                        }
                        else
                        {
                            PlayerFall();
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
                GrabAfterFall();
            }
        }

        JumpPoint jumpPoint = GameInput.Instance.HitObject != null ? hitObject.GetComponent<JumpPoint>() : null;
        if (!jumpPoint)
            PlayerFall();
    }

    void PlayerFall()
    {//падение игрока
        if (LevelGenerator.Instance.IsRunLevel)
        {
            StopCoroutine("Lerp");
            LerpCoroutine = null;
            isPlayerFall = true;
            onPlatformAfterFall = false;
            rig2D.bodyType = RigidbodyType2D.Dynamic;
            rig2D.gravityScale = 0.8f;
            transform.parent = null;
            animController.SetFall(true);
        }
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

        if(hitJumpPoint.IsSeed && Market.Instance.Seeds > 0)
        {
            Market.Instance.Seeds--;
        }

        onPlatformAfterFall = true;
        idLine = hitJumpPoint.Line;
        AchievementsController.AddToAchievement(AchievementsController.Type.GotHigh, 1);
        if(PlayerChangeLine != null)
                PlayerChangeLine.Invoke(idLine);
        idCollumn = hitJumpPoint.Collumn;
        LerpCoroutine = null;
        boxColl = true;
    }

    private void GrabAfterFall()
    {
        isPlayerFall = false;
        rig2D.bodyType = RigidbodyType2D.Static;
        animController.SetFall(isPlayerFall);
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
