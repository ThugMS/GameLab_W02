using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    #region PublicVariables
    public GameObject player;
    public float lerpDelay = 0.5f;
    public float m_moveSpeed = 1f;
    #endregion

    #region PrivateVariables
    private Rigidbody m_rigidbody;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), lerpDelay);
        Vector3 move = transform.forward * m_moveSpeed;
        m_rigidbody.velocity = new Vector3(move.x, m_rigidbody.velocity.y, move.z);
    }
    #endregion
}
