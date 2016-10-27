using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    public int maxLines=25; //кол-во генерируемых линий
    public int maxItemsInLine=3; //максимальное кол-во пушеров на линии
    public List<JumpPoint> pushers = new List<JumpPoint>(); //варианты пушеров
    public List<JumpPoint> alternatePushers = new List<JumpPoint>();
    public Transform[] startPositions;
    public float speedGenerationLines=1f;
    public List<GameObject> lines = new List<GameObject>();

    private int currentLinesCount; //текущее кол-во созданных линий
    private List<GameObject> Turn = new List<GameObject>();

	// Use this for initialization
	void Awake () {
        StartCoroutine(GeneratorLines());//запускаем генератор линий
    }

    void Update()
    {
        for(int i=0; i < lines.Count; i++)
        {
            lines[i].transform.Translate(Vector3.down*0.03f);
        }
    }

    IEnumerator GeneratorLines()
    {//генератор линий и объектов на них
        GameObject _obj = null;//здесь будет наш новый пушер
        int _idLine=0;//id родителя пушера
        Transform _randomPos; //рандомная позиция
        List<Transform> _curPos = new List<Transform>();//список возможных позиций
        JumpPoint _newPush;//пушер
        GameObject _go;//линия для пушеров
        int _idCurPos = 0;

        while (this.currentLinesCount < this.maxLines)//пока не создадим нужное кол-во линий
        {
            var _pusherCountInLine = Random.Range(1, maxItemsInLine + 1); //кол-во пушеров на линии
            _go = new GameObject();//новая линия, будет родителем пушера/ов
            _go.name = _idLine.ToString(); //даём ему имя
            _idLine++;//прикидываем имя для следующего родителя

            foreach (var element in startPositions)
            {//формируем список возможных позиций для удобства
                _curPos.Add(element);
            }

            for (int _pushId = 0; _pushId < _pusherCountInLine; _pushId++)
            {
                _newPush = pushers[Random.Range(0, pushers.Count)];//новый рандомный пушер

                for (int i = 0; i < alternatePushers.Count; i++)//смотрим все альтернативные пушеры, может какой-то сейчас должен быть создан
                {
                    if ((int)alternatePushers[i].TimeCreated_s == (int)Time.time)//проверяем параметры пушера и время
                    {
                        _newPush = alternatePushers[i];//Пушер, который нужно создать вовремя
                        _newPush.isRandomPos = false; //будет стоять в своей позоции
                        _newPush.Pusher.transform.position = _curPos[_idCurPos].position;//ставим в стартовую позицию.
                        _pushId = _pusherCountInLine;
                    }
                }

                if (_newPush.isRandomPos)//поставить ли пушер в рандомную позицию
                {
                    _idCurPos = Random.Range(0, _curPos.Count);
                    _randomPos = _curPos[_idCurPos];//получаем рандомную позицию из списка возможных
                    _newPush.Pusher.transform.position = _randomPos.position;//ставим в позицию
                }

                _obj = Instantiate(_newPush.Pusher, _newPush.Pusher.transform.position, _newPush.Pusher.transform.rotation) as GameObject;//добавляем на сцену
                _obj.transform.parent = _go.transform;//делаем новый пушер "ребёнком" нового родителя
                _curPos.RemoveAt(_idCurPos);//удаляем из списка позиций, чтобы не создавать два объекта в одном месте
                _newPush = null;
            }
            
            _curPos.Clear();//очщаем список возможных позиций
            lines.Add(_go);//добавляем в список линий (дижутся в апдейте)
            yield return new WaitForSeconds(speedGenerationLines);//ждём сек. тут регулируем скорость создания линий
        }
    }
}
