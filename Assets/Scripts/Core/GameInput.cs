using UnityEngine;
using System.Collections;
using System;

public class GameInput : CreateSingletonGameObject<GameInput>
{
    public enum PlayerAction { climb, jump, doubleJump };
    public Action<PlayerAction> PlayerInputAction; //вставляем enum в Action

    Vector2 StartMousePosition;
    float firstClickTime = 0;
    Vector3 firstClickPosition;

    float secondClickTime = 0;
    Vector3 secondClickPosition;

    Coroutine CorutineIsEnable;  //переменная корутины

    // Update is called once per frame
    void Update()
    {
        //первый клик мышкой
        if (Input.GetMouseButtonDown(0))
        {
            if (CorutineIsEnable == null)  //если корутина не запущена, то запускаем и зпоминаем время первого клика для использования в корутине
            {
                firstClickTime = Time.time;
                firstClickPosition = Input.mousePosition; //узнаём позицию первого клика
                StartMousePosition = firstClickPosition;
                CorutineIsEnable = StartCoroutine(WaitInput());
            }
            else
            {
                //второй клик мышкой
                if (secondClickTime == 0)   //если равно нулю, то зпоминаем время второго клика для использования в корутине                       
                {                               
                    secondClickTime = Time.time;
                    secondClickPosition = Input.mousePosition; //позиция второго клика
                }
                else
                {
                    StopCoroutine(WaitInput()); // если корутина запущена и secondClickTime не равно нулю, то останавливаем ее
                }
            }
        }
    }

    IEnumerator WaitInput()   //корутина, тут определяем тип Action'a и инвокаем его
    {
        yield return new WaitForSeconds(0.35f); // ждем 0.35 сек и продолжаем
        var action = PlayerAction.climb; // предполагаем что climb(малый прыжок)
        if (secondClickTime > 0 && Vector2.Distance(firstClickPosition,secondClickPosition) < 3f)            //если был второй клик то записываем jump(средний прыжок)
        {                                                                           //узнаём дистанцию между кликами, чтобы пофиксить баг
            if (PlayerInputAction != null)
            {
                action = PlayerAction.jump;
            }
        }
        else
        if (Vector2.Distance(StartMousePosition, Input.mousePosition) >= 20f && secondClickTime == 0) // если мы свайпнули и второго клика не было(число - минимальная длинна свайпа)
        {
            if (PlayerInputAction != null)           
            {                                        // то записываем doubleJump(дальний прыжок)
                action = PlayerAction.doubleJump;
            }
        }
        PlayerInputAction.Invoke(action);   // отправляем в эфир(принимать будем в PlayerBehaviour скрипте)
        secondClickTime = 0;         // обнуляем время второго клика
        CorutineIsEnable = null; // выключам корутину
    }
}
