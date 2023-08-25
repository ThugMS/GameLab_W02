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
    [Header("Time of move")]
    public bool isUsingTimer = false;
    public float m_startDelay = 1f;
    public float m_moveTime = 5f;
    #endregion

    #region PrivateVariables
    private bool canMove = true;
    private bool CanMove
    {
        get { return canMove; }
        set { canMove = value; m_rigidbody.velocity = new Vector3(); }
    }


    private Rigidbody m_rigidbody;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        canMove = true;
        if(isUsingTimer)
            StartCoroutine(IE_MoveControl(m_startDelay, m_moveTime));
    }



    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }
    private void Move()
    {
        m_rigidbody.velocity = m_speed * m_direction.normalized;

        m_speed += m_accel * Time.fixedDeltaTime;
    }

    private IEnumerator IE_MoveControl(float _startDelay, float _moveTime)
    {
        CanMove = false;
        yield return new WaitForSeconds(_startDelay);

        CanMove = true;
        yield return new WaitForSeconds(_moveTime);

        CanMove = false;
    }
    #endregion
}
