using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JHMoveTest01 : MonoBehaviour
{
    public float m_dasheVelocity = 30f;     //대쉬 속도
    public float m_acceleration = 20f;      //가속도
    public float m_deceleration = 20f;      //감속도
    public float m_dashDuration = 1.0f;     //대쉬 지속시간
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

            //가속도 비율 계산(0에서 1사이)
            float accelerationRatio = Mathf.Clamp01((Time.time - startAccelerationTime) * m_acceleration / m_dasheVelocity);
            //감속도 비율 계산(1에서 0사이)
            float decelerationRatio = Mathf.Clamp01(1 - (Time.time - endDecelerationTime) * m_deceleration / m_dasheVelocity);
            //현재 속도 계산(가속도 및 감속도 적용)
            float currentVelocity = m_dasheVelocity * accelerationRatio * decelerationRatio;

            rb.velocity = slideDirection * currentVelocity;

            yield return null;
        }

        isSliding = false;
        rb.velocity = Vector3.zero;
    }
}