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
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody m_rigidbody;

    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_maxAccelration = 10f;
    [SerializeField] private float m_maxDecelration = 10f;
    [SerializeField] private float m_maxTurnSpeed = 10f;

    private Vector3 m_direction = Vector3.zero;
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
    #endregion

    #region PublicMethod
    private void Update()
    {
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
        if(m_lastDir != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(m_lastDir);
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, m_turnSpeed);
        }
        
        if(CharacterManager.instance.GetCanMove() == true)
        {
            RunWithAccelration();
        }

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

    public void OnLook(InputAction.CallbackContext _context)
    {
        m_look = _context.ReadValue<Vector2>();
    }

    #endregion

    #region PrivateMethod
    private void RunWithAccelration()
    {   

        Vector3 move = m_direction * m_maxSpeed;

        ////CheckTurn();

        ////if(m_flipTurnTrigger == true)
        ////{
        ////    move = m_direction * m_flipTurnPower;
        ////    StartCoroutine(nameof(IE_OffFlipTurnTrigger));
        ////}
        m_rigidbody.velocity = new Vector3 (move.x, m_rigidbody.velocity.y, move.z);
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
