using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<JumpPoint> pushers = new List<JumpPoint>();        //варианты пушеров. (!) Добавляются из префабов

    [SerializeField]
    private Transform[] startPositions;                             //позиции в которых создавать новые пушеры

    [SerializeField]
    private List<JumpPoint> altPushers = new List<JumpPoint>();     //альтернативные пушеры. (!) Добавляются со сцены

    //[SerializeField]
    //private List<JumpPoint> staticPushers = new List<JumpPoint>();  //начальные пушеры, которые должны быть при старте игры

    [SerializeField]
    private int maxLines = 25;                 //кол-во генерируемых линий

    [SerializeField]
    private int maxItemsInLine = 3;            //максимальное кол-во пушеров на линии

    [SerializeField]
    private float timeGenerationLines = 2f;   //скорость создания линий

    [SerializeField]
    private float speedPusher = 1f;            //скорость пушера. Пушеры узнают текущую скорость

    private float baseTimeGenerationLines=2f; //начальная время генерации линий
    private float baseSpeedPusher=1f;         //начальная скорость пушеров

    private int currentLinesCount;            //текущее кол-во созданных линий
    private bool _typePush;                   //тип пушера: рандомный или альтернативный
    private float _timeStartLevel;            //время запуска левела
    private bool _isRunLevel = false;
    private float timeAcceleration=5f;

    public static LevelGenerator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void FixedUpdate()
    {
        if (IsRunLevel)
        {
            SpeedPusher = Mathf.Clamp(SpeedPusher + baseSpeedPusher * (0.2f / 60) * Time.deltaTime, 0, 6);
            TimeGenerationLine = Mathf.Clamp(TimeGenerationLine - baseTimeGenerationLines * (0.1f / 60) * Time.deltaTime, 0.5f, 6);
        }
    }

    public void StartLevel()
    {
        _timeStartLevel = Time.time;
        StartCoroutine(GeneratorLines());//запускаем генератор линий
        IsRunLevel = true;
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
        int _idLine = 5;        //id родителя пушера
        Transform _randomPos;   //рандомная позиция
        List<Transform> _curPos = new List<Transform>();    //список возможных позиций
        JumpPoint _newPush;    //пушер
        GameObject _go;         //линия для пушеров
        int _idCurPos = 0;      //позиция для нового пушера

        while (CurrentLinesCount < MaxLines)        //пока не создадим нужное кол-во линий
        {
            List<int> _colUsedInLines = new List<int>();

            var _countPushersInLine = Random.Range(1, MaxItemsInLine + 1);   //кол-во пушеров на линии
            _go = new GameObject();                 //новая линия, будет родителем пушера/ов
            _go.name = _idLine.ToString();          //даём ему имя
            
            foreach (var element in startPositions)
            {                   //формируем список возможных позиций
                _curPos.Add(element);
            }

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

               foreach(int col in _colUsedInLines) //новая позиция не должна совпасть с предыдущей, чтобы два пушера не создались друг в друге
                {
                    while (col == _idCurPos)
                    {
                        _idCurPos = Random.Range(0, startPositions.Length);//тут затык
                    }
                }
                _colUsedInLines.Add(_idCurPos);

                _randomPos = startPositions[_idCurPos];
                _newPush.transform.position = _randomPos.position;          //ставим в позицию

                if (_typePush)//если это альтернативный пушер, то просто становится в нужную позицию, иначе - инстантируется на сцену
                    _obj = _newPush.gameObject; //альтернативный
                else
                    _obj = Instantiate(_newPush.gameObject, _newPush.transform.position, _newPush.transform.rotation) as GameObject; //рандомный

                if (!_obj.activeSelf) _obj.SetActive(true);

                _jp = _obj.GetComponent<JumpPoint>();
                _jp.Line = _idLine;              //задаём пушеру линию, на которой он находится
                _jp.Collumn = _idCurPos;         //задаём пушеру колонку в которой он находится
                _jp.Speed = speedPusher;         //задаём скорость пушера
                _obj.transform.parent = _go.transform;                      //делаем новый пушер "ребёнком" нового родителя
                //_curPos.RemoveAt(_idCurPos);                                //удаляем из списка позиций, чтобы не создавать два объекта в одном месте

                _obj = null;
                _newPush = null;
                _jp = null;
            }

            _colUsedInLines = null;
            _go = null;
            currentLinesCount++;
            _idLine++;              //прикидываем имя для следующего родителя
            _curPos.Clear();        //очщаем список возможных позиций
            yield return new WaitForSeconds(TimeGenerationLine);           //ждём сек. тут регулируем скорость создания линий
        }
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
