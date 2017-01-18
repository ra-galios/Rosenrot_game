using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator playerAnimator;

    // Use this for initialization
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    } 

	public void SetJump(GameInput.PlayerAction action)
    {
        if(action == GameInput.PlayerAction.climb)
        {
            playerAnimator.SetTrigger("Jump");
        }
        else if(action == GameInput.PlayerAction.jump)
        {
            playerAnimator.SetTrigger("Jump");
        }
        else
        {
            playerAnimator.SetTrigger("DoubleJump");
        }
    }

    public void SetFall(bool isFall)
    {
        playerAnimator.SetBool("IsFall", isFall);
    }
}
