using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JHMoveTest : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float slideForce = 20.0f;

    public float slideDuration = 0.5f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private bool isSliding = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody >();
    }

    private void Update()
    {
        if (!isSliding)
        {
            // 이동 입력 처리
            Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y) * moveSpeed;

            // 이동
            rb.velocity = new Vector3(movement.x, 0/*rb.velocity.y*/, movement.z);

            // 회전
            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotationSpeed * Time.deltaTime);
            }

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isSliding)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSliding = true;
            rb.velocity = transform.forward * slideForce;
            StartCoroutine(EndSlide());
        }
    }

    private IEnumerator EndSlide()
    {
        yield return new WaitForSeconds(slideDuration);
        isSliding = false;
    }
}