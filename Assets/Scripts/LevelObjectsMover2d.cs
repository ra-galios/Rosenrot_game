using UnityEngine;
using System.Collections;

public class LevelObjectsMover2d :  MonoBehaviour
{
	public GameObject ClimbObj;
    public GameObject JumpObj;
    public GameObject DoubleJumpObj;
    public GameObject ClimbHold;
    public GameObject JumpHold;
    public GameObject DoubleJumpHold;
    enum ControlType {climb, jump, doubleJump };
    ControlType type;

    //float MotionSpeed;
    // Use this for initialization
    void Start()
    {
        //MotionSpeed = LevelGenerator2d.speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * LevelGenerator2d.speed);
        if (transform.position.y < -10)
            Destroy(this.gameObject);

    }

    //public void SetMotionSpeed(float AddSpeed)
    //{
    //    MotionSpeed += AddSpeed;
    //}

    public void SetPushgentype(Vector3 PrevPosition)
    {
        float dist = Mathf.Abs(transform.position.x - PrevPosition.x);
        if(dist < 1f)
        {
            enableObject(ClimbObj);
            type = ControlType.climb;
        }else if(dist <= LevelGenerator2d.FarDistance * 0.6f && dist >= 1f)
        {
            enableObject(JumpObj);
            type = ControlType.jump;
        }
        else if(dist > LevelGenerator2d.FarDistance * 0.6f)
        {
            enableObject(DoubleJumpObj);
            type = ControlType.doubleJump;
        }

    }

    void enableObject(GameObject obj)
    {
       
        if (obj == ClimbObj)
        {
            ClimbObj.SetActive(true);
            ClimbHold.SetActive(true);
        }
        else {
            ClimbObj.SetActive(false);
            ClimbHold.SetActive(false);
        }

        if (obj == JumpObj)
        {
            JumpObj.SetActive(true);
            JumpHold.SetActive(true);
        }
        else {
            JumpObj.SetActive(false);
            JumpHold.SetActive(false);
        }

        if (obj == DoubleJumpObj)
        {
            DoubleJumpObj.SetActive(true);
            DoubleJumpHold.SetActive(true);
        }
        else { DoubleJumpObj.SetActive(false);
            DoubleJumpHold.SetActive(false);
        }
    }

    public string getType()
    {
        return type.ToString();
    }
}
