using UnityEngine;
using System.Collections;

public class CollectableGO : MonoBehaviour
{

    private bool collected = false;

    virtual public void EnterBonus()
    {
        collected = true;
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Collect", true);
        Destroy(this.gameObject, 1f);
    } //взять колектблз

    virtual public void Save() { } //сохранить собранные колектблзы ... язык сломаешь :(
}