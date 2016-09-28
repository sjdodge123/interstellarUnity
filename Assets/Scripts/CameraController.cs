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
        
        Vector3 temp = Vector3.one;
        temp.z = m_DesiredPosition.z;
        if ((m_DesiredPosition.x + m_DesiredSize >= GameVars.MapWidth / 2))
        {
            temp.x = GameVars.MapWidth / 2;

            m_DesiredPosition.x = m_DesiredPosition.x - (m_DesiredPosition.x + m_DesiredSize - temp.x);
            //m_DesiredPosition.x = (temp + N * m_DesiredPosition) / (N + 1);
            //m_DesiredSize = Mathf.Abs(m_DesiredPosition.x - GameVars.MapWidth / 2);
            
            edgeBound = true;
            
        }
        
        if((m_DesiredPosition.x - m_DesiredSize <= -GameVars.MapWidth / 2))
        {
            temp.x = -GameVars.MapWidth / 2;
            
            m_DesiredPosition.x = m_DesiredPosition.x - (m_DesiredPosition.x - m_DesiredSize - temp.x);

            edgeBound = true;
        }
        
        if ((m_DesiredPosition.y + m_DesiredSize >= GameVars.MapHeight / 2))
        {
            temp.y = GameVars.MapHeight / 2;
            m_DesiredPosition.y = m_DesiredPosition.y - (m_DesiredPosition.y + m_DesiredSize - temp.y);

            edgeBound = true;
        }
        
        if((m_DesiredPosition.y - m_DesiredSize <= -GameVars.MapHeight / 2))
        {
            temp.y = -GameVars.MapHeight / 2;
            m_DesiredPosition.y = m_DesiredPosition.y - (m_DesiredPosition.y - m_DesiredSize - temp.y);

            edgeBound = true;
            
        }
        edgeBound = false;
        
    }

    private void Zoom()
    {
        
        checkZoom();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, m_DesiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    void FindRequiredSize()
    {
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

    private void checkZoom()
    {
        /*
        if ((m_DesiredPosition.x + desiredSize     >=  GameVars.MapWidth / 2) ||
            (m_DesiredPosition.x - desiredSize     <= -GameVars.MapWidth / 2) ||
            (m_DesiredPosition.y + desiredSize / 2 >=  GameVars.MapHeight / 2) ||
            (m_DesiredPosition.y - desiredSize / 2 <= -GameVars.MapHeight / 2))
        {
            desiredSize = m_Camera.orthographicSize;
        }
        
        return desiredSize;
        */
    }
}
