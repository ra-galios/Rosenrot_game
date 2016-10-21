using UnityEngine;
using System.Collections;
using System;

public class GameInput : CreateSingletonGameObject<GameInput> {
    public enum PlayerAction { climb, jump, doubleJump };
    Vector2 StartMousePosition;
    public Action<PlayerAction> PlayerInputAction; //вставляем enum в Action
    float firstClickTime = 0;
    float secondClickTime = 0;

    Coroutine InputCoroutine;  //переменная корутины
   

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (InputCoroutine == null)  //если корутина не запущена, то запускаем и зпоминаем время первого клика для использования в корутине
            {
                firstClickTime = Time.time;
                StartMousePosition = Input.mousePosition;
                InputCoroutine = StartCoroutine(WaitInput()); 
            }
            else
            {                                                  // если корутина запущена и secondClickTime не равно нулю, то останавливаем ее
                if (secondClickTime == 0)                       // если равно нулю, то зпоминаем время второго клика для использования в корутине
                {
                    secondClickTime = Time.time;
                }
                else
                {
                    StopCoroutine(WaitInput());
                }
            }

        }

    }
    IEnumerator WaitInput()   //корутина, тут определяем тип Action'a и инвокаем его
    {
        yield return new WaitForSeconds(0.35f); // ждем 0.35 сек и продолжаем
        var action = PlayerAction.climb; // предполагаем что climb(малый прыжок)
        if (secondClickTime > 0)            //если был вторй клик то записываем jump(средний прыжок)
        {
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


        InputCoroutine = null; // выключам корутину
    }
}
