using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
public class Line
{
    private GameObject parentGO;
    private List<GameObject> pushers = new List<GameObject>();

    public GameObject ParentGO {
        get { return this.parentGO; }
        set { this.parentGO = value; }
    }
    public List<GameObject> Pushers
    {
        get { return pushers; }
        set { this.pushers = value; }
    }

}
