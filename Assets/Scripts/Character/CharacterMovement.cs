using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody m_rigidbody;

    [SerializeField] private float m_maxSpeed = 1000.0f;
    [SerializeField] private float m_maxAccelration = 10f;
    [SerializeField] private float m_maxDecelration = 10f;
    [SerializeField] private float m_maxTurnSpeed = 10f;

    private Vector3 m_direction;
    private Vector3 m_velocity;
    
    private float m_acceleration;
    private float m_deceleration;
    private float m_turnspeed;
    #endregion

    #region PublicMethod
    private void Update()
    {
        if (m_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(m_direction);
        }
    }

    private void FixedUpdate()
    {
        m_velocity = m_rigidbody.velocity;

        RunWithAccelration();
    }

    public void OnMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();

        if(input != null)
        {
            m_direction = new Vector3(input.x, 0f, input.y);
        }

        Debug.Log(m_direction);
    }
    #endregion

    #region PrivateMethod
    private void RunWithAccelration()
    {
        m_velocity = m_direction.normalized * m_maxSpeed;

    }
    #endregion
}
