using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JHMoveTest01 : MonoBehaviour
{
    public float m_dasheVelocity = 30f;     //�뽬 �ӵ�
    public float m_acceleration = 20f;      //���ӵ�
    public float m_deceleration = 20f;      //���ӵ�
    public float m_dashDuration = 1.0f;     //�뽬 ���ӽð�
    public Rigidbody rb;
    private bool isSliding = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started && !isSliding)
        {
            isSliding = true;
            Vector3 slideDirection = transform.forward;
            StartCoroutine(IE_Dash(slideDirection));
        }
    }

    private IEnumerator IE_Dash(Vector3 slideDirection)
    {
        float startTime = Time.time;
        float startAccelerationTime = startTime;
        float endDecelerationTime = startTime + m_dashDuration - (m_deceleration / m_acceleration);

        while (Time.time - startTime < m_dashDuration)
        {
            float elapsedTime = Time.time - startTime;

            //���ӵ� ���� ���(0���� 1����)
            float accelerationRatio = Mathf.Clamp01((Time.time - startAccelerationTime) * m_acceleration / m_dasheVelocity);
            //���ӵ� ���� ���(1���� 0����)
            float decelerationRatio = Mathf.Clamp01(1 - (Time.time - endDecelerationTime) * m_deceleration / m_dasheVelocity);
            //���� �ӵ� ���(���ӵ� �� ���ӵ� ����)
            float currentVelocity = m_dasheVelocity * accelerationRatio * decelerationRatio;

            rb.velocity = slideDirection * currentVelocity;

            yield return null;
        }

        isSliding = false;
        rb.velocity = Vector3.zero;
    }
}