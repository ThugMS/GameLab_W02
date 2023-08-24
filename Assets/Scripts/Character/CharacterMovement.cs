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

    [SerializeField] private float m_acceleration = 0.01f;
    [SerializeField] private float m_curSpeed = 0.01f;
    [SerializeField] private float m_deceleration = 0.01f;
    [SerializeField] private float m_turnSpeed = 0.01f;
    [SerializeField] private float m_maxSpeedChange;
    [SerializeField] private float m_flipTurnPower = 20f;
    #endregion

    #region PublicMethod
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if(m_lastDir != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(m_lastDir);
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, m_turnSpeed);
        }
        

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
                if(Mathf.Abs(GetAngleFromVector(m_lastDir, m_direction)) > 150)
                {
                    m_flipTurnTrigger = true;
                }
                m_lastDir = m_direction;
            }
        }
    }
    #endregion

    #region PrivateMethod
    private void RunWithAccelration()
    {   

        Vector3 move = m_direction * m_maxSpeed;
 
        CheckTurn();

        if(m_flipTurnTrigger == true)
        {
            move = m_direction * m_flipTurnPower;
            StartCoroutine(nameof(IE_OffFlipTurnTrigger));
        }

        m_rigidbody.velocity = move;

        //if (m_direction != Vector3.zero)
        //{
        //    m_maxSpeedChange = m_acceleration * Time.deltaTime;
        //}
        //else
        //{
        //    m_maxSpeedChange = m_deceleration * Time.deltaTime;
        //}
    }

    private float GetAngleFromVector(Vector3 _from, Vector3 _to)
    {
        Vector3 v = _to - _from;

        return Vector3.Angle(_from, _to);
    }

    private void CheckTurn()
    {
        if(m_flipTurnTrigger || m_rightAngleTurnTrigger)
        {

            m_curSpeed = 0f;
        }
    }

    private IEnumerator IE_OffFlipTurnTrigger()
    {
        yield return new WaitForSeconds(0.2f);
        m_flipTurnTrigger = false;
    }
    #endregion
}
