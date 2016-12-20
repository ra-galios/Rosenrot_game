using UnityEngine;
using System.Collections;
using System;


public class GameInput : CreateSingletonGameObject<GameInput>
{
    public enum PlayerAction { climb, jump, doubleJump, question };
    public Action<PlayerAction> PlayerInputAction; //вставляем enum в Action

    private float firstClickTime {get; set;}
    private float secondClickTime { get; set; }
    private Vector3 firstClickPosition { get; set; }
    private Vector3 secondClickPosition { get; set; }
    private Coroutine Coroutine { get; set; }  //переменная корутины
    private GameObject hitObject;//на что тыкнули
    private GameObject bonusHitObject;//бонус того, на что тыкнули

    void Start()
    {
        firstClickTime = 0;
        secondClickTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Market.Instance.Health > 0 || LevelGenerator.Instance.IsRunLevel)
        {
            //первый клик мышкой
            if (Input.GetMouseButtonDown(0))
            {
                hitObject = GetHitObject(); //узнаём куда ткнули
                if (hitObject)//если попали в пушер
                {
                    if (Coroutine == null)  //если корутина не запущена, то запускаем и зпоминаем время первого клика для использования в корутине
                    {
                        firstClickTime = Time.time;
                        firstClickPosition = Input.mousePosition; //узнаём позицию первого клика
                        Coroutine = StartCoroutine("WaitInput");
                    }
                    else //если карутина запущена
                    {
                        //второй клик мышкой
                        if (secondClickTime == 0)   //если равно нулю (второго клика небыло), то зпоминаем время второго клика для использования в корутине                       
                        {
                            secondClickTime = Time.time; //время второго клика
                            secondClickPosition = Input.mousePosition; //позиция второго клика
                        }
                        else //второй клик уже был, карутина не нужна
                        {
                            StopCoroutine("WaitInput"); // если корутина запущена и secondClickTime не равно нулю, то останавливаем ее
                        }
                    }
                }
            }
        }
    }

    IEnumerator WaitInput()   //корутина, тут определяем тип Action'a и инвокаем его
    {
        yield return new WaitForSeconds(0.35f); // ждем 0.35 сек и продолжаем
        var action = PlayerAction.climb; // предполагаем что climb(малый прыжок)
        if (secondClickTime > 0 && Vector2.Distance(firstClickPosition, secondClickPosition) < 20f)            //если был второй клик то записываем jump(средний прыжок)
        {                                                                           //узнаём дистанцию между кликами, чтобы пофиксить баг
            if (PlayerInputAction != null)
            {
                action = PlayerAction.jump;
            }
        }
        else
        if (Vector2.Distance(firstClickPosition, Input.mousePosition) >= 20f && secondClickTime == 0) // если мы свайпнули и второго клика не было(число 20 - минимальная длинна свайпа)
        {
            if (PlayerInputAction != null)
            {                                        
                action = PlayerAction.doubleJump;
            }
        }

        PlayerInputAction.Invoke(action);   // отправляем в эфир(принимать будем в PlayerBehaviour скрипте)

        secondClickTime = 0;         
        Coroutine = null; 
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
