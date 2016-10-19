using UnityEngine;
using System.Collections;
using System;

public class GameInput : CreateSingletonGameObject<GameInput> {
    public enum PlayerAction { climb, jump, doubleJump };
    Vector2 StartMousePosition;
    public Action<PlayerAction> PlayerInputAction;
    bool isWaitInput;
    float firstClickTime = 0;
    float secondClickTime = 0;
    public static int PlayerLifes;
    public static bool isFalling;
    GameObject hitObject;
    // Use this for initialization
    void Start () {
        isFalling = false;
        hitObject = null;
        PlayerLifes = Calculator.GetPlayerLifes();
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    if (PlayerInputAction != null)
        //    {
        //        PlayerInputAction.Invoke(PlayerAction.climb);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    if (PlayerInputAction != null)
        //    {
        //        PlayerInputAction.Invoke(PlayerAction.jump);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    if (PlayerInputAction != null)
        //    {
        //        PlayerInputAction.Invoke(PlayerAction.doubleJump);
        //    }
        //}
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

    }
    IEnumerator WaitInput()
    {
        isWaitInput = true;
        yield return new WaitForSeconds(0.35f);
        PlayerAction action = PlayerAction.climb;
        if (secondClickTime > 0)
        {
            action = PlayerAction.jump;
        }
        else if (Vector2.Distance(StartMousePosition, Input.mousePosition) > 50f && secondClickTime == 0)
        {
            action = PlayerAction.doubleJump;
        }

        secondClickTime = 0;

   
        isWaitInput = false;
    }
    GameObject SetHitObject()
    {


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    void SetFalling()
    {

        isFalling = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        transform.parent = null;
    }
}
