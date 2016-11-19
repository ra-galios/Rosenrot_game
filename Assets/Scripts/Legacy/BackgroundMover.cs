using UnityEngine;
using System.Collections;

public class BackgroundMover : MonoBehaviour {


    public float FlowSpeed = 2f;
    //float offsetX;
    float offsetY;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offsetY += Time.deltaTime * LevelGenerator2d.speed / transform.localScale.y;
        if (offsetY > 1)
        {
            //offsetX = 0.5f * (Random.Range(0, 2));
            offsetY = 0;
            
        }
           

        GetComponent<Renderer>().material.SetTextureOffset
            ("_MainTex", new Vector2(0, offsetY));
    }
    }
