using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_MinSize = 6.5f;

    private List<GameObject> m_Targets = new List<GameObject>();

    private float m_ZoomSpeed;
    private Vector3 m_DesiredPosition;
    private float m_DesiredSize;
    private Vector3 m_MoveVelocity;
    private bool edgeBound = false;

    //bounds
    private Vector2 boundsMin;
    private Vector2 boundsMax;

    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
        GameVars.Camera = this;
    }

    void Start()
    {

        //for (var i = 0; i < GameVars.Planets.Count; i++)
        //{
        //    m_Targets.Add(GameVars.Planets[i].gameObject);
        //}
        boundsMin = new Vector2(-GameVars.MapWidth / 2, -GameVars.MapHeight / 2);
        boundsMax = new Vector2(GameVars.MapWidth / 2, GameVars.MapHeight / 2);

        for (var j = 0; j < GameVars.Ships.Count; j++)
        {
            m_Targets.Add(GameVars.Ships[j].gameObject);
        }
    }

    public void AddToCamera(GameObject target)
    {
        if (!m_Targets.Contains(target))
        {
            m_Targets.Add(target);
        }
    }
    public void RemoveFromCamera(GameObject target)
    {
        if (m_Targets.Contains(target))
        {
            m_Targets.Remove(target);
        }
    }

    private void LateUpdate()
    {
        FindAveragePosition();
        FindRequiredSize();

        CheckPos();

        Move();
        Zoom();
    }


    private void Move()
    {
        transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, -10), m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
          
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int N = 0;

        for (int i = 0; i < m_Targets.Count; i++)
        {
            if (!m_Targets[i].activeSelf)
                continue;

            averagePos += m_Targets[i].transform.position;
            N++;
        }

        if (N > 0)
            averagePos /= N;
        m_DesiredPosition = averagePos;
    }

    private void CheckPos()
    {
        float vertExt = m_DesiredSize;
        float horExt = m_Camera.aspect * m_DesiredSize;

        float leftBound = boundsMin.x + horExt;
        float rightBound = boundsMax.x - horExt;
        float bottomBound = boundsMin.x + vertExt;
        float topBound = boundsMax.x - vertExt;

        m_DesiredPosition.x = Mathf.Clamp(m_DesiredPosition.x, leftBound, rightBound);
        m_DesiredPosition.y = Mathf.Clamp(m_DesiredPosition.y, bottomBound, topBound);
    }

    private void Zoom()
    {
        
        //checkZoom();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, m_DesiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    void FindRequiredSize()
    {
        //DEBUG: Perhaps local coordinates cause discrepancy between vertical and horizontal boundary behaviors
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);
        
        m_DesiredSize = 0;

        for (int i = 0; i < m_Targets.Count; i++)
        {
            if (!m_Targets[i].activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].transform.position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            m_DesiredSize = Mathf.Max(m_DesiredSize, Mathf.Abs(desiredPosToTarget.y));

            m_DesiredSize = Mathf.Max(m_DesiredSize, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        m_DesiredSize += m_ScreenEdgeBuffer;

        m_DesiredSize = Mathf.Max(m_DesiredSize, m_MinSize);
        
        

    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, new Vector3(GameVars.MapWidth, GameVars.MapHeight, 0));
    }
}
