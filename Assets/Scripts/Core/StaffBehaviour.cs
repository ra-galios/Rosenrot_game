using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffBehaviour : MonoBehaviour
{

    private LineRenderer lineRend;
    private PlayerBehaviour player;

    public GameObject staffHead;
    public Vector2 handleStaffOffset;
    [Range(0f, 1f)]
    public float staffLerpSpeed = 1f;

    public Vector3 staffHeadOffset = new Vector3(-0.4f, 1.6f, 0f);

    // Use this for initialization
    void OnEnable()
    {
        player = GameController.Instance.playerBeh;
        lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //ElasticStaff();

    }

    private void ElasticStaff()
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
        while(Vector2.Distance(staffHead.transform.position, hitObject.transform.position + staffHeadOffset) > 0.1f)
        {
            staffHead.transform.position = Vector2.Lerp(staffHead.transform.position, hitObject.transform.position + staffHeadOffset, staffLerpSpeed);
            ElasticStaff();
            yield return null;
        }
        StartCoroutine(StayAtHitObj(hitObject));
        StopCoroutine("MoveStaff");
    }

    public IEnumerator StayAtHitObj(GameObject hitObject)
    {
        while (true)
        {
            staffHead.transform.position = hitObject.transform.position + staffHeadOffset;

            yield return null;
            ElasticStaff();
        }
    }
}
