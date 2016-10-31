using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class LevelGenerator : MonoBehaviour {
    [SerializeField]
    private List<GameObject> pushers = new List<GameObject>(); //варианты пушеров
    [SerializeField]
    private Transform[] startPositions;

    private int maxLines=25; //кол-во генерируемых линий
    private int maxItemsInLine=3; //максимальное кол-во пушеров на линии
    private float speedGenerationLines=1f;
    private int currentLinesCount; //текущее кол-во созданных линий

    //свойства
    public List<GameObject> Pushers
    {
        get { return this.pushers; }
        set { this.pushers = value; }
    }
    public int MaxLines
    {
        get { return this.maxLines; }
        set { this.maxLines = value; }
    }
    public int MaxItemsInLine
    {
        get { return this.maxItemsInLine; }
        set { this.maxItemsInLine = value; }
    }
    public float SpeedGeneration
    {
        get { return this.speedGenerationLines; }
        set { this.speedGenerationLines = value; }
    }

    void Awake ()
    {
        StartCoroutine(GeneratorLines());//запускаем генератор линий
    }

    IEnumerator GeneratorLines()
    {//генератор линий и объектов на них
        GameObject _obj = null;//здесь будет наш новый пушер
        int _idLine = 1;//id родителя пушера
        Transform _randomPos; //рандомная позиция
        List<Transform> _curPos = new List<Transform>();//список возможных позиций
        GameObject _newPush;//пушер
        GameObject _go;//линия для пушеров
        int _idCurPos = 0;//позиция для нового пушера

        while (this.currentLinesCount < this.MaxLines)//пока не создадим нужное кол-во линий
        {
            var _pusherCountInLine = Random.Range(1, MaxItemsInLine + 1); //кол-во пушеров на линии
            _go = new GameObject();//новая линия, будет родителем пушера/ов
            _go.name = _idLine.ToString(); //даём ему имя
            
            foreach (var element in startPositions)
            {//формируем список возможных позиций для удобства
                _curPos.Add(element);
            }

            for (int _pushId = 0; _pushId < _pusherCountInLine; _pushId++)
            {
                _newPush = Pushers[Random.Range(0, Pushers.Count)];//новый рандомный пушер

                //диз сможет создать на сцене готовый блок пушеров и указать когда данный блок должен будет появиться
                //{...}

                _idCurPos = Random.Range(0, _curPos.Count);
                _randomPos = _curPos[_idCurPos];//получаем рандомную позицию из списка возможных
                _newPush.transform.position = _randomPos.position;//ставим в позицию

                _obj = Instantiate(_newPush, _newPush.transform.position, _newPush.transform.rotation) as GameObject;//добавляем на сцену
                _obj.GetComponent<JumpPoint>().Line = _idLine; //задаём пушеру линию, на которой он находится
                _obj.transform.parent = _go.transform;//делаем новый пушер "ребёнком" нового родителя
                _curPos.RemoveAt(_idCurPos);//удаляем из списка позиций, чтобы не создавать два объекта в одном месте
                _newPush = null;
            }

            _go = null;
            currentLinesCount++;
            _idLine++;//прикидываем имя для следующего родителя
            _curPos.Clear();//очщаем список возможных позиций
            yield return new WaitForSeconds(SpeedGeneration);//ждём сек. тут регулируем скорость создания линий
        }
    }

}
