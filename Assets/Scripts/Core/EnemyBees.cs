using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBees : Enemy
{
    [SerializeField]
    private float m_Speed = 1f;
    [SerializeField]
    private Vector3[] m_Waypoints;
    [SerializeField]
    private float m_WaitTime = 0.5f;
    [SerializeField, RangeAttribute(1, 3)]
    private float m_Smooth = 0.5f;

    private int m_IndexCounter;
    private Vector2 m_Indexes;
    private float m_PercentBetweenWaypoints;
    private int m_NumberOfPoints;
    private float m_DistanceBetweenPoints;
    private float m_nextMoveTime;

#if UNITY_EDITOR
    private Vector3[] m_GlobalPositions;

    private void Start()
    {
        m_GlobalPositions = new Vector3[m_Waypoints.Length];
        for (int i = 0; i < m_Waypoints.Length; i++)
        {
            m_GlobalPositions[i] = m_Waypoints[i] + transform.parent.position;
        }
#endif
        m_IndexCounter = 0;
        m_Indexes = new Vector2(m_IndexCounter, m_IndexCounter + 1);
        m_NumberOfPoints = m_Waypoints.Length;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (Time.time < m_nextMoveTime)
        {
            return;
        }

        if (m_NumberOfPoints > 1)
        {
            m_DistanceBetweenPoints = Vector2.Distance(m_Waypoints[(int)m_Indexes.x], m_Waypoints[(int)m_Indexes.y]);
            m_PercentBetweenWaypoints += Time.deltaTime * m_Speed / m_DistanceBetweenPoints;
            m_PercentBetweenWaypoints = Mathf.Clamp01(m_PercentBetweenWaypoints);
            float SmoothPercentBetweenWaypoints = Smooth(m_PercentBetweenWaypoints);

            transform.localPosition = Vector3.Lerp(m_Waypoints[(int)m_Indexes.x], m_Waypoints[(int)m_Indexes.y], SmoothPercentBetweenWaypoints);

            if (m_PercentBetweenWaypoints >= 1f)
            {
                m_PercentBetweenWaypoints = 0;
                m_nextMoveTime = Time.time + m_WaitTime;
                m_IndexCounter++;
                m_Indexes.x = (int)Mathf.Repeat(m_IndexCounter, m_NumberOfPoints);
                m_Indexes.y = (int)Mathf.Repeat(m_IndexCounter + 1, m_NumberOfPoints);
            }
        }
    }

    private float Smooth(float x)
    {
        return Mathf.Pow(x, m_Smooth) / (Mathf.Pow(x, m_Smooth) + Mathf.Pow(1 - x, m_Smooth));
    }

    private void OnDrawGizmos()
    {
        if (m_GlobalPositions != null)
        {
            for (int i = 0; i < m_GlobalPositions.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(m_GlobalPositions[i], 0.1f);
            }
        }
        else
        {
            for (int i = 0; i < m_Waypoints.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(m_Waypoints[i] + transform.parent.position, 0.1f);
            }
        }
    }
}
