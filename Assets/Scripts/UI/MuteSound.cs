using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteSound : MonoBehaviour
{
    
     
     public enum Trigger
    {
        OnClick
    }

    public Trigger trigger = Trigger.OnClick;
    private bool muted = false;

    public void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick)
        {
            if (!muted)
            {
                AudioListener.volume = 0.0f;
                muted = true;
            }
            else
            {
                AudioListener.volume = 1.0f;
                muted = false;
            }
        }
    }
}