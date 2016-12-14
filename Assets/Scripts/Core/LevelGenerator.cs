using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    //[SerializeField]
    //private List<JumpPoint> m_Pushers = new List<JumpPoint>();        //варианты пушеров. (!) Добавляются из префабов

    //################# Из редактора ########################
    [Header("Варианты скал для подтягиваний/прыжков/двойных прыжков: ")]
    [SerializeField] private JumpPoint[] m_Array_JumpPoint;
    [Header("Варианты скал с вопросами: (!пока не работает!) ")]
    [SerializeField] private JumpPoint[] m_Array_JumpPoint_Question;
    [Header("Скалы, которые должны появится по истечению времени: ")]
    [SerializeField] private List<JumpPoint> m_AltPushers;     //альтернативные скалы. (!) Добавляются со сцены. Рекомендация: добавить скалу на сцену, указать ей время, line и collumn не указывать
    [Space]
    [Header("Позиции для генерирования скал: ")]
    [SerializeField] private Transform[] m_StartPositions;                             //позиции в которых создавать новые скалы
    [Header("Максимальное количество линий из скал: ")]
    [SerializeField] private int m_MaxLines;                 //кол-во генерируемых линий
    [Header("Максимальное кол-во скал на линии: ")]
    [SerializeField] private int m_MaxItemsInLine;            //максимальное кол-во скал на линии
    [Header("Интервал создания линий: ")]
    [SerializeField] private float m_TimeGenerationLines;   //интервал создания линий
    [Header("Скорость движения скал: ")]
    [SerializeField] private float m_SpeedPusher;            //скорость скалы
    //################################################################


    private List<int[]> createdRocks;

    private float baseTimeGenerationLines; //начальное время генерации линий
    private float baseSpeedPusher;         //начальная скорость скал
    private int currentLinesCount;            //текущее кол-во созданных линий
    private bool typePush;                   //тип скалы: рандомная или альтернативная
    private float timeStartLevel;            //время запуска левела
    private bool isRunLevel;          //запущен ли левел       
    private int idLine;        //id линии родителя скалы
    //private int linesInScene;   //кол-во линий со статическими пошерами

    private int posPrevJumpPoint; //позиция предыдущей скалы


    void Reset()
    {
        isRunLevel = false;

        m_MaxLines = 25;
        m_MaxItemsInLine = 3;
        m_TimeGenerationLines = 2f;
        m_SpeedPusher = 1f;

        createdRocks = new List<int[]>();
        idLine = 0;
        baseSpeedPusher = 1f;
        baseTimeGenerationLines = 2f;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentLinesCount = CurrentLinesInScene();//колво уже созданных линий на сцене
        idLine = currentLinesCount+1;
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
        GameObject objNewRock = null; //здесь будет наш новый пушер
        JumpPoint jumpPointRock = null;   //компонент JumpPoint объекта _obj
        Transform randomPos;   //рандомная позиция
        JumpPoint newRock;    //пушер
        GameObject parentLine;         //линия для пушеров
        int posNewRock = 0;      //позиция для нового пушера

        while (currentLinesCount <= m_MaxLines)        //пока не создадим нужное кол-во линий
        {
            bool[] SetCol = new bool[m_StartPositions.Length]; ; //флаги занятых колонок, для избежания создания пушеров в одной колонке

            int countPushersInLine = Random.Range(1, MaxItemsInLine + 1);   //кол-во пушеров на линии

            /*    
             * Если позиции предыдущих скал были по краям, а сейчас нужно создать одину скалу в центре, то эта скала будет для прыжка, иначе с вопросом
             * 
             * Нужно создать несколько "конвееров" для каждой колонки и вызывать в них метод создания скалы.
            */


            parentLine = new GameObject();                 //новая линия, будет родителем пушера/ов
            parentLine.name = idLine.ToString();          //даём ему имя

            for (int _pushId = 0; _pushId < countPushersInLine; _pushId++)  //создаем необходимое кол-во пушеров на линию
            {
                newRock = m_Array_JumpPoint[Random.Range(0, m_Array_JumpPoint.Length)];         //новый рандомный пушер
                typePush = false;

                if (m_AltPushers.Count > 0)
                {
                    foreach (JumpPoint push in m_AltPushers)
                    {
                        if (push != null)
                        {
                            if ((Time.time - timeStartLevel) >= push.TimeCreate)   //пора ставить пушер с установленным временем
                            {
                                newRock = push;
                                m_AltPushers.RemoveAt(m_AltPushers.LastIndexOf(push));
                                typePush = true;                //указываем что это альтернативный пушер
                                break;
                            }
                        }
                    }
                }

                posNewRock = Random.Range(0, m_StartPositions.Length);     //получаем рандомную позицию из списка возможных

                //смотриим чтобы эта позиция не совпала с другим пушером
                var isSetPos = false;
                while (!isSetPos)
                {
                    if (!SetCol[posNewRock])
                    {
                        isSetPos = true;
                        SetCol[posNewRock] = true;
                        break;
                    }
                    else
                    {
                        posNewRock = Random.Range(0, m_StartPositions.Length); //иначе берём новую
                    }
                }

                randomPos = m_StartPositions[posNewRock];
                newRock.transform.position = randomPos.position;          //ставим в позицию

                if (typePush)//если это альтернативный пушер, то просто становится в нужную позицию, иначе - инстантируется на сцену
                    objNewRock = newRock.gameObject; //альтернативный
                else
                {
                    Vector2 newRandomPos = RandomPos();
                    objNewRock = Instantiate(newRock.gameObject, new Vector2(newRock.transform.position.x + newRandomPos.x, newRock.transform.position.y + newRandomPos.y), newRock.transform.rotation) as GameObject; //рандомный
                }

                if (!objNewRock.activeSelf) objNewRock.SetActive(true);

                jumpPointRock = objNewRock.GetComponent<JumpPoint>();
                jumpPointRock.Line = idLine;              //задаём пушеру линию, на которой он находится
                jumpPointRock.Collumn = posNewRock;         //задаём пушеру колонку в которой он находится
                jumpPointRock.Speed = SpeedPusher;         //задаём скорость пушера
                objNewRock.transform.parent = parentLine.transform;                      //делаем новый пушер "ребёнком" нового родителя

                objNewRock = null;
                newRock = null;
                jumpPointRock = null;
            }

            parentLine = null;
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

    private void WhatTheRockCreate(int currentLinesCount)
    {
        Transform prevLine = GameObject.Find(currentLinesCount.ToString()).transform;

        if (prevLine.childCount > 1)
        {

        }
    }


    private Vector2 RandomPos()
    {
        float x = Random.Range(-0.30f, 0.31f);
        float y = Random.Range(-0.30f, 0.31f);

        return new Vector2(x, y);
    }

    //Свойства
    //public List<JumpPoint> Pushers
    //{
    //    get { return m_Pushers; }
    //    set { m_Pushers = value; }
    //}

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
