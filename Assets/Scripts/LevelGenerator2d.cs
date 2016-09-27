using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator2d : MonoBehaviour {
    public int MaxPushersInLevel = 25;
    int GeneratedPushers;

    public Transform[] Position;
    public GameObject[] Pushers;
    public float MinGenerationSpeed = 0.3f;
    float dif = 2f;
    public static float speed = 0;
    public static List<GameObject> PushersInScene = new List<GameObject>();
    public static float FarDistance;
    public static List<PushgenBlock> PushgenBlocks;
    public static List<GameObject> AlternativePushers;   

    public float startSpeed = 0.75f;

    public int Seed;
    float StartTime;

    Coroutine generatorCoroutine = null;

	// Use this for initialization
	void Awake () {
        PushgenBlocks = new List<PushgenBlock>();
        StartTime = Time.time;
        speed = startSpeed;
        PushersInScene = new List<GameObject>();
        
        FarDistance = Position[0].position.x - Position[1].position.x;
        foreach (Transform a in Position)
        {
            foreach (Transform b in Position)
            {
                if (a.position.x - b.position.x > FarDistance)
                    FarDistance = a.position.x - b.position.x;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if(generatorCoroutine == null && GameController.inGame)
        {
            generatorCoroutine = StartCoroutine(Generator());
        }
            


        if (GeneratedPushers >= MaxPushersInLevel)
        {
            this.gameObject.SetActive(false);
        } 
	}


    GameObject getBlockPusgen()
    {
        GameObject obj = null;
        foreach (PushgenBlock block in PushgenBlocks)
        {
            if (block && Time.time - StartTime >= block.PushgenStartTime)//какой конкретно блок проверяем
            {                
                if (block.transform.childCount > 0)
                {
                    //пушер, следующий в иерархии блока
                    obj = block.transform.GetChild(0).gameObject;

                    //проверяем следующего пушера на предмет вилки - по позиции
                    AlternativePushersSorting.AddToAlternativePushersList(GetAlternativePushers(block));
                    SetPusherStartPosition(obj);         
                    
                }
                else
                {
                    Destroy(block.transform.gameObject);
                }

                break;
            }
        }
        return obj; 
    }

    IEnumerator Generator()
    {
        GameObject obj = getBlockPusgen();
        if (!obj)
        {
            Random.seed = Seed + (int)(Time.time - StartTime);
            GameObject newObject = Pushers[Random.Range(0, Pushers.Length)];
            Transform newTransform = Position[Random.Range(0, Position.Length)];
            Random.seed = 0;
            obj = Instantiate(newObject, newTransform.position, newTransform.rotation) as GameObject;
        }


        if (PushersInScene.Count > 0)
        {
            obj.GetComponent<LevelObjectsMover2d>().SetPushgentype(PushersInScene[PushersInScene.Count - 1].transform.position);
        }
        else
        {
            obj.GetComponent<LevelObjectsMover2d>().SetPushgentype(Vector3.zero);
        }


        PushersInScene.Add(obj);
        GeneratedPushers++;

        yield return new WaitForSeconds(dif);
        dif -= 0.03f;
        speed += 0.03f;
        dif = Mathf.Clamp(dif, MinGenerationSpeed, dif);
        yield return null;
        StartCoroutine(Generator());
        yield break;
    }

    List<GameObject> GetAlternativePushers(PushgenBlock block)
    {
        List<GameObject> pushers = new List<GameObject>();
        if (block.transform.childCount > 1)//проверяем возможность альтернативных пушеров
        {
            GameObject secondPusher = null;
            GameObject thirdPusher = null;
            float posA = block.transform.GetChild(0).position.y;
            float posB = block.transform.GetChild(1).position.y;
            if (Mathf.Abs(posA - posB) < 0.5f)//проверяем первого возможного альтернативного пушера
            {
                secondPusher = block.transform.GetChild(1).gameObject;

                if (block.transform.childCount > 2)//проверяем второго возможного альтернативного пушера
                {
                    float posC = block.transform.GetChild(2).position.y;

                    if (Mathf.Abs(posB - posC) < 0.5f)
                        thirdPusher = block.transform.GetChild(2).gameObject;
                }
            }
            if (secondPusher)
                pushers.Add(secondPusher);

            if (thirdPusher)
                pushers.Add(thirdPusher);        
        }

        if (pushers.Count > 0)
        {
            foreach(GameObject push in pushers)
            {
                SetPusherStartPosition(push);
            }
            return pushers;
        }
            
        else
            return null;

    }

    void SetPusherStartPosition(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        pos.y = Position[0].position.y;
        obj.transform.position = pos;
        obj.transform.parent = null;
    }
}
