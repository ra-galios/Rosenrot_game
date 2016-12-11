using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [SerializeField]
    private List<JumpPoint> m_Pushers = new List<JumpPoint>();        //варианты пушеров. (!) Добавляются из префабов

    [SerializeField]
    private Transform[] m_StartPositions;                             //позиции в которых создавать новые пушеры

    [SerializeField]
    private List<JumpPoint> m_AltPushers = new List<JumpPoint>();     //альтернативные пушеры. (!) Добавляются со сцены. Рекомендация: добавить пуш на сцену, указать ему время, line и collumn не указывать

    [SerializeField]
    private int m_MaxLines = 25;                 //кол-во генерируемых линий

    [SerializeField]
    private int m_MaxItemsInLine = 3;            //максимальное кол-во пушеров на линии

    [SerializeField]
    private float m_TimeGenerationLines = 2f;   //интервал создания линий

    [SerializeField]
    private float m_SpeedPusher = 1f;            //скорость пушера. Пушеры узнают текущую скорость

    [SerializeField]
    private PlayerBehaviour player;

    private float baseTimeGenerationLines=2f; //начальная время генерации линий
    private float baseSpeedPusher=1f;         //начальная скорость пушеров
    private int currentLinesCount;            //текущее кол-во созданных линий
    private bool typePush;                   //тип пушера: рандомный или альтернативный
    private float timeStartLevel;            //время запуска левела
    private bool isRunLevel = false;          //запущен ли левел       
    private int idLine=0;        //id родителя пушера
    private int linesInScene;   //кол-во линий со статическими пошерами

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        linesInScene = CurrentLinesInScene();//колво уже созданных линий на сцене
    }

    void FixedUpdate()
    {
        if (IsRunLevel) //ускорение уровня
        {
            SpeedPusher = Mathf.Clamp(SpeedPusher + baseSpeedPusher * (0.2f / 5) * Time.deltaTime, 0, 6);
            m_TimeGenerationLines = Mathf.Clamp(m_TimeGenerationLines - baseTimeGenerationLines * (0.2f / 5) * Time.deltaTime, 0.5f, 6);
        }
    }

    public void StartLevel()
    {
        if (Market.Instance.Health > 0)//если есть жизни, то можно играть
        {
            timeStartLevel = Time.time; //время старта
            IsRunLevel = true;
            Market.Instance.Health-=1; //отнимаем одну использованную жизнь, т.к. запустили левел
            Market.Instance.SetCurrentDatePlayer(); //записываем новую дату обновления жизней, через 5 минут будет +1
            //StartCoroutine("GeneratorLines");
        }
    }

    public void StopLevel()
    {
        timeStartLevel = 0;
        //StopCoroutine("GeneratorLines");//останавливаем
        IsRunLevel = false;
    }

    IEnumerator GeneratorLines()
    {                           //Генератор линий и объектов на них
        GameObject _obj = null; //здесь будет наш новый пушер
        JumpPoint _jp = null;   //компонент JumpPoint объекта _obj
        Transform _randomPos;   //рандомная позиция
        JumpPoint _newPush;    //пушер
        GameObject _go;         //линия для пушеров
        int _idCurPos = 0;      //позиция для нового пушера

        idLine = linesInScene+1;
        CurrentLinesCount = linesInScene+1;
        while (CurrentLinesCount <= MaxLines)        //пока не создадим нужное кол-во линий
        {
            bool[] SetCol = new bool[m_StartPositions.Length]; ; //флаги занятых колонок, для избежания создания пушеров в одной колонке

            var _countPushersInLine = Random.Range(1, MaxItemsInLine + 1);   //кол-во пушеров на линии
            _go = new GameObject();                 //новая линия, будет родителем пушера/ов
            _go.name = idLine.ToString();          //даём ему имя

            for (int _pushId = 0; _pushId < _countPushersInLine; _pushId++)  //создаем необходимое кол-во пушеров на линию
            {
                _newPush = Pushers[Random.Range(0, Pushers.Count)];         //новый рандомный пушер
                typePush = false;

                if (AltPushers.Count > 0)
                {
                    foreach (JumpPoint push in AltPushers)
                    {
                        if (push != null)
                        {
                            if ((Time.time - timeStartLevel) >= push.TimeCreate)   //пора ставить пушер с установленным временем
                            {
                                _newPush = push;
                                AltPushers.RemoveAt(AltPushers.LastIndexOf(push));
                                typePush = true;                //указываем что это альтернативный пушер
                                break;
                            }
                        }
                    }
                }

                _idCurPos = Random.Range(0, m_StartPositions.Length);     //получаем рандомную позицию из списка возможных

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
                        _idCurPos = Random.Range(0, m_StartPositions.Length); //иначе берём новую
                    }
                }

                _randomPos = m_StartPositions[_idCurPos];
                _newPush.transform.position = _randomPos.position;          //ставим в позицию

                if (typePush)//если это альтернативный пушер, то просто становится в нужную позицию, иначе - инстантируется на сцену
                    _obj = _newPush.gameObject; //альтернативный
                else
                {
                    var randomPos = RandomPos();

                    _obj = Instantiate(_newPush.gameObject, new Vector2(_newPush.transform.position.x + randomPos.x, _newPush.transform.position.y + randomPos.y), _newPush.transform.rotation) as GameObject; //рандомный
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
            yield return new WaitForSeconds(TimeGenerationLines);           //ждём сек. тут регулируем скорость создания линий
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
        get { return m_Pushers; }
        set { m_Pushers = value; }
    }
    public List<JumpPoint> AltPushers
    {
        get { return this.m_AltPushers; }
        set { this.m_AltPushers = value; }
    }
    public int MaxLines
    {
        get { return m_MaxLines; }
        set { m_MaxLines = value; }
    }
    public int MaxItemsInLine
    {
        get { return m_MaxItemsInLine; }
        set { m_MaxItemsInLine = value; }
    }
    public float TimeGenerationLines
    {
        get { return m_TimeGenerationLines; }
        set { m_TimeGenerationLines = value; }
    }
    public int CurrentLinesCount
    {
        get { return currentLinesCount; }
        set { currentLinesCount = value; }
    }
    public bool IsRunLevel
    {
        get { return isRunLevel; }
        set { isRunLevel = value; }
    }
    public float SpeedPusher
    {
        get { return this.m_SpeedPusher; }
        set { this.m_SpeedPusher = value; }
    }
}
