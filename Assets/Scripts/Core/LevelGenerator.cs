﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    //################# Из редактора ########################
    [Header("Варианты скал для подтягиваний: ")]
    [SerializeField]
    private JumpPoint[] m_Array_Climb;
    [Header("Варианты скал для прыжков: ")]
    [SerializeField]
    private JumpPoint[] m_Array_Jump;
    [Header("Варианты скал для двайных прыжков: ")]
    [SerializeField]
    private JumpPoint[] m_Array_DoubleJump;
    [Header("Варианты скал с вопросами: ")]
    [SerializeField]
    private JumpPoint[] m_Array_JumpPoint_Question;
    [Header("Декорации: ")]
    [SerializeField]
    private GameObject[] m_Decorations;
    [Header("Возможные бонусы на скалах")]
    [SerializeField]
    private CollectableGO[] m_BonusPrefabs;

    [Space]

    [Header("Позиции для генерирования скал: ")]
    [SerializeField]
    private Transform[] m_StartPositions;                             //позиции в которых создавать новые скалы
    [Header("Максимальное кол-во скал на линии: ")]
    [SerializeField]
    private int m_MaxItemsInLine;            //максимальное кол-во скал на линии
    [Header("Интервал между линиями по Y: ")]
    [SerializeField]
    private float lineSpacing;
    [Header("Скорость движения скал: ")]
    [SerializeField]
    private float m_SpeedJumpPoint = 1f;            //скорость скалы
    [Header("Ускорение движения скал: ")]
    [SerializeField]
    private float accelerationJumpPoint = 0f;         //ускорение скал


    [Space(30), Header("Debug")]

    [Header("Скала, после которой нужно запустить генератор: ")]
    [SerializeField]
    private GameObject m_LastRock;
    [Header("Позиция последней скалы: ")]
    [SerializeField]
    private int posPrevJumpPoint; //позиция предыдущей скалы
    [Header("Кол-во скал на последней линии: ")]
    [SerializeField]
    private int prevCountJumpPoint; //кол-во скал на предыдущей линии
    [Header("Максимальное количество линий из скал: ")]
    [SerializeField]
    private int m_MaxLines;                 //кол-во генерируемых линий


    //################################################################

    private int currentLinesCount;            //текущее кол-во созданных линий
    private bool isRunLevel = false;          //запущен ли левел       
    private int idLine;        //id линии родителя скалы
    private GameObject lastLinePusher;     //пушер на последней построеной линии
    private int m_LastRockId;
    private GameObject decorParent;
    private GameObject pushersParent;

    void Reset()
    {
        isRunLevel = false;

        m_MaxLines = 25;
        m_MaxItemsInLine = 3;
        lineSpacing = 3f;
        m_SpeedJumpPoint = 1f;

        idLine = 0;
        accelerationJumpPoint = 0f;
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
        m_MaxLines = CurrentLinesInScene();
        currentLinesCount = m_MaxLines;//колво уже созданных линий на сцене
        m_LastRockId = m_MaxLines;
        idLine = currentLinesCount + 1;
        lastLinePusher = m_LastRock;

        decorParent = new GameObject();
        decorParent.name = "DecorParent";
        pushersParent = new GameObject();
        pushersParent.name = "PushersParent";
    }

    void FixedUpdate()
    {
        if (IsRunLevel) //ускорение уровня
        {
            m_SpeedJumpPoint = Mathf.Clamp(m_SpeedJumpPoint + accelerationJumpPoint * 0.00001f, 0, 6);
        }
    }

    public void StartLevel()
    {
        if (Market.Instance.Health > 0)//если есть жизни, то можно играть
        {
            isRunLevel = true;
            Market.Instance.Health--; //отнимаем одну использованную жизнь, т.к. запустили левел
            int playLevelTimes = ++GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].playLevelTimes;
            int neverGiveUpVal;
            if ((neverGiveUpVal = AchievementsController.GetAchievement(AchievementsController.Type.NeverGiveUp)) < playLevelTimes)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.NeverGiveUp, playLevelTimes - neverGiveUpVal);
            }
            if (Market.Instance.Health == 0)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.HeartBreaker, 1);
            }
            StartCoroutine("GeneratorLines");
        }
    }

    public void StopLevel()
    {
        StopCoroutine("GeneratorLines");//останавливаем
        isRunLevel = false;
    }


    private void CreateDecorations(bool[] SetCol)
    {
        int decorationsCount = 0;

        for (int i = 0; i < SetCol.Length; i++)
        {
            if (!SetCol[i])
            {
                GameObject decoration = Instantiate(m_Decorations[Random.Range(0, m_Decorations.Length)]);
                decoration.transform.position = m_StartPositions[i].position + RandomizePos(0.3f, 0.7f) ;
                decoration.transform.rotation = Quaternion.identity;
                decoration.transform.parent = decorParent.transform;
                decorationsCount++;
            }

            if(decorationsCount > 0 && Random.Range(0f, 1f) > 0.3f)
            {
                break;
            }
        }
    }

    IEnumerator GeneratorLines()
    {                           //Генератор линий и объектов на них
        GameObject objNewRock = null; //здесь будет наш новый пушер
        JumpPoint jumpPointRock = null;   //компонент JumpPoint объекта _obj
        Vector3 pusherPos;   //рандомная позиция
        JumpPoint newRock;    //пушер
        GameObject parentLine;         //линия для пушеров
        int posNewRock = 0;      //позиция для нового пушера

        while (!GameController.Instance.OnBonusLevel)
        {
            yield return null;
        }

        while (GameController.Instance.OnBonusLevel)        //пока не создадим нужное кол-во линий
        {
            while ((transform.position.y - lastLinePusher.transform.position.y) < lineSpacing)       //ждем пeред тем как построить новую линию
            {
                yield return null;
            }

            bool[] SetCol = new bool[m_StartPositions.Length]; ; //флаги занятых колонок, для избежания создания пушеров в одной колонке
            int countPushersInLine = Random.Range(1, MaxItemsInLine + 1);   //кол-во пушеров на линии
            //countPushersInLine = Mathf.Clamp(countPushersInLine, 1, Market.Instance.Seeds);

            parentLine = new GameObject();                 //новая линия, будет родителем пушера/ов
            parentLine.transform.parent = pushersParent.transform;
            parentLine.name = idLine.ToString();          //даём ему имя

            for (int _pushId = 0; _pushId < countPushersInLine; _pushId++)  //создаем необходимое кол-во пушеров на линию
            {
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

                JumpPoint[] tempArray = identifyTypeOfJumpPoint(countPushersInLine, posNewRock);        //определяем тип пушера

                newRock = tempArray[Random.Range(0, tempArray.Length)];     //выбираем внешний вид пушера

                prevCountJumpPoint = countPushersInLine;

                pusherPos = m_StartPositions[posNewRock].position;      //выбираем позицию

                pusherPos += RandomizePos(0.25f, 0.25f);       //немного изменяем позицию

                objNewRock = Instantiate(newRock.gameObject, pusherPos, newRock.transform.rotation) as GameObject; //рандомный
                lastLinePusher = objNewRock;

                if (!objNewRock.activeSelf)
                    objNewRock.SetActive(true);

                jumpPointRock = objNewRock.GetComponent<JumpPoint>();
                jumpPointRock.Line = idLine;              //задаём пушеру линию, на которой он находится
                jumpPointRock.Collumn = posNewRock;         //задаём пушеру колонку в которой он находится
                jumpPointRock.PrefBonus = GetBonus();       //задаём пушеру бонус
                posPrevJumpPoint = posNewRock;              //запоминаем где создали
                jumpPointRock.Speed = m_SpeedJumpPoint;         //задаём скорость пушера
                objNewRock.transform.parent = parentLine.transform;                      //делаем новый пушер "ребёнком" нового родителя

                newRock = null;
                jumpPointRock = null;
                tempArray = null;

                yield return null;
            }

            CreateDecorations(SetCol);

            parentLine = null;
            currentLinesCount++;
            idLine++;              //прикидываем имя для следующего родителя

            yield return null;
        }

    }

    private int CurrentLinesInScene()
    {
        int maxIdLine = 0;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Pusher");

        foreach (GameObject item in objs)
        {
            var line = item.GetComponent<JumpPoint>().Line;
            if (maxIdLine < line)
            {
                maxIdLine = line;
                m_LastRock = item;
            }
        }

        return maxIdLine;
    }

    private JumpPoint[] identifyTypeOfJumpPoint(int countPushersInLine, int posNewRock)
    {
        JumpPoint[] tempArray = m_Array_JumpPoint_Question;

        if (prevCountJumpPoint == 1 && countPushersInLine == 1)
        {
            if (posNewRock == 0)
            {
                if (posPrevJumpPoint == 0)
                    tempArray = m_Array_Climb;


                if (posPrevJumpPoint == 1)
                    tempArray = m_Array_Jump;

                if (posPrevJumpPoint == 2)
                    tempArray = m_Array_DoubleJump;
            }
            //-----------
            if (posNewRock == 1)
            {
                if (posPrevJumpPoint == 0)
                    tempArray = m_Array_Jump;

                if (posPrevJumpPoint == 1)
                    tempArray = m_Array_Climb;

                if (posPrevJumpPoint == 2)
                    tempArray = m_Array_Jump;
            }
            //-----------
            if (posNewRock == 2)
            {
                if (posPrevJumpPoint == 0)
                    tempArray = m_Array_DoubleJump;

                if (posPrevJumpPoint == 1)
                    tempArray = m_Array_Jump;

                if (posPrevJumpPoint == 2)
                    tempArray = m_Array_Climb;
            }
        }

        return tempArray;
    }

    private Vector3 RandomizePos(float xGap, float yGap)
    {
        float x = Random.Range(xGap, -xGap);
        float y = Random.Range(yGap, -yGap);

        return new Vector3(x, y, 0f);
    }

    private CollectableGO GetBonus()
    {
        float chance = Random.Range(0f, 1f);
        if (chance < 0.3f)
            return m_BonusPrefabs[0];
        else if (chance < 0.4f)
            return m_BonusPrefabs[1];
        else
            return null;
    }

    //Свойства

    public int MaxLines
    {
        get { return m_MaxLines; }
        set { m_MaxLines = value; }
    }

    public int LastRockId
    {
        get { return m_LastRockId; }
        set { m_LastRockId = value; }
    }

    public int MaxItemsInLine
    {
        get { return m_MaxItemsInLine; }
        set { m_MaxItemsInLine = value; }
    }

    public bool IsRunLevel
    {
        get { return isRunLevel; }
    }

    public float SpeedPusher
    {
        get { return this.m_SpeedJumpPoint; }
        set { this.m_SpeedJumpPoint = value; }
    }
}
