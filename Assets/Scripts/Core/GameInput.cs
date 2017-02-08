using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;


public class GameInput : CreateSingletonGameObject<GameInput>
{
    public enum PlayerAction { climb, jump, doubleJump, question, climbAfterFall };
    public Action<PlayerAction> PlayerInputAction; //вставляем enum в Action

    public PlayerBehaviour playerBeh;

    private float firstClickTime { get; set; }
    private Vector2 firstClickPosition { get; set; }
    private Vector2 secondClickPosition { get; set; }
    private GameObject hitObject;//на что тыкнули
    private Vector2 playerPos;
    private bool clickedOnce;
    private bool readInput;
    private bool clickOverUI = false;
    private float waitTime;
    private PlayerAction action;
    private LayerMask hitObjectMask = 1537;              //default, pushers, staticPushers
    private JumpPoint clickedPusher;

    void OnLevelWasLoaded()
    {
        Initialization();
    }

    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        firstClickTime = 0;
        clickedOnce = false;
        readInput = true;
        waitTime = 0.35f;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Market.Instance.Health > 0 || LevelGenerator.Instance.IsRunLevel) && playerBeh)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickOverUI = EventSystem.current.IsPointerOverGameObject();
                CheckClick();
            }
            if (Input.GetMouseButtonUp(0))
            {
                readInput = false;
                //CheckSwipe();
            }

            if (Time.time > firstClickTime + waitTime && !readInput)  //если истекло время ожидания и ввод не прочитан
            {

                if (hitObject != null && hitObject.GetComponent<Enemy>())
                {
                    if (Market.Instance.Bomb > 0)
                    {
                        hitObject.GetComponent<Enemy>().DestroyEnemy();
                    }
                }
                else if (!clickOverUI && Time.timeScale > 0f)
                {
                    CheckDoubleClick();
                    CheckClimbAfterFall();
                    //Debug.Log("Do: " + action);
                    PlayerInputAction.Invoke(action);
                }

                clickedPusher = null;
                clickedOnce = false;        //сбрасываем первый клик
                readInput = true;
            }
        }
    }


    private GameObject GetHitObject()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, hitObjectMask);
        if (hit.transform != null)
        {
            return hit.transform.gameObject; //объект на который нажали
        }
        else
        {
            return null;
        }

    }

    private void CheckClick()
    {
        hitObject = GetHitObject();
        if (hitObject)
        {
            clickedPusher = hitObject.GetComponent<JumpPoint>();
            if (clickedPusher)
            {
                readInput = false;

                if (!clickedOnce)                //клик
                {
                    action = PlayerAction.climb;
                    firstClickTime = Time.time;
                    firstClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);       //для свайпа
                    clickedOnce = true;
                }
                else                             //даблклик
                {
                    action = PlayerAction.jump;
                    clickedOnce = false;
                }
            }
        }
    }

    private void CheckDoubleClick()
    {
        if (Vector2.Distance(firstClickPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)) > 0.5f && clickedOnce) //если длина свайпа больше 0.5
        {
            action = PlayerAction.doubleJump;
        }
    }

    private void CheckClimbAfterFall()
    {
        if (playerBeh.IsPlayerFall && clickedPusher) //если игрок падает и нажал на пушер
        {
            if (clickedPusher.Line < playerBeh.IdLine)
            {
                action = PlayerAction.climbAfterFall;
            }
        }
    }

    private void CheckSwipe()
    {
        readInput = false;
        secondClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPos = playerBeh.transform.position;
        GameObject[] pushers = GameObject.FindGameObjectsWithTag("Pusher");
        List<GameObject> swipePushers = new List<GameObject>();
        float prevSwipeAngle = 180f;
        if (Vector2.Distance(firstClickPosition, secondClickPosition) > 1f) //если длина больше 1
        {
            foreach (GameObject pusher in pushers)              //находим все доступные для свайпа пушеры
            {
                PlayerAction pusherAction = pusher.GetComponent<JumpPoint>().Action;
                if (pusherAction == PlayerAction.doubleJump || pusherAction == PlayerAction.question)
                {
                    JumpPoint doubleJunmpPusher = pusher.GetComponent<JumpPoint>();
                    int diffV = Mathf.Abs(doubleJunmpPusher.Collumn - playerBeh.IdCollumn);
                    int diffH = doubleJunmpPusher.Line - playerBeh.IdLine;
                    if (diffH == 1 && diffV == 2)
                    {
                        swipePushers.Add(pusher);
                    }
                }
            }
            foreach (GameObject pusher in swipePushers)             //находим пушер к которому свайпнули
            {
                Vector2 pusherPos = pusher.transform.position;
                Vector2 normal = (pusherPos - playerPos).normalized;  //по направлению к камню(не значку)

                Vector2 projection = new Vector3(playerPos.x, playerPos.y) + Vector3.Project(firstClickPosition - playerPos, normal);

                //Debug.DrawLine(playerPos, pusherPos);
                if (Vector2.Distance(projection, firstClickPosition) < 2.5f)    //если расстояние меньше 2.5
                {
                    float angle = Vector2.Angle(normal, (secondClickPosition - firstClickPosition).normalized);

                    if (angle < 35f && angle < prevSwipeAngle)        //если угол меньше 35 и меньше предыдущего
                    {
                        prevSwipeAngle = angle;
                        hitObject = pusher;                //устанавливаем пушер к которому свайпнули 
                        action = PlayerAction.doubleJump;
                    }
                }
            }
        }
    }

    //свойства
    public GameObject HitObject
    {
        get { return this.hitObject; }
    }
}
