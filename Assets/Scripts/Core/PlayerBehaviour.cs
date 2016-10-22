using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    public static bool isFalling;

    bool isWaitInput;//не используется, походу
    int nextPusher;

    Transform TargetTransform;
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
        if (Input.GetMouseButtonDown(0))
        {
            hitObject = null;
            hitObject = SetHitObject();
        }
    }

    void OnEnable () {
        GameInput.Instance.PlayerInputAction += JumpToNext; 
	}
	

	void OnDisable () {
        GameInput.Instance.PlayerInputAction -= JumpToNext;
    }

    void JumpToNext(GameInput.PlayerAction action)
    {
        if (hitObject)
        {
            float dist = Mathf.Abs(transform.position.x - hitObject.transform.position.x); // дистанция от игрока до hitObject'a
            print(dist);                                                                   //bool inAction = false;

            if (dist < 0.3f && action == GameInput.PlayerAction.climb){
                Lerp();
            }

            if (dist <= 1.8f && dist >= 0.3f && action == GameInput.PlayerAction.jump){
                Lerp();
            }

            if (dist > 2.5f && action == GameInput.PlayerAction.doubleJump){
                Lerp();
            }
        }
    }

    GameObject SetHitObject(){
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null){
            print("vozvratTransform");
            return hit.transform.gameObject;
        }
        else{
            print("vozvratNULL");
            return null;
        }
    }

    void Lerp()
    {
        TargetTransform = hitObject.transform;
        Vector2 newPosition = TargetTransform.position;
        transform.position = Vector2.Lerp(transform.position, newPosition, 1);
    }

    //int SetNormal(Transform parent)
    //{
    //    isFalling = false;
    //    GetComponent<Rigidbody2D>().isKinematic = true;
    //    //transform.parent = parent;
    //    TargetTransform = parent;
    //    print(parent);
    //    //нужно пробежать по всем пушерам и найти номер parent.gameobject

    //    GameObject obj = parent.gameObject;
    //    int i = 0;
    //    if (obj)
    //    {
    //        i = LevelGenerator2d.PushersInScene.IndexOf(obj);
    //        i++;
    //    }
    //    return i;
    //}
}
