using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJump : MonoBehaviour
{
    #region PublicVariables
    [Header("can -")]
    public bool canUseJump = true;

    [Header("is - ")]
    public bool isOnGround = false;
    public bool isJumping = false;


    #endregion
    #region PrivateVariables
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;

    private bool isPressingJump = false;    //���������� ����Ű�� ���ȴ����� Ȯ���մϴ�.
    private bool isDesiredJump = false;     //���������� ������ �����ϴ� �����Դϴ�.

    //Ground�� Ray �׸��� ���� ���Դϴ�.
    [Header("Ground Ray")]
    [SerializeField] float gapFromGround = 0.1f;        //�Ʒ��� Ray �߻�, Ground�� Ȯ���Ѵ�.
    float gapOnRadius = 0.02f;          //Ray�� ���� �ݶ��̴����� �¿츦 ���� �ణ �۰� ���ϴ�. �� ���� �����մϴ�. (���� �ݶ��̴��� ������ ������ �¿� �浹�� onGround�� ������ ���� �ֽ��ϴ�.)

    private Vector3 colliderCenter;
    private Vector3 rayDirection = new Vector3(0.0f, -1.0f, 0.0f);      //Ray�� ������ Ȯ���մϴ�.
    
    //���� ���� ó���� ���� �������Դϴ�.
    private Vector3 velocity;       //���������� ����ϱ� ���� velocity�Դϴ�.
    #endregion
    #region PublicMethod
    public void Jump()
    {
        if (isOnGround)
        {
            velocity = body.velocity;
            //�� ���� ���� �� ������ �����մϴ�.
            velocity.y = 10.0f;
            
            body.velocity = velocity;
            isDesiredJump = false;
        }
    }

    public void OnJumpPressed(InputAction.CallbackContext callback)
    {
        Debug.Log("���� ����");

        if (callback.started)
        {
            //������ ���� ����(KeyDown) ȣ��˴ϴ�.
            isPressingJump = true;
            isDesiredJump = true;
        }

        if (callback.canceled)
        {
            //���� Ű�� ������ ����(KeyUp) ȣ��˴ϴ�.
            isPressingJump = false;
        }
    }

    public void OnMovePressed(InputAction.CallbackContext callback)
    {
        Debug.Log("������: " + callback.ReadValue<Vector2>());
    }

    public bool GetOnGround()
    {
        return isOnGround;
    }

    #endregion
    #region PrivateMethod


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        isOnGround = CheckOnGround();
    }

    private void FixedUpdate()
    {
        if (isDesiredJump)
        {
            //DesiredJump: ����Ű�� ���Ȱų� ������ ������ ���¶�� ȣ��˴ϴ�.
            Jump();
        }
    }


    private bool CheckOnGround()
    {
        //Update���� ȣ��˴ϴ�. �� ������ �� ���� �ִ����� Ȯ���մϴ�.
        RaycastHit hit;
        colliderCenter = transform.position + capsuleCollider.center;

        return Physics.SphereCast(
            colliderCenter,
            capsuleCollider.radius - gapOnRadius,
            rayDirection,
            out hit,
            (capsuleCollider.height * 0.5f) - capsuleCollider.radius + gapFromGround + gapOnRadius, //radius�� ���� �ݶ��̴����� �ణ �۱� ������, gapOnRadius�� �����־� ���ϱ��̴� ���� �ݶ��̴��� ����ϰ� �����մϴ�.
            groundLayer
            );
    }

    private void OnDrawGizmos()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        colliderCenter = transform.position + capsuleCollider.center;
        if (isOnGround) { Gizmos.color = Color.green; }
        else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(
            colliderCenter - rayDirection * (0.5f * capsuleCollider.height - capsuleCollider.radius),
            colliderCenter + rayDirection * (0.5f * capsuleCollider.height - capsuleCollider.radius));
    }

    #endregion
}
