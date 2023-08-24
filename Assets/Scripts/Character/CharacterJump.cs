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

    [Header("basic jump variable")]
    public float jumpTime = 1.0f;       //������ �ð�
    public float jumpHeight = 2.0f;     //������ ����


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
    private Vector3 gravity;     //�߷°��� �����ϱ� ���� ���Դϴ�.
    #endregion
    #region PublicMethod
    public void Jump()
    {
        if (isOnGround)
        {
            //�󸶳� ����, ���� ����
            velocity = body.velocity;
            //�� ���� ���� �� ������ �����մϴ�.
            //���� ��� ���� Fs = mas = mgh: (1/2)mv^2 = mgh, v = sqrt(2gh)
            //���� �ð��� ���Ϸ��� �߷� ���� �����ؾ��� -> FixedUpdate�� CalculateGravity
            velocity.y = Mathf.Sqrt(-2f * gravity.y * jumpHeight);
            
            body.velocity = velocity;
            isDesiredJump = false;
        }
    }

    public void OnJumpPressed(InputAction.CallbackContext callback)
    {
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
        if (body.velocity.y > 0f)
        {
            lt += Time.deltaTime;
            Debug.Log(lt);
        }
        else
        {
            lt = 0f;
        }
    }

    private void FixedUpdate()
    {
        SetGravityByJumpTime();

        if (isDesiredJump)
        {
            //DesiredJump: ����Ű�� ���Ȱų� ������ ������ ���¶�� true.
            Jump();
            return; //������ �����Ͽ� rigidbody.velocity�� �����Ǿ����� �̹� �����ӿ� �߷� ��ȭ�� ����� �������� ����.
        }
        
        CalculateGravity();
    }

    private float lt = 0f;
    private void SetGravityByJumpTime()
    {
        //���� s(t) = (1/2)at^2
        //���� ����(����): ����, ���� �ð�: ���� -> �߷°��ӵ��� �����ؾ���
        //a = 2s/(t^2)
        gravity = new Vector3(0f, (-2f*jumpHeight)/(jumpTime*jumpTime), 0f);
        //�߷��� a�� ������ֱ� ����, a-g��ŭ ���� ������
        body.AddForce(gravity-Physics.gravity);
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

    private void CalculateGravity()
    {

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
