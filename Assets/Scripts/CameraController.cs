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
    private Vector3 m_MoveVelocity;

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
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, -10), m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
        
        
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        float leftMost  = Mathf.Infinity;
        float rightMost = Mathf.NegativeInfinity;
        float botMost   = Mathf.Infinity;
        float topMost   = Mathf.NegativeInfinity;
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Count; i++)
        {
            if (!m_Targets[i].activeSelf)
                continue;

            averagePos += m_Targets[i].transform.position;
            numTargets++;

            leftMost  = Mathf.Min(leftMost, m_Targets[i].transform.position.x);
            botMost   = Mathf.Min(botMost, m_Targets[i].transform.position.y);
            rightMost = Mathf.Max(rightMost, m_Targets[i].transform.position.x);
            topMost   = Mathf.Min(topMost, m_Targets[i].transform.position.y);
        }

        if (numTargets > 0)
            averagePos /= numTargets;
        m_DesiredPosition = averagePos;
        CheckPos(leftMost, rightMost, botMost, topMost);
    }

    private void CheckPos(float leftMost, float rightMost, float botMost, float topMost)
    {

        if ((m_DesiredPosition.x + m_Camera.orthographicSize >= GameVars.MapWidth / 2))
        {
            m_DesiredPosition.x = ((GameVars.MapWidth / 2) + leftMost)/2;
        }
        if((m_DesiredPosition.x - m_Camera.orthographicSize <= -GameVars.MapWidth / 2))
        {
            m_DesiredPosition.x = ((-GameVars.MapWidth / 2) + rightMost) / 2;
        }
        if ((m_DesiredPosition.y + m_Camera.orthographicSize >= GameVars.MapHeight / 2))
        {
            m_DesiredPosition.y = ((GameVars.MapHeight/ 2) + botMost) / 2;
        }
        if((m_DesiredPosition.y - m_Camera.orthographicSize <= -GameVars.MapHeight / 2))
        {
            m_DesiredPosition.y = ((-GameVars.MapHeight / 2) + topMost) / 2;
        }
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        requiredSize = checkZoom(requiredSize);
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Count; i++)
        {
            if (!m_Targets[i].activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].transform.position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize);

        return size;
    }

    private float checkZoom(float desiredSize)
    {
        if ((m_DesiredPosition.x + desiredSize     >=  GameVars.MapWidth / 2) ||
            (m_DesiredPosition.x - desiredSize     <= -GameVars.MapWidth / 2) ||
            (m_DesiredPosition.y + desiredSize / 2 >=  GameVars.MapHeight / 2) ||
            (m_DesiredPosition.y - desiredSize / 2 <= -GameVars.MapHeight / 2))
        {
            desiredSize = m_Camera.orthographicSize;
        }
        
        return desiredSize;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}
