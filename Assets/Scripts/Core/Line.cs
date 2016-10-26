using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
public class Line
{
    public GameObject ParentGO;
    public List<GameObject> Pushers = new List<GameObject>();
}
