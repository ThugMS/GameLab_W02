using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JHMoveTest : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    //대쉬 변수
    public float m_dashForce = 10f;          // 대쉬 시작할 때의 가속도
    public float m_maxDashSpeed = 20f;       // 최대 대쉬 속도
    public float m_dashDuration = 1.0f;      // 대쉬 시간

    private Rigidbody rb;
    private Vector2 movementInput;
    private bool isDashing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody >();
    }

    private void Update()
    {
        /*if (!isDashing)
        {
            // 이동 입력 처리
            Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y) * moveSpeed;

            // 이동
            rb.velocity = new Vector3(movement.x, 0*//*rb.velocity.y*//*, movement.z);

            // 회전
            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotationSpeed * Time.deltaTime);
            }

        }*/
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing)
        {
            StartCoroutine(IE_Dash());
        }
    }

    private IEnumerator IE_Dash()
    {
        isDashing = true;
        rb.velocity = transform.forward * m_dashForce;

        float elapsedTime = 0f;

        while (elapsedTime < m_dashDuration)
        {
            // 가속도 제어
            float currentSpeed = rb.velocity.magnitude;
            float targetSpeed = Mathf.Lerp(currentSpeed, m_maxDashSpeed, elapsedTime / m_dashDuration);
            rb.velocity = transform.forward * targetSpeed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector3.zero;  // 대쉬 종료 후 속도 초기화
        isDashing = false;
    }
}