using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour {
    private GameObject hitObject;
    private JumpPoint hitJumpPoint;
    private Rigidbody2D rig2D;
    private int idLine = 0;
    private int idCollumn = 1;
    private bool isPlayerFall=false;

    Coroutine LerpCoroutine; //здесь будем хранить выполняющуюся корутину лерпа движения игрока
    
    void Start()
    {
        rig2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetHitObject();//устанавливаем в какой объект нажали и записываем в hitObject, если таковый был, иначе null
            if(!LevelGenerator.Instance.IsRunLevel)
                LevelGenerator.Instance.StartLevel();
        }
    }

    void OnEnable () {//когда объект активирован
        GameInput.Instance.PlayerInputAction += JumpToNext; //подписываемся на эфир PlayerInputAction и ждём когда он скажет чё нам делать
	}

	void OnDisable () {//когда всё потухло
        GameInput.Instance.PlayerInputAction -= JumpToNext; //отписываемся от эфира
    }

    void JumpToNext(GameInput.PlayerAction action) //Когда в эфире PlayerInputAction что-то "прозвучит", запускается JumpToNext
    {
        if (hitObject)//если есть объект на который нажали мышкой
        {
			float dist = Vector2.Distance(transform.position, hitObject.transform.position);//Mathf.Abs(transform.position.x - hitObject.transform.position.x); // дистанция от игрока до hitObject'a       

            var diffV = Mathf.Abs(hitJumpPoint.Collumn - idCollumn);
            var diffH = Mathf.Abs(hitJumpPoint.Line - idLine);

            if (!isPlayerFall)
            {
                switch (action)
                {
                    case GameInput.PlayerAction.climb:
                        {
                            if(diffV == 0 && diffH == 1)
                            { //подтягивание
                                if (LerpCoroutine == null)
                                    StartCoroutine(Lerp());
                            }
                            else
                            {//Падение при неверном нажатии на пушер
                                if(hitObject.layer != 10) //10 - это StaticPushers, со статического пушера упасть нельзя
                                    PlayerFall();
                            }
                            break;
                        }
                    case GameInput.PlayerAction.jump:
                        {
                            if((diffV == 1 && diffH == 1) || (diffV == 1 && diffH == 0))
                            { //прыжок
                                if (LerpCoroutine == null)
                                    StartCoroutine(Lerp());
                            }
                            else
                            {//Падение при неверном нажатии на пушер
                                if (hitObject.layer != 10) //10 - это StaticPushers
                                    PlayerFall();
                            }
                            break;
                        }
                    case GameInput.PlayerAction.doubleJump:
                        {
                            if(diffV >= 2 || diffH >= 2)
                            { //двойной прыжок
                                if (LerpCoroutine == null)
                                    StartCoroutine(Lerp());
                            }
                            else
                            {//Падение при неверном нажатии на пушер
                                if (hitObject.layer != 10) //10 - это StaticPushers
                                    PlayerFall(); 
                            }
                            break;
                        }
                }
            }
        }
    }

    void SetHitObject(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null && hit.transform.gameObject.tag == "Pusher"){
            rig2D.isKinematic = true;
            isPlayerFall = false; //уже не падаем
            this.hitObject = hit.transform.gameObject; //объект на который нажали
            hitJumpPoint = hitObject.GetComponent<JumpPoint>();
        }
        else{
            this.hitObject = null;
        }
    }

    void PlayerFall()
    {//падение игрока
        isPlayerFall = true;
        rig2D.isKinematic = false;
        StartCoroutine(Fall());
    }

    IEnumerator Lerp()
    {   
        Vector2 _from = transform.position;
        Vector2 _to = hitObject.transform.position;
        float _t = 0f;
        while (_t < 1){
            _t += 0.05f;
            _to = hitObject.transform.position;
            transform.position = Vector2.Lerp(_from, _to, _t); //перемещаем тело в позицию объекта, на который нажали
            yield return null;
        }    
        transform.parent = hitObject.transform;
        idLine = hitJumpPoint.Line;
        idCollumn = hitJumpPoint.Collumn;
        LerpCoroutine = null;
    }

    IEnumerator Fall()//пока падаем, отслеживаем нажатие на кнопку мыши и целимся в ближайший пушер
    {
        GameObject[] _pushers = GameObject.FindGameObjectsWithTag("Pusher"); //берём все созданные на данный момент пушеры
        float _minDist=100f; //немного чисел с неба
        float _dist;//дистанция до ближайшего пушера
        while(isPlayerFall) //пока мы падаем
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
                    }
                }
                rig2D.isKinematic = true;
                isPlayerFall = false; //уже не падаем
                StartCoroutine(Lerp()); //перемещаемся к спасительному пушеру
            }
            yield return null;
       }
    }
}
