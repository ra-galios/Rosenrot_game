using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    public int nextPusher { get; set; }
    public Transform TargetTransform { get; set; }
    public GameObject hitObject { get; set; }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetHitObject();//устанавливаем в какой объект нажали и записываем в hitObject, если таковый был, иначе null
        }
    }

    void OnEnable () {//когда сцена запустилась
        GameInput.Instance.PlayerInputAction += JumpToNext; //подписываемся на эфир PlayerInputAction и ждём когда он скажет чё нам делать
	}

	void OnDisable () {//когда всё потухло
        GameInput.Instance.PlayerInputAction -= JumpToNext; //отписываемся от эфира
    }

    void JumpToNext(GameInput.PlayerAction action) //Когда в эфире PlayerInputAction что-то "прозвучит", запускается JumpToNext
    {
        if (hitObject)//если есть объект на который нажали мышкой
        {
            float dist = Mathf.Abs(transform.position.x - hitObject.transform.position.x); // дистанция от игрока до hitObject'a
            print(dist);                                                                   

            if (dist < 0.3f && action == GameInput.PlayerAction.climb){ //подтягивание
                Lerp();
            }

            if (dist <= 1.8f && dist >= 0.3f && action == GameInput.PlayerAction.jump){ //прыжок
                Lerp();
            }

            if (dist > 2.5f && action == GameInput.PlayerAction.doubleJump){ //двойной прыжок
                Lerp();
            }
        }
    }

    void SetHitObject(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null){
            print("vozvratTransform");
            this.hitObject = hit.transform.gameObject; //объект на который нажали
        }
        else{
            print("vozvratNULL");
            this.hitObject = null;
        }
    }

    void Lerp()//нужно передеать в карутину для плавного передвижения главного хероя
    {
        Vector2 newPosition = hitObject.transform.position;
        transform.position = Vector2.Lerp(transform.position, newPosition, 1); //перемещаем тело в позицию объекта, на который нажали
    }
}
