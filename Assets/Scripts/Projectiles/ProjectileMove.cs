using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    #region PublicVariables
    [Header("Move")]
    public float m_speed;
    public float m_accel;
    [Header("Rotate")]
    public Vector3 m_direction = Vector3.forward;
    #endregion
    #region PrivateVariables
    private Rigidbody m_rigidbody;
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        m_rigidbody.velocity = m_speed * m_direction.normalized;

        m_speed += m_accel * Time.fixedDeltaTime;
    }
    #endregion
}
