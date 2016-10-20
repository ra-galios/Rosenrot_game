using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    bool isWaitInput;
    Transform TargetTransform;
    public static bool isFalling;
    int nextPusher;
    GameObject hitObject;


    void Start()
    {
        isFalling = false;
        TargetTransform = transform;
        nextPusher = 0;
        hitObject = null;
    }
    void Update()
    {
        
    }
        void OnEnable () {
        GameInput.Instance.PlayerInputAction += PlayerMove; 
	}
	

	void OnDisable () {
        GameInput.Instance.PlayerInputAction -= PlayerMove;
    }

    void PlayerMove(GameInput.PlayerAction action)
    {
        
        print(action);
        if (action == GameInput.PlayerAction.jump)
        {
            JumpToNext(action);

            hitObject = null;
            hitObject = SetHitObject();

        }

    //GameObject newObj = SetHitObject();
    //nextPusher = SetNormal(newObj.transform);
}
    void JumpToNext(GameInput.PlayerAction action)
    {

        if (hitObject)
        {

            print("zashloJumpToNext");
            float dist = Mathf.Abs(transform.position.x - hitObject.transform.position.x); // дистанция от игрока до hitObject'a
        print(dist);                                                                       //bool inAction = false;

        TargetTransform = hitObject.transform;
        Vector2 newPosition = TargetTransform.position;
                    transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * 5f);



        }
    }

    GameObject SetHitObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            return hit.transform.gameObject;
            print("vozvratTransform");
        }
        else
        {
            print("vozvratNULL");
            return null;
        }
    }

    int SetNormal(Transform parent)
    {
        isFalling = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        //transform.parent = parent;
        TargetTransform = parent;
        print(parent);
        //нужно проюежать по всем пушерам и найти номер parent.gameobject

        GameObject obj = parent.gameObject;
        int i = 0;
        if (obj)
        {
            i = LevelGenerator2d.PushersInScene.IndexOf(obj);
            i++;
        }
        return i;
    }
}
