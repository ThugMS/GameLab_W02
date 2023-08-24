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
        // Ű �Է� ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �̵� ���� ���
        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * moveSpeed;

        // Rigidbody�� velocity �����Ͽ� �̵�
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
