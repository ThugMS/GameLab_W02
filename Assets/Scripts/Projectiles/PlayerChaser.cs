using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_player;
    public float m_lerpDelay = 0.5f;
    public float m_moveSpeed = 1f;
    #endregion

    #region PrivateVariables
    private Rigidbody m_rigidbody;
    private TongnamuAddForcer m_addForcer;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_addForcer = GetComponent<TongnamuAddForcer>();
        m_addForcer.useStun = false;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_player.transform.position - transform.position), m_lerpDelay);
        Vector3 move = transform.forward * m_moveSpeed;
        //m_rigidbody.velocity = new Vector3(move.x, m_rigidbody.velocity.y, move.z);
        m_rigidbody.velocity = new Vector3(move.x, move.y, move.z);
        Vector2 temp = (new Vector2(move.x, move.z)).normalized;
        m_addForcer.m_direction = new Vector3(temp.x,0.8f, temp.y);
    }
    #endregion
}
