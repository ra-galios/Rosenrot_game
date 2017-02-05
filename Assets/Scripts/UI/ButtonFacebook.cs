using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFacebook : MonoBehaviour
{

    public void OpenPage(string url)
    {
        AchievementsController.AddToAchievement(AchievementsController.Type.BeOurGuest, 1);
        if (url != "")
        {
            Application.OpenURL(url);
        }
    }
}
