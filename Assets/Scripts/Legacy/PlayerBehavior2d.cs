using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public  class PlayerBehavior2d : MonoBehaviour {
    Transform TargetTransform;
        int nextPusher;
    float startPress;
    Vector2 StartMousePosition;
    enum ControlType {climb, jump, doubleJump};
    GameObject hitObject;
    float firstClickTime = 0;
    float secondClickTime = 0;
    bool isJump;
    public Text text;

    public static bool isFalling;
    bool isWaitInput;

    public static int PlayerLifes;
   // Animator anim;
   

    // Use this for initialization
    void Start () {
       //PlayerPrefs.SetInt("PlayerLifes", 0);
        PlayerLifes = Calculator.GetPlayerLifes();
        TargetTransform = transform;
        nextPusher = 0;
        hitObject = null;
        isFalling = false;
       // anim = GetComponent<Animator>();
     //   anim.SetBool("start", false);
    }
	
	// Update is called once per frame
	void Update () {
        //text.text = PlayerLifes.ToString();
        if (Input.GetMouseButtonDown(0) && PlayerLifes > 0)
        {
            //if (!GameController.inGame)
                GameController.inGame = true;
         //   anim.SetBool("start", true);
            if (!isFalling)
            {
                hitObject = null;
                hitObject = SetHitObject();
                if (!hitObject)
                {
                    SetFalling();
                }
            }
            else
            {
                GameObject newObj = SetHitObject();
                if (newObj && newObj.transform.position.y < transform.position.y)
                {
                    nextPusher = SetNormal(newObj.transform);
                }
            }



            if (!isWaitInput)
            {
                firstClickTime = Time.time;
                StartMousePosition = Input.mousePosition;
                StartCoroutine(WaitInput());


            }
            else
            {
                if (secondClickTime == 0)
                {
                    secondClickTime = Time.time;
                }
                else
                {
                    StopCoroutine(WaitInput());
                    SetFalling();
                }
            }

        }


        if (transform.position.y < -4f)
        {
            LevelGenerator2d.AlternativePushers = null;
            AlternativePushersSorting.AlternativePushersList = new List<List<GameObject>>();
            Calculator.SetPlayerLifes(-1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!isFalling)
        {
            Vector2 newPosition = TargetTransform.position;
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * 5f); // лерпаем на JumpPoint
        }

    }

    IEnumerator WaitInput()//ControlType type)
    {
        isWaitInput = true;
        yield return new WaitForSeconds(0.35f);
        ControlType type = ControlType.climb;
        if(secondClickTime > 0)
        {
            type = ControlType.jump;
        }
        else if (Vector2.Distance(StartMousePosition, Input.mousePosition) > 50f && secondClickTime == 0)
        {
            type = ControlType.doubleJump;
        }

        secondClickTime = 0;

        JumpToNext(type);
        isWaitInput = false;
        print(type);
    }


    void JumpToNext(ControlType type)
    {
        if (hitObject)
        {
            //print(hitObject.GetComponent<LevelObjectsMover2d>().getType());
            if ( hitObject.tag == "Respawn" && CheckNextPushgen())
            {
                bool inAction = false;
                float dist = Mathf.Abs(transform.position.x - hitObject.transform.position.x);
                switch (type)
                {
                    case ControlType.climb:
                        if (dist < 1f)
                            inAction = true;
                        break;
                    case ControlType.jump:                        
                        if (dist <= LevelGenerator2d.FarDistance * 0.6f && dist >= 1f)
                            inAction = true;
                        break;

                    case ControlType.doubleJump:
                        if(dist > LevelGenerator2d.FarDistance * 0.6f)
                            inAction = true;
                        break;
                    default:
                        break;
                        
                }
                
                if (inAction)
                {
                    TargetTransform = hitObject.transform;
                    nextPusher++;
                }
                else
                {
                    SetFalling();
                }

            }
            else if(hitObject.tag == "Respawn")
            {
                SetFalling();
            }
        }
    }

    GameObject SetHitObject()
    {
       

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            print("vozvratTransform");
            return hit.transform.gameObject;
        }
        else
        {
            print("vozvratNULL");
            return null;
        }
    }

    void SetFalling()
    {
        
        isFalling = true;
        
        GetComponent<Rigidbody2D>().isKinematic = false;
       
        transform.parent = null;
    }

    int SetNormal(Transform parent)
    {
        isFalling = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        //transform.parent = parent;
        TargetTransform = parent;
        print(parent);
        //нужно проюежать по всем пушерам и найти номер parent.gameobject
        
        GameObject obj =  parent.gameObject;
        int i=0;
        if (obj)
        {
            i = LevelGenerator2d.PushersInScene.IndexOf(obj);
            i++;
         }
        return i;       
    }

    bool CheckNextPushgen()
    {
        if(hitObject == LevelGenerator2d.PushersInScene[nextPusher])
        {
            return true;
        }
        else
        {
            if (LevelGenerator2d.AlternativePushers != null)
            {
                float checkPos = LevelGenerator2d.AlternativePushers[0].transform.position.y;
                if (Mathf.Abs(checkPos - hitObject.transform.position.y) < 0.5f)
                {
                    foreach (GameObject obj in LevelGenerator2d.AlternativePushers)
                    {
                        if (hitObject == obj)
                        {
                            LevelGenerator2d.AlternativePushers = null;
                            return true;
                        }
                    }
                }

 
            }

            return false;
        }
        
    }

}
