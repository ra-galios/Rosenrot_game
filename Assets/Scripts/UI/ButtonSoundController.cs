using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour
{
    public AudioSource m_AudioSource;

    public AudioClip m_AcceptClip;
    public AudioClip m_DenyClip;

    // Use this for initialization
    {
        if (thisButton.interactable)
        {
            m_AudioSource.clip = m_AcceptClip;
            m_AudioSource.Play();
        }
        else
        {
            m_AudioSource.clip = m_DenyClip;
            m_AudioSource.Play();
        }
    }

}
