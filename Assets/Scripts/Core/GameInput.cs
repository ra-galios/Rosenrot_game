using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameInput : CreateSingletonGameObject<GameInput>
{
    public enum PlayerAction { climb, jump, doubleJump, question, none };
    public Action<PlayerAction> PlayerInputAction; //вставляем enum в Action

    private float firstClickTime { get; set; }
    private float secondClickTime { get; set; }
    private Vector2 firstClickPosition { get; set; }
    private Vector2 secondClickPosition { get; set; }
    private Coroutine Coroutine { get; set; }  //переменная корутины
    private GameObject hitObject;//на что тыкнули
    private GameObject bonusHitObject;//бонус того, на что тыкнули
    private PlayerBehaviour player;
    private Vector2 playerPos;

    private bool clickedOnce;
    private float waitTime;
   private PlayerAction action;


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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        playerPos = player.transform.position;
        action = PlayerAction.none;
        firstClickTime = 0;
        secondClickTime = 0;
        clickedOnce = false;
        waitTime = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Market.Instance.Health > 0 || LevelGenerator.Instance.IsRunLevel)
        {

            if (Input.GetMouseButtonDown(0))
            {
                hitObject = GetHitObject();
                if (hitObject)
                {
                    if (!clickedOnce)                //клик
                    {
                        action = PlayerAction.climb;//
                        firstClickTime = Time.time;
                        clickedOnce = true;
                        //print("click");
                    }
                    else                             //даблклик
                    {
                        action = PlayerAction.jump;
                        clickedOnce = false;
                        //print("double");
                    }
                }
                firstClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);       //для свайпа
            }

            if (Input.GetMouseButtonUp(0))              //проверяем свайп
            {
                secondClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                playerPos = player.transform.position;
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
                            var diffV = Mathf.Abs(doubleJunmpPusher.Collumn - player.IdCollumn);
                            int diffH = doubleJunmpPusher.Line - player.IdLine;
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

                        Debug.DrawLine(playerPos, pusherPos);
                        if (Vector2.Distance(projection, firstClickPosition) < 2.5f)    //если расстояние меньше 2.5
                        {
                            float angle = Vector2.Angle(normal, (secondClickPosition - firstClickPosition).normalized);
                            //Debug.Log(pusher.name + ", angle: " + angle);

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

            if (Time.time > firstClickTime + waitTime)  //если время ожидания истекло
            {
                if (hitObject != null && action != PlayerAction.none)    //начинаем прыжок
                {
                    Debug.Log("Do: " + action);
                    PlayerInputAction.Invoke(action);
                }
                clickedOnce = false;        //сбрасываем первый клик
                action = PlayerAction.none;
            }
            if(hitObject == null && Input.GetMouseButtonUp(0) && action != PlayerAction.doubleJump)
            {
                //print("Fail"); //+обрабатывать врагов врагов
            }
        }
    }


    GameObject GetHitObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null && hit.transform.gameObject.GetComponent<JumpPoint>())
        {
            return hit.transform.gameObject; //объект на который нажали
        }
        else
        {
            return null;
        }
    }

    //свойства
    public GameObject HitObject
    {
        get { return this.hitObject; }
    }
}
