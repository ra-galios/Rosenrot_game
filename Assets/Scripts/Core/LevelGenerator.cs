using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private List<JumpPoint> pushers = new List<JumpPoint>();   //варианты пушеров
    [SerializeField]
    private Transform[] startPositions;                          //позиции в которых создавать новые пушеры
    [SerializeField]
    private List<JumpPoint> altPushers = new List<JumpPoint>();   //варианты пушеров

    private static int maxLines=25;                 //кол-во генерируемых линий
    private static int maxItemsInLine=3;            //максимальное кол-во пушеров на линии
    private static float speedGenerationLines=1f;   //скорость создания линий
    private static int currentLinesCount;           //текущее кол-во созданных линий
    private static float speedPusher=2f;            //скорость пушера
    private static bool isCoroutineRun=false;       //флаг работы корутины
    private static float timeCreateAltPusher=0;     //время создания последнего алтернативного пушера

    public static LevelGenerator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(GeneratorLines());//запускаем генератор линий
        }
    }

    void Update() { }

    IEnumerator GeneratorLines()
    {                                                       //Генератор линий и объектов на них
        GameObject _obj = null; //здесь будет наш новый пушер
        int _idLine = 1;        //id родителя пушера
        Transform _randomPos;   //рандомная позиция
        List<Transform> _curPos = new List<Transform>();    //список возможных позиций
        JumpPoint _newPush;    //пушер
        GameObject _go;         //линия для пушеров
        int _idCurPos = 0;      //позиция для нового пушера

        while (CurrentLinesCount < MaxLines)        //пока не создадим нужное кол-во линий
        {
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

                foreach (JumpPoint push in AltPushers)
                {
                    if (push!=null)
                    {
                        if (Time.time >= (push.TimeLastCreate + push.TimeCreate)) //при первом обращении к timeLastCreate его значение равно примерно 20.1002... 
                        {
                            _newPush = push;

                            if (push.Repeat)
                            {
                                push.TimeLastCreate = (int)Time.time;
                            }
                            else
                            {
                                AltPushers.RemoveAt(AltPushers.LastIndexOf(push));
                                break;
                            }
                        }
                    }
                    
                }

                _idCurPos = Random.Range(0, _curPos.Count);     //получаем рандомную позицию из списка возможных
                _randomPos = _curPos[_idCurPos];    
                _newPush.transform.position = _randomPos.position;          //ставим в позицию

                _obj = Instantiate(_newPush.gameObject, _newPush.transform.position, _newPush.transform.rotation) as GameObject;   //добавляем на сцену
                
                _obj.GetComponent<JumpPoint>().Line = _idLine;              //задаём пушеру линию, на которой он находится
                _obj.GetComponent<JumpPoint>().Speed = speedPusher;         //задаём скорость пушера
                _obj.transform.parent = _go.transform;                      //делаем новый пушер "ребёнком" нового родителя
                _curPos.RemoveAt(_idCurPos);                                //удаляем из списка позиций, чтобы не создавать два объекта в одном месте
                _newPush = null;
            }

            _go = null;
            currentLinesCount++;
            _idLine++;              //прикидываем имя для следующего родителя
            _curPos.Clear();        //очщаем список возможных позиций
            yield return new WaitForSeconds(SpeedGeneration);           //ждём сек. тут регулируем скорость создания линий
        }
    }

    //private JumpPoint NewPusher()
    //{

    //}

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
    public float SpeedGeneration
    {
        get { return speedGenerationLines; }
        set { speedGenerationLines = value; }
    }
    public int CurrentLinesCount
    {
        get { return currentLinesCount; }
        set { currentLinesCount = value; }
    }
}
