using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffBehaviour : MonoBehaviour {

    private LineRenderer lineRend;
    private PlayerBehaviour player;

    public GameObject staffHead;
    public Vector2 handleStaffOffset;
    [Range(0f, 1f)]
    public float staffLerpSpeed = 1f;

    // Use this for initialization
    void Start () {
        player = GameController.Instance.playerBeh;
        lineRend = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        EasticStaff();

    }

    private void EasticStaff()
    {
        lineRend.SetPosition(0, staffHead.transform.position);      //позиция верхней точки палки

        Vector3 stuffNormal = (lineRend.GetPosition(0) - lineRend.GetPosition(1)).normalized;

        float angle = Mathf.Atan2(stuffNormal.x, stuffNormal.y);
        angle = Mathf.Rad2Deg * angle;

        staffHead.transform.rotation = Quaternion.Euler(0f, 0f, -angle - 90f);

        lineRend.SetPosition(1, new Vector2(GameController.Instance.playerBeh.transform.position.x, GameController.Instance.playerBeh.transform.position.y) + handleStaffOffset);  //позиция средней точки

        lineRend.SetPosition(2, lineRend.GetPosition(1) - stuffNormal * .7f);       //позиция нижней точки
    }

    public IEnumerator MoveStaff(GameObject hitObject)
    {
        float t = staffLerpSpeed;

        while(t < 1f)
        {
            staffHead.transform.position = Vector2.Lerp(staffHead.transform.position, hitObject.transform.position, t);
            yield return null;
        }

    }
}
