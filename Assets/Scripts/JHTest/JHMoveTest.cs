using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHMoveTest : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 키 입력 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 이동 방향 계산
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * moveSpeed;

        // Rigidbody에 velocity 설정하여 이동
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
