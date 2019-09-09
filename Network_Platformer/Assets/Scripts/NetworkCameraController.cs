using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCameraController : NetworkBehaviour
{
    private const float Y_ANGLE_MIN = -20.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private const float ZOOM_MIN = 2.5f;
    private const float ZOOM_MAX = 10.0f;

    public GameObject m_Player;
    public Transform m_LookAt;
    public Transform m_CameraTransform;
    public LayerMask m_LayerMask;
    public MeshRenderer[] m_PlayerMashes;


    //private Camera m_Camera;

    private float m_distance = 10.0f;
    private float m_currentX = 0.0f;
    private float m_currentY = 0.0f;
    private float m_sensitivityX = 4.0f;
    private float m_sensitivityY = 1.0f;
    private float m_distanceAmount;
    private float m_zoomfactor = ZOOM_MAX;
    private float m_sphereCastRadius = 0.5f;
    private bool m_collided = false;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        m_CameraTransform = Camera.main.transform;
        m_LookAt = this.gameObject.transform.Find("HeadSphere");
        m_PlayerMashes = m_LookAt.transform.parent.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = new Ray(m_LookAt.position, m_CameraTransform.position - m_LookAt.position);
        Debug.DrawRay(m_LookAt.position, m_CameraTransform.position - m_LookAt.position, Color.green, 1f);
        if (Physics.SphereCast(ray, m_sphereCastRadius, out hit, m_zoomfactor, m_LayerMask))
        {

            m_distance = Mathf.Max(0f, hit.distance);
            m_collided = true;
        }
        else
        {
            m_collided = false;
        }

        if (!m_collided)
        {
            m_distanceAmount += 0.1f * Time.deltaTime;
            m_distance = Mathf.Lerp(m_distance, m_zoomfactor, m_distanceAmount);
        }

        if (m_distance <= 1.5f)
        {
            foreach (MeshRenderer PlayerRederer in m_PlayerMashes)
            {
                PlayerRederer.enabled = false;
            }
        }
        else
        {
            foreach (MeshRenderer PlayerRederer in m_PlayerMashes)
            {
                PlayerRederer.enabled = true;
            }
        }

        m_currentX += (Input.GetAxis("Mouse X") * m_sensitivityX);
        m_currentY += (Input.GetAxis("Mouse Y") * m_sensitivityY);
        float m_scroll = -Input.GetAxis("Mouse ScrollWheel") * 5;

        m_currentY = Mathf.Clamp(m_currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        m_zoomfactor = Mathf.Clamp(m_zoomfactor += m_scroll, ZOOM_MIN, ZOOM_MAX);
    }



    private void LateUpdate()
    {
        if (m_LookAt == null)
        {
            return;
        }
        Vector3 m_direction = new Vector3(0, 0, -m_distance);
        Quaternion m_rotation = Quaternion.Euler(m_currentY, m_currentX, 0);
        m_CameraTransform.position = m_LookAt.position + m_rotation * m_direction;
        m_CameraTransform.LookAt(m_LookAt.position);
    }
}
