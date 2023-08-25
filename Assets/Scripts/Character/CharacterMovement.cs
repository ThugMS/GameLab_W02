using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region PublicVariables
    public bool m_flipTurnTrigger = false;
    public bool m_rightAngleTurnTrigger = false;

    public GameObject m_followTransform;
    public bool m_isMove = false;
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody m_rigidbody;

    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_maxAccelration = 10f;
    [SerializeField] private float m_maxDecelration = 10f;
    [SerializeField] private float m_maxTurnSpeed = 10f;

    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_lastDir = Vector3.zero;
    private Vector3 m_look = Vector3.zero;
    private Vector3 m_velocity;
    private Vector3 m_desiredVelocity;

    [SerializeField] private float m_acceleration = 0.01f;
    [SerializeField] private float m_curSpeed = 0.01f;
    [SerializeField] private float m_deceleration = 0.01f;
    [SerializeField] private float m_turnSpeed = 0.01f;
    [SerializeField] private float m_maxSpeedChange;
    [SerializeField] private float m_flipTurnPower = 20f;

    [SerializeField] private float m_rotationPower = 3f;
    [SerializeField] private Quaternion m_nextRotation;
    [SerializeField] private float m_rotationLerp = 0.5f;
    #endregion

    #region PublicMethod
    private void Update()
    {
        if (m_moveDirection == Vector3.zero)
        {
            m_isMove = false;
        }
        else
        {
            m_isMove = true;
        }

        m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_look.x * m_rotationPower, Vector3.up);

        m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_look.y * m_rotationPower, Vector3.right);

        var angles = m_followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = m_followTransform.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        m_followTransform.transform.localEulerAngles = angles;
    }

    private void FixedUpdate()
    {
        #region IsoMetric Move
        //if (m_lastDir != Vector3.zero)
        //{
        //    Quaternion rotation = Quaternion.LookRotation(m_lastDir);
        //    m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, m_turnSpeed);
        //}

        //if(CharacterManager.instance.GetCanMove() == true)
        //{
        //    RunWithAccelration();
        //}
        #endregion

        #region Shoulderview Move
        m_nextRotation = Quaternion.Lerp(m_followTransform.transform.rotation, m_nextRotation, Time.deltaTime * m_rotationLerp);
        
        if(m_isMove == true)
        {
            m_rigidbody.rotation = Quaternion.Euler(new Vector3(0, m_nextRotation.eulerAngles.y, 0));
        }
        

        float moveSpeed = m_maxSpeed / 100f;
     
        #endregion
    }

    public void OnMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        if(input != null)
        {
            m_moveDirection = new Vector3(input.x, 0f, input.y);

            if(m_moveDirection != Vector3.zero)
            {   
                m_lastDir = m_moveDirection;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext _context)
    {
        m_look = _context.ReadValue<Vector2>();
        Debug.Log(m_look);
    }

    #endregion

    #region PrivateMethod
    private void RunWithAccelration()
    {
        #region IsoMetric Move
        Vector3 move = m_moveDirection * m_maxSpeed;
        m_rigidbody.velocity = new Vector3 (move.x, m_rigidbody.velocity.y, move.z);
        #endregion

        #region Shoulderview Move

        #endregion
    }

    private float GetAngleFromVector(Vector3 _from, Vector3 _to)
    {
        Vector3 v = _to - _from;

        return Vector3.Angle(_from, _to);
    }


    #endregion
}
