using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    public float m_dashVelocity = 50f;
    public float m_accelerationDuration = 0.05f;
    public float m_decelerationDuration = 0.3f;
    public float m_dashDuration = 0.5f;
    private Rigidbody rb;

    private float m_cameraDis = 10f;
    private float m_plus = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !CharacterManager.instance.GetIsDash())
        {
            CharacterManager.instance.SetIsDash(true);
            Vector3 dashDirection = transform.forward;
            rb.velocity = Vector3.zero; // 초기 속도 초기화
            StartCoroutine(IE_mDash(dashDirection));
        }
        
    }

    private IEnumerator IE_mDash(Vector3 dashDirection)
    {
        float startTime = Time.time;
        float accelerationEndTime = startTime + m_accelerationDuration;
        float decelerationStartTime = startTime + m_dashDuration - m_decelerationDuration;

        while (Time.time - startTime < m_dashDuration)
        {
            float elapsedTime = Time.time - startTime;

            float accelerationRatio = Mathf.Clamp01((Time.time - startTime) / m_accelerationDuration);
            float decelerationRatio = Mathf.Clamp01(1 - (Time.time - decelerationStartTime) / m_decelerationDuration);

            float currentSpeed = m_dashVelocity * accelerationRatio * decelerationRatio;

            rb.velocity = dashDirection * currentSpeed;

            yield return null;
        }
        rb.velocity = Vector3.zero; // 동작 종료 시 속도 초기화
        CharacterManager.instance.SetIsDash(false);
    }
}