using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffBehaviour : MonoBehaviour
{

    private LineRenderer lineRend;
    [HideInInspector]
    public Coroutine moveCoroutine;

    public GameObject staffHead;
    public Vector3 handleStaffOffset;       //положение в руке средней части
    [Range(0f, 1f)]
    public float staffLerpSpeed = 1f;

    public Vector3 staffTargetOffset = new Vector3(-0.4f, 1.6f, 0f);       //сдвиг относительно камня
    public Vector3 defaultStaffPosition = new Vector3();        //положение верхней части

    // Use this for initialization
    void OnEnable()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine("InDefaultPosition");
    }

    private void ElasticStaff()
    {
        lineRend.SetPosition(0, staffHead.transform.position);      //позиция верхней точки палки

        Vector3 stuffNormal = (lineRend.GetPosition(0) - lineRend.GetPosition(1)).normalized;

        float angle = Mathf.Atan2(stuffNormal.x, stuffNormal.y);
        angle = Mathf.Rad2Deg * angle;

        staffHead.transform.rotation = Quaternion.Euler(0f, 0f, -angle - 90f);

        lineRend.SetPosition(1, GameController.Instance.playerBeh.transform.position + handleStaffOffset);  //позиция средней точки

        stuffNormal = (lineRend.GetPosition(0) - lineRend.GetPosition(1)).normalized;

        lineRend.SetPosition(2, lineRend.GetPosition(1) - stuffNormal * .7f);       //позиция нижней точки
    }

    public IEnumerator MoveStaff(GameObject hitObject)
    {
        while(Vector2.Distance(staffHead.transform.position, hitObject.transform.position + staffTargetOffset) > 0.1f)
        {
            staffHead.transform.position = Vector3.Lerp(staffHead.transform.position, hitObject.transform.position + staffTargetOffset, staffLerpSpeed);
            ElasticStaff();
            yield return null;
        }
        StartCoroutine(StayAtHitObj(hitObject));
        moveCoroutine = null;
        StopCoroutine("MoveStaff");
    }

    public IEnumerator StayAtHitObj(GameObject hitObject)
    {
        while (Vector2.Distance(staffHead.transform.position, lineRend.GetPosition(1)) > 0.8f)
        {
            staffHead.transform.position = hitObject.transform.position + staffTargetOffset;
            ElasticStaff();
            yield return null;
        }
        StartCoroutine("ToDefaultPosition");
        StopCoroutine("StayAtHitObj");
    }

    public IEnumerator ToDefaultPosition()
    {
        while(Vector2.Distance(staffHead.transform.position, GameController.Instance.playerBeh.transform.position + defaultStaffPosition) > 0.2f)
        {
            staffHead.transform.position = Vector3.Lerp(staffHead.transform.position, GameController.Instance.playerBeh.transform.position + defaultStaffPosition, .1f);
            ElasticStaff();
            yield return null;
        }
        StartCoroutine("InDefaultPosition");
        StopCoroutine("ToDefaultPosition");
    }

    public IEnumerator InDefaultPosition()
    {
        while(moveCoroutine == null)
        {
            staffHead.transform.position = GameController.Instance.playerBeh.transform.position + defaultStaffPosition;
            ElasticStaff();
            yield return null;
        }
        StopCoroutine("InDefaultPosition");
    }
}
