using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    public int maxLines=25; //кол-во генерируемых линий
    public int itemsInLine=3; //кол-во пушеров на линии
    public List<JumpPoint> Pushers = new List<JumpPoint>(); //варианты пушеров

    private List<Line> Lines = new List<Line>(); //лист созданных линий
    private int CurrentLinesCount; //текущее кол-во созданных линий


	// Use this for initialization
	void Awake () {
        StartCoroutine(GeneratorLines());//запускаем генератор линий
	}

    IEnumerator GeneratorLines() 
    {//генератор линий и объектов на них
        Line _line = new Line();//линия
        GameObject _obj = null;//здесь будет наш новый пушер
        int idLine=0;//часть имени для родителя пушера
        while (this.CurrentLinesCount < this.maxLines)
        {//пока не создадим нужное кол-во линий
            JumpPoint _newPush = Pushers[Random.Range(0, Pushers.Count)];//новый пушер

            if (_newPush.TimeCreated_s == 0)
            {//если время для пушера не указано
                _obj = Instantiate(_newPush.pusher, _newPush.pusher.transform.position, _newPush.pusher.transform.rotation) as GameObject;//добавляем на сцену
            }
            else
            {//если время указано
                if (_newPush.TimeCreated_s == Time.time)
                {//и если оно совпадает с текущим после старта сцены
                    _obj = Instantiate(_newPush.pusher, _newPush.pusher.transform.position, _newPush.pusher.transform.rotation) as GameObject;//добавляем на сцену
                }
            }

            if (_obj)
            {//если пушер создан
                _line.Pushers.Add(_obj);//закидываем его в линию
                GameObject GO = new GameObject();//создаём пустой ГО, будет родителем пушера/ов
                GO.name = "Line" + idLine; //даём ему имя
                _obj.transform.parent = GO.transform; //делаем новый пушер "ребёнком" нового родителя
                _line.ParentGO = GO; //запоминаем "родителя"
                Lines.Add(_line); //запоминаем всю новую "семью" (родитель GO и его ребёнок _obj)
                _line = new Line();//создаём новую линию

                this.CurrentLinesCount++;//+1 в кол-во созданных линий
                idLine++;//прикидываем имя для следующего родителя
            }

            yield return new WaitForSeconds(2f);//ждём пару сек
        }
    }
}
