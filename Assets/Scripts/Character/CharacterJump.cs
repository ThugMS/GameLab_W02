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

    [Header("About Basic Jump")]
    public float jumpTime = 1.0f;       //������ �ð�
    public float jumpHeight = 2.0f;     //������ ����

    [Header("About Gravity")]
    public float descendingSpeedLimit = 37.0f; //�����Ӵ� onGround üŷ ������ y�� ���̺��� �� ���� ���� �̵��ع����� �ٴ��� �վ���� error �߻�, 60fps ���� �ӵ��� 37.2, �ٵ� �� �ϰ� �ӵ��� �ʹ� ������ �ȵǹǷ� ������ �ʿ� yes
    public float defaultGravityScale = 1.0f;
    public float upwardGravityScale = 1.0f;    //�ϰ� �� �߷� ���
    public float downwardGravityScale = 1.0f;    //�ϰ� �� �߷� ���

    #endregion
    #region PrivateVariables
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;

    private bool isPressingJump = false;    //���������� ����Ű�� ���ȴ����� Ȯ���մϴ�.
    private bool isDesiredJump = false;     //���������� ������ �����ϴ� �����Դϴ�.

    //Ground�� Ray �׸��� ���� ���Դϴ�.
    [Header("Ground Raycasting")]
    [SerializeField] float gapFromGround = 0.1f;        //�Ʒ��� Ray �߻�, Ground�� Ȯ���Ѵ�.
    private float gapOnRadius = 0.02f;          //Ray�� ���� �ݶ��̴����� �¿츦 ���� �ణ �۰� ���ϴ�. �� ���� �����մϴ�. (���� �ݶ��̴��� ������ ������ �¿� �浹�� onGround�� ������ ���� �ֽ��ϴ�.)

    private Vector3 colliderCenter;
    private Vector3 rayDirection = new Vector3(0.0f, -1.0f, 0.0f);      //Ray�� ������ Ȯ���մϴ�.
    
    //���� ���� ó���� ���� �������Դϴ�.
    private Vector3 velocity;       //���������� ����ϱ� ���� velocity�Դϴ�.
    private Vector3 gravity;     //�߷°��� �����ϱ� ���� ���Դϴ�.
    private float gravityMultiflier = 1.0f;  //�߷°��� ������ ���
    #endregion
    #region PublicMethod

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
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        SetGravityByJumpTime();

        if (isDesiredJump)
        {
            //DesiredJump: ����Ű�� ���Ȱų� ������ ������ ���¶�� true.
            Jump();
            return; //������ �����Ͽ� rigidbody.velocity�� �����Ǿ����� �̹� �����ӿ� �߷� ��ȭ�� ����� �������� ����.
        }
        
        ComputeGravityScale();

        //�ϰ� �ӵ��� ���� �ӵ� �̻��� �Ѿ��, �ٴ��� �վ���� ���ɼ��� �ֽ��ϴ�. ���� �����մϴ�.
        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -descendingSpeedLimit, 100), velocity.z);
    }
    public void Jump()
    {
        if (isOnGround)
        {
            velocity = body.velocity;
            //�� ���� ���� �� ������ �����մϴ�.
            //���� ��� ���� Fs = mas = mgh: (1/2)mv^2 = mgh, v = sqrt(2gh)
            //���� �ð��� ���Ϸ��� �߷� ���� �����ؾ��� -> FixedUpdate�� CalculateGravity
            velocity.y = Mathf.Sqrt(-2f * gravity.y * jumpHeight);

            body.velocity = velocity;
            isDesiredJump = false;
        }
    }

    private void SetGravityByJumpTime()
    {
        //���� s(t) = (1/2)at^2
        //���� ����(����): ����, ���� �ð�: ���� -> �߷°��ӵ��� �����ؾ���
        //a = 2s/(t^2)
        gravity = new Vector3(0f, (-2f*jumpHeight)/(jumpTime*jumpTime), 0f);

        //�߷� ����� ���ߴٸ� �ش� ���� ������ (�⺻��: 1)
        gravity *= gravityMultiflier;

        //�߷��� a�� ������ֱ� ����, a-g��ŭ ���� ������
        body.AddForce(gravity-Physics.gravity);
    }

    private bool CheckOnGround()
    {
        //Update���� ȣ��˴ϴ�. �� ������ �� ���� �ִ����� Ȯ���մϴ�.
        RaycastHit hit;
        colliderCenter = transform.position + capsuleCollider.center;
        float rayDistance = (capsuleCollider.height * 0.5f) - capsuleCollider.radius + gapFromGround + gapOnRadius;

        return Physics.SphereCast(
            colliderCenter,
            capsuleCollider.radius - gapOnRadius,
            rayDirection,
            out hit,
            rayDistance, //radius�� ���� �ݶ��̴����� �ణ �۱� ������, gapOnRadius�� �����־� ���ϱ��̴� ���� �ݶ��̴��� ����ϰ� �����մϴ�.
            groundLayer
            );
    }

    private void ComputeGravityScale()
    {
        //��� ���̶��
        if (body.velocity.y > 0.01f)
        {
            if(!isOnGround)
            {
                //������̰�, ���� ���� �ƴ϶��, ��� �߷� ����� ����
                gravityMultiflier = upwardGravityScale;
            }
            else
            {
                //��� ���̰�, ���� ����� �⺻��
                //�����̴� ����, ������ ��
                gravityMultiflier = defaultGravityScale;
            }
        }
        else if (body.velocity.y < 0.01f)
        {
            if (!isOnGround)
            {
                //�ϰ����̰�, ���� ���� �ƴ϶��, �ϰ� �߷� ����� ����
                gravityMultiflier = downwardGravityScale;
            }
            else
            {
                //�ϰ����̰�, ���� ����� �ϰ� �߷� ����� ����
                //�����̴� ����, ������ ��
                gravityMultiflier = defaultGravityScale;
            }
        }
        else
        {
            //Y������ �̵��ϰ� ���� �ʴٸ�
            gravityMultiflier = defaultGravityScale;
            if (isOnGround)
            {
                //Y�� �ӵ��� 0�̰�, �� ����� ���̻� ���� ���� �ƴ�
                isJumping = false;
            }            
        }
    }

    private void OnDrawGizmos()
    { 
        //Scene View�� 
        //capsuleCollider = GetComponent<CapsuleCollider>();
        //colliderCenter = transform.position + capsuleCollider.center;
        //if (isOnGround) { Gizmos.color = Color.green; }
        //else { Gizmos.color = Color.red; }
        //Gizmos.DrawLine(
        //    colliderCenter - rayDirection * (0.5f * capsuleCollider.height - capsuleCollider.radius),
        //    colliderCenter + rayDirection * (0.5f * capsuleCollider.height - capsuleCollider.radius));
    }

    #endregion
}
