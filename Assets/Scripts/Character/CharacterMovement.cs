using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody m_rigidbody;

    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_maxAccelration = 10f;
    [SerializeField] private float m_maxDecelration = 10f;
    [SerializeField] private float m_maxTurnSpeed = 10f;

    private Vector3 m_direction = Vector3.zero;
    private Vector3 m_lastDir = Vector3.zero;
    private Vector3 m_velocity;
    private Vector3 m_desiredVelocity;
    
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;
    [SerializeField] private float m_turnSpeed = 0.01f;
    [SerializeField] private float m_maxSpeedChange;
    #endregion

    #region PublicMethod
    private void Update()
    {
        Debug.Log(m_direction);
    }

    private void FixedUpdate()
    {

        Quaternion rotation = Quaternion.LookRotation(m_lastDir);
        m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, m_turnSpeed);

        RunWithAccelration();
    }

    public void OnMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();

        if(input != null)
        {
            m_direction = new Vector3(input.x, 0f, input.y);

            if(m_direction != Vector3.zero)
            {
                m_lastDir = m_direction;
            }
        }
    }
    #endregion

    #region PrivateMethod
    private void RunWithAccelration()
    {
        Vector3 move = m_direction * m_maxSpeed;
        m_rigidbody.velocity = move;
    }
    #endregion
}
