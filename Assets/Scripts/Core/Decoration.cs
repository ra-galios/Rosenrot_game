using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour {
    private float speed; //скорость 

    // Update is called once per frame
    void Update () {
        if (LevelGenerator.Instance.IsRunLevel)
        {
            MovePusher();
        }
    }

    void MovePusher()
    {
        speed = LevelGenerator.Instance.SpeedPusher;
        this.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
