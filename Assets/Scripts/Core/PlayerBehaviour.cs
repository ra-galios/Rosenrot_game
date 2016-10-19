using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    bool isWaitInput;


    void OnEnable () {
        GameInput.Instance.PlayerInputAction += PlayerMove; 
	}
	

	void OnDisable () {
        GameInput.Instance.PlayerInputAction -= PlayerMove;
    }

    void PlayerMove(GameInput.PlayerAction action)
    {
       //if (action != null)
       // {
            JumpToNext(action);
        //}
    }
    void JumpToNext(GameInput.PlayerAction action)
    {

    }

    

}
