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
    public float m_jumpTime = 1.0f;       //������ �ð�
    public float m_jumpHeight = 2.0f;     //������ ����
    public float m_maxJumpCount = 2;        //�ٴ� ���� Ƚ��

    [Header("About Gravity")]
    public float m_downwardSpeedLimit = 37.0f; //�����Ӵ� onGround üŷ ������ y�� ���̺��� �� ���� ���� �̵��ع����� �ٴ��� �վ���� error �߻�, 60fps ���� �ӵ��� 37.2, �ٵ� �� �ϰ� �ӵ��� �ʹ� ������ �ȵǹǷ� ������ �ʿ� yes
    public float m_defaultGravityScale = 1.0f;
    public float m_upwardGravityScale = 1.0f;    //�ϰ� �� �߷� ���
    public float m_downwardGravityScale = 1.0f;    //�ϰ� �� �߷� ���

    [Header("About Variable Jump")]
    public bool canVariableJump = true;
    public float m_jumpCutOffGravity = 2.0f;    //���� �������� ����Ű�� �������� �� ������ �߷°��



    #endregion
    #region PrivateVariables
    [Header("Private Variables")]
    [SerializeField] private LayerMask m_groundLayer;

    private Rigidbody m_rigidbody;
    private CapsuleCollider m_capsuleCollider;

    private bool isPressingJump = false;    //���������� ����Ű�� ���ȴ����� Ȯ���մϴ�.
    private bool isDesiredJump = false;     //���������� ������ �����ϴ� �����Դϴ�.

    //Ground�� Ray �׸��� ���� ���Դϴ�.
    [Header("Ground Raycasting")]
    [SerializeField] float m_gapFromGround = 0.1f;        //�Ʒ��� Ray �߻�, Ground�� Ȯ���Ѵ�.
    private float m_gapOnRadius = 0.02f;          //Ray�� ���� �ݶ��̴����� �¿츦 ���� �ణ �۰� ���ϴ�. �� ���� �����մϴ�. (���� �ݶ��̴��� ������ ������ �¿� �浹�� onGround�� ������ ���� �ֽ��ϴ�.)

    private Vector3 m_colliderCenter;
    private Vector3 m_rayDirection = new Vector3(0.0f, -1.0f, 0.0f);      //Ray�� ������ Ȯ���մϴ�.

    //���� ���� ó���� ���� �������Դϴ�.
    private Vector3 m_velocity;       //���������� ����ϱ� ���� velocity�Դϴ�.
    private Vector3 m_gravity;     //�߷°��� �����ϱ� ���� ���Դϴ�.
    private float m_gravityMultiflier = 1.0f;  //�߷°��� ������ ���

    //�ٴ� ������ ���� �����Դϴ�.
    [SerializeField]private int m_jumpCount = 0;
    #endregion
    #region PublicMethod

    public void OnJumpPressed(InputAction.CallbackContext _callback)
    {
        if (_callback.started)
        {
            //������ ���� ����(KeyDown) ȣ��˴ϴ�.
            isPressingJump = true;
            isDesiredJump = true;
        }

        if (_callback.canceled)
        {
            //���� Ű�� ������ ����(KeyUp) ȣ��˴ϴ�.
            isPressingJump = false;
        }
    }

    public void OnMovePressed(InputAction.CallbackContext _callback)
    {
        Debug.Log("������: " + _callback.ReadValue<Vector2>());
    }

    public bool GetOnGround()
    {
        return isOnGround;
    }

    #endregion
    #region PrivateMethod
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        isOnGround = CheckOnGround();
    }

    private void FixedUpdate()
    {
        m_velocity = m_rigidbody.velocity;
        SetGravityByJumpTime();

        if (isDesiredJump)
        {
            //DesiredJump: ����Ű�� ���Ȱų� ������ ������ ���¶�� true.
            Jump();
            return; //������ �����Ͽ� rigidbody.velocity�� �����Ǿ����� �̹� �����ӿ� �߷� ��ȭ�� ����� �������� ����.
        }

        CheckGravityScale();
        CheckJumpEnded();

        //�ϰ� �ӵ��� ���� �ӵ� �̻��� �Ѿ��, �ٴ��� �վ���� ���ɼ��� �ֽ��ϴ�. ���� �����մϴ�.
        m_rigidbody.velocity = new Vector3(m_velocity.x, Mathf.Clamp(m_velocity.y, -m_downwardSpeedLimit, 100), m_velocity.z);
    }
    public void Jump()
    {
        if (isOnGround||m_jumpCount<m_maxJumpCount)
        {
            isDesiredJump = false;
            isJumping = true;
            m_jumpCount += 1;

            m_velocity = m_rigidbody.velocity;
            //�� ���� ���� �� ������ �����մϴ�.
            //���� ��� ���� Fs = mas = mgh: (1/2)mv^2 = mgh, v = sqrt(2gh)
            //���� �ð��� ���Ϸ��� �߷� ���� �����ؾ��� -> FixedUpdate�� CalculateGravity
            m_velocity.y = Mathf.Sqrt(-2f * m_gravity.y * m_jumpHeight);

            if(m_velocity.y >= m_rigidbody.velocity.y)
            {
                //���� ���� �ӵ��� �� �����ٸ� �̴� ������ ������� �ʽ��ϴ�.
                m_rigidbody.velocity = m_velocity;
            }
        }
    }

    private void SetGravityByJumpTime()
    {
        //���� s(t) = (1/2)at^2
        //���� ����(����): ����, ���� �ð�: ���� -> �߷°��ӵ��� �����ؾ���
        //a = 2s/(t^2)
        m_gravity = new Vector3(0f, (-2f * m_jumpHeight) / (m_jumpTime * m_jumpTime), 0f);

        //�߷� ����� ���ߴٸ� �ش� ���� ������ (�⺻��: 1)
        m_gravity *= m_gravityMultiflier;

        //�߷��� a�� ������ֱ� ����, a-g��ŭ ���� ������
        m_rigidbody.AddForce(m_gravity - Physics.gravity);
    }

    private bool CheckOnGround()
    {
        //Update���� ȣ��˴ϴ�. �� ������ �� ���� �ִ����� Ȯ���մϴ�.
        RaycastHit hit;
        m_colliderCenter = transform.position + m_capsuleCollider.center;
        float rayDistance = (m_capsuleCollider.height * 0.5f) - m_capsuleCollider.radius + m_gapFromGround + m_gapOnRadius;

        return Physics.SphereCast(
            m_colliderCenter,
            m_capsuleCollider.radius - m_gapOnRadius,
            m_rayDirection,
            out hit,
            rayDistance, //radius�� ���� �ݶ��̴����� �ణ �۱� ������, gapOnRadius�� �����־� ���ϱ��̴� ���� �ݶ��̴��� ����ϰ� �����մϴ�.
            m_groundLayer
            );
    }

    private void CheckGravityScale()
    {
        //��� ���̶��
        if (m_rigidbody.velocity.y > 0.01f)
        {
            if (!isOnGround)
            {
                //������̰�, ���� ���� �ƴ϶��, ��� �߷� ����� ����
                m_gravityMultiflier = m_upwardGravityScale;

                //VariableJump�� true�� ��� (like ������)
                if (canVariableJump && isJumping && !isPressingJump)
                {
                    //��ư�� �տ��� �������� ������ �߷��� Ȯ �÷�����
                    //���� jumping�� �ƴѵ� y�� ������� ��� ���⼭ ���� �߻��� ���ɼ� ����
                    m_gravityMultiflier = m_jumpCutOffGravity;
                }
            }
            else
            {
                //��� ���̰�, ���� ����� �⺻��
                //�����̴� ����, ������ ��
                m_gravityMultiflier = m_defaultGravityScale;
            }
        }
        else if (m_rigidbody.velocity.y < 0.01f)
        {
            if (!isOnGround)
            {
                //�ϰ����̰�, ���� ���� �ƴ϶��, �ϰ� �߷� ����� ����
                m_gravityMultiflier = m_downwardGravityScale;
            }
            else
            {
                //�ϰ����̰�, ���� ����� �ϰ� �߷� ����� ����
                //�����̴� ����, ������ ��
                m_gravityMultiflier = m_defaultGravityScale;
            }
        }
        else
        {
            //Y������ �̵��ϰ� ���� �ʴٸ�
            m_gravityMultiflier = m_defaultGravityScale;
        }
    }

    private void CheckJumpEnded()
    {

        if (m_rigidbody.velocity.y>=-0.01f&&
            m_rigidbody.velocity.y<=0.01f&&
            isOnGround)
        {
            //Y�� �ӵ��� 0�̰�, �� ����� ���̻� ���� ���� �ƴ�
            isJumping = false;
            m_jumpCount = 0;
            Debug.Log("���� �ʱ�ȭ");
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
