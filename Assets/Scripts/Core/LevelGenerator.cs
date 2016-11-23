using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [SerializeField]
    private List<JumpPoint> pushers = new List<JumpPoint>();        //варианты пушеров. (!) Добавляются из префабов

    [SerializeField]
    private Transform[] startPositions;                             //позиции в которых создавать новые пушеры

    [SerializeField]
    private List<JumpPoint> altPushers = new List<JumpPoint>();     //альтернативные пушеры. (!) Добавляются со сцены. Рекомендация: добавить пуш на сцену, указать ему время, line и collumn не указывать

    [SerializeField]
    private int maxLines = 25;                 //кол-во генерируемых линий

    [SerializeField]
    private int maxItemsInLine = 3;            //максимальное кол-во пушеров на линии

    [SerializeField]
    private float timeGenerationLines = 2f;   //интервал создания линий

    [SerializeField]
    private float speedPusher = 1f;            //скорость пушера. Пушеры узнают текущую скорость

    private float _baseTimeGenerationLines=2f; //начальная время генерации линий
    private float _baseSpeedPusher=1f;         //начальная скорость пушеров
    private int currentLinesCount;            //текущее кол-во созданных линий
    private bool _typePush;                   //тип пушера: рандомный или альтернативный
    private float _timeStartLevel;            //время запуска левела
    private bool _isRunLevel = false;          //запущен ли левел       
    private int idLine=0;        //id родителя пушера - ИСПРАВИТЬ НА НАХОЖДЕНИЕ ПОСЛЕДНЕГО АЙДИ НА СЦЕНЕ В СТАТИЧЕСКИХ ПУШЕРАХ

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void FixedUpdate()
    {
        if (IsRunLevel) //ускорение уровня
        {
            SpeedPusher = Mathf.Clamp(SpeedPusher + _baseSpeedPusher * (0.2f / 5) * Time.deltaTime, 0, 6);
            TimeGenerationLine = Mathf.Clamp(TimeGenerationLine - _baseTimeGenerationLines * (0.2f / 5) * Time.deltaTime, 0.5f, 6);
        }
    }

    public void StartLevel()
    {
        if (Market.Instance.Health > 0)//если есть жизни, то можно играть
        {
            _timeStartLevel = Time.time; //время старта
            StartCoroutine(GeneratorLines());//запускаем генератор линий
            IsRunLevel = true;
            Market.Instance.Health-=1; //отнимаем одну использованную жизнь, т.к. запустили левел
            Market.Instance.SetCurrentDatePlayer(); //записываем новую дату обновления жизней, через 5 минут будет +1
        }
    }

    public void StopLevel()
    {
        _timeStartLevel = 0;
        StopCoroutine(GeneratorLines());//останавливаем
        IsRunLevel = false;
    }

    IEnumerator GeneratorLines()
    {                                                       //Генератор линий и объектов на них
        GameObject _obj = null; //здесь будет наш новый пушер
        JumpPoint _jp = null;   //компонент JumpPoint объекта _obj
        Transform _randomPos;   //рандомная позиция
        JumpPoint _newPush;    //пушер
        GameObject _go;         //линия для пушеров
        int _idCurPos = 0;      //позиция для нового пушера

        idLine = CurrentLinesInScene() + 1;
        CurrentLinesCount = idLine;
        while (CurrentLinesCount <= MaxLines)        //пока не создадим нужное кол-во линий
        {
            bool[] SetCol = new bool[startPositions.Length]; ; //флаги занятых колонок, для избежания создания пушеров в одной колонке

            var _countPushersInLine = Random.Range(1, MaxItemsInLine + 1);   //кол-во пушеров на линии
            _go = new GameObject();                 //новая линия, будет родителем пушера/ов
            _go.name = idLine.ToString();          //даём ему имя

            for (int _pushId = 0; _pushId < _countPushersInLine; _pushId++)  //создаем необходимое кол-во пушеров на линию
            {
                _newPush = Pushers[Random.Range(0, Pushers.Count)];         //новый рандомный пушер
                _typePush = false;

                if (AltPushers.Count > 0)
                {
                    foreach (JumpPoint push in AltPushers)
                    {
                        if (push != null)
                        {
                            if ((Time.time - _timeStartLevel) >= push.TimeCreate)   //пора ставить пушер с установленным временем
                            {
                                _newPush = push;
                                AltPushers.RemoveAt(AltPushers.LastIndexOf(push));
                                _typePush = true;                //указываем что это альтернативный пушер
                                break;
                            }
                        }
                    }
                }

                _idCurPos = Random.Range(0, startPositions.Length);     //получаем рандомную позицию из списка возможных

                //смотриим чтобы эта позиция не совпала с другим пушером
                var isSetPos = false; 
                while (!isSetPos)
                {
                    if (!SetCol[_idCurPos])
                    {
                        isSetPos = true;
                        SetCol[_idCurPos] = true;
                        break;
                    }
                    else
                    {
                        _idCurPos = Random.Range(0, startPositions.Length); //иначе берём новую
                    }
                }

                _randomPos = startPositions[_idCurPos];
                _newPush.transform.position = _randomPos.position;          //ставим в позицию

                if (_typePush)//если это альтернативный пушер, то просто становится в нужную позицию, иначе - инстантируется на сцену
                    _obj = _newPush.gameObject; //альтернативный
                else
                {
                    var randomPos = RandomPos();

                    _obj = Instantiate(_newPush.gameObject, new Vector2(_newPush.transform.position.x + randomPos.x,_newPush.transform.position.y + randomPos.y) , _newPush.transform.rotation) as GameObject; //рандомный
                }

                if (!_obj.activeSelf) _obj.SetActive(true);

                _jp = _obj.GetComponent<JumpPoint>();
                _jp.Line = idLine;              //задаём пушеру линию, на которой он находится
                _jp.Collumn = _idCurPos;         //задаём пушеру колонку в которой он находится
                _jp.Speed = SpeedPusher;         //задаём скорость пушера
                _obj.transform.parent = _go.transform;                      //делаем новый пушер "ребёнком" нового родителя

                _obj = null;
                _newPush = null;
                _jp = null;
            }

            _go = null;
            currentLinesCount++;
            idLine++;              //прикидываем имя для следующего родителя
            yield return new WaitForSeconds(TimeGenerationLine);           //ждём сек. тут регулируем скорость создания линий
        }
    }

    private int CurrentLinesInScene()
    {
        int maxIdLine = 0;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Pusher");

        foreach (GameObject item in objs)
        {
            var line = item.GetComponent<JumpPoint>().Line;
            maxIdLine = maxIdLine < line ? line : maxIdLine;
        }

        return maxIdLine;
    }

    private Vector2 RandomPos()
    {
        float x = Random.Range(-0.30f, 0.31f);
        float y = Random.Range(-0.30f, 0.31f);

        return new Vector2(x, y);
    }

    //Свойства
    public List<JumpPoint> Pushers
    {
        get { return pushers; }
        set { pushers = value; }
    }
    public List<JumpPoint> AltPushers
    {
        get { return this.altPushers; }
        set { this.altPushers = value; }
    }
    public int MaxLines
    {
        get { return maxLines; }
        set { maxLines = value; }
    }
    public int MaxItemsInLine
    {
        get { return maxItemsInLine; }
        set { maxItemsInLine = value; }
    }
    public float TimeGenerationLine
    {
        get { return timeGenerationLines; }
        set { timeGenerationLines = value; }
    }
    public int CurrentLinesCount
    {
        get { return currentLinesCount; }
        set { currentLinesCount = value; }
    }
    public bool IsRunLevel
    {
        get { return _isRunLevel; }
        set { _isRunLevel = value; }
    }
    public float SpeedPusher
    {
        get { return this.speedPusher; }
        set { this.speedPusher = value; }
    }
}
