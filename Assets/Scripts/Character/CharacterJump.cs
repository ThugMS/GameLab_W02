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
    public float m_jumpTime = 1.0f;       //점프의 시간
    public float m_jumpHeight = 2.0f;     //점프의 높이
    public float m_maxJumpCount = 2;        //다단 점프 횟수

    [Header("About Gravity")]
    public float m_downwardSpeedLimit = 37.0f; //프레임당 onGround 체킹 로직의 y축 길이보다 더 많은 양을 이동해버리면 바닥을 뚫어버릴 error 발생, 60fps 기준 속도는 37.2, 근데 또 하강 속도가 너무 빠르면 안되므로 조절할 필요 yes
    public float m_defaultGravityScale = 1.0f;
    public float m_upwardGravityScale = 1.0f;    //하강 시 중력 계수
    public float m_downwardGravityScale = 1.0f;    //하강 시 중력 계수

    [Header("About Variable Jump")]
    public bool canVariableJump = true;
    public float m_jumpCutOffGravity = 2.0f;    //가변 점프에서 점프키가 떼어졌을 때 적용할 중력계수



    #endregion
    #region PrivateVariables
    [Header("Private Variables")]
    [SerializeField] private LayerMask m_groundLayer;

    private Rigidbody m_rigidbody;
    private CapsuleCollider m_capsuleCollider;

    private bool isPressingJump = false;    //물리적으로 점프키가 눌렸는지를 확인합니다.
    private bool isDesiredJump = false;     //실질적으로 점프를 시작하는 판정입니다.

    //Ground의 Ray 그리기 위한 값입니다.
    [Header("Ground Raycasting")]
    [SerializeField] float m_gapFromGround = 0.1f;        //아래로 Ray 발사, Ground만 확인한다.
    private float m_gapOnRadius = 0.02f;          //Ray는 실제 콜라이더보다 좌우를 아주 약간 작게 쏩니다. 그 값을 결정합니다. (실제 콜라이더와 완전히 같으면 좌우 충돌도 onGround로 판정할 수가 있습니다.)

    private Vector3 m_colliderCenter;
    private Vector3 m_rayDirection = new Vector3(0.0f, -1.0f, 0.0f);      //Ray의 방향을 확인합니다.

    //내부 물리 처리를 위한 변수들입니다.
    private Vector3 m_velocity;       //내부적으로 계산하기 위한 velocity입니다.
    private Vector3 m_gravity;     //중력값을 조율하기 위한 값입니다.
    private float m_gravityMultiflier = 1.0f;  //중력값에 곱해질 계수

    //다단 점프를 위한 변수입니다.
    [SerializeField]private int m_jumpCount = 0;
    #endregion
    #region PublicMethod

    public void OnJumpPressed(InputAction.CallbackContext _callback)
    {
        if (_callback.started)
        {
            //점프가 눌린 순간(KeyDown) 호출됩니다.
            isPressingJump = true;
            isDesiredJump = true;
        }

        if (_callback.canceled)
        {
            //점프 키가 떼어진 순간(KeyUp) 호출됩니다.
            isPressingJump = false;
        }
    }

    public void OnMovePressed(InputAction.CallbackContext _callback)
    {
        Debug.Log("움직임: " + _callback.ReadValue<Vector2>());
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
            //DesiredJump: 점프키가 눌렸거나 점프가 가능한 상태라면 true.
            Jump();
            return; //점프를 적용하여 rigidbody.velocity가 수정되었으니 이번 프레임엔 중력 변화값 계산을 적용하지 않음.
        }

        CheckGravityScale();
        CheckJumpEnded();

        //하강 속도가 일정 속도 이상을 넘어가면, 바닥을 뚫어버릴 가능성이 있습니다. 따라서 제한합니다.
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
            //땅 위에 있을 때 점프를 실행합니다.
            //땅에 닿는 순간 Fs = mas = mgh: (1/2)mv^2 = mgh, v = sqrt(2gh)
            //점프 시간을 정하려면 중력 값을 수정해야함 -> FixedUpdate의 CalculateGravity
            m_velocity.y = Mathf.Sqrt(-2f * m_gravity.y * m_jumpHeight);

            if(m_velocity.y >= m_rigidbody.velocity.y)
            {
                //만약 현재 속도가 더 빠르다면 이단 점프를 허용하지 않습니다.
                m_rigidbody.velocity = m_velocity;
            }
        }
    }

    private void SetGravityByJumpTime()
    {
        //변위 s(t) = (1/2)at^2
        //점프 높이(변위): 고정, 점프 시간: 고정 -> 중력가속도를 수정해야함
        //a = 2s/(t^2)
        m_gravity = new Vector3(0f, (-2f * m_jumpHeight) / (m_jumpTime * m_jumpTime), 0f);

        //중력 계수를 정했다면 해당 값을 곱해줌 (기본값: 1)
        m_gravity *= m_gravityMultiflier;

        //중력을 a로 만들어주기 위해, a-g만큼 힘을 가해줌
        m_rigidbody.AddForce(m_gravity - Physics.gravity);
    }

    private bool CheckOnGround()
    {
        //Update에서 호출됩니다. 매 프레임 땅 위에 있는지를 확인합니다.
        RaycastHit hit;
        m_colliderCenter = transform.position + m_capsuleCollider.center;
        float rayDistance = (m_capsuleCollider.height * 0.5f) - m_capsuleCollider.radius + m_gapFromGround + m_gapOnRadius;

        return Physics.SphereCast(
            m_colliderCenter,
            m_capsuleCollider.radius - m_gapOnRadius,
            m_rayDirection,
            out hit,
            rayDistance, //radius가 실제 콜라이더보다 약간 작기 때문에, gapOnRadius를 더해주어 상하길이는 실제 콜라이더와 비슷하게 보정합니다.
            m_groundLayer
            );
    }

    private void CheckGravityScale()
    {
        //상승 중이라면
        if (m_rigidbody.velocity.y > 0.01f)
        {
            if (!isOnGround)
            {
                //상승중이고, 발판 위가 아니라면, 상승 중력 계수로 설정
                m_gravityMultiflier = m_upwardGravityScale;

                //VariableJump가 true일 경우 (like 마리오)
                if (canVariableJump && isJumping && !isPressingJump)
                {
                    //버튼이 손에서 떼어지면 점프의 중력을 확 올려버림
                    //만약 jumping이 아닌데 y축 상승중일 경우 여기서 오류 발생할 가능성 있음
                    m_gravityMultiflier = m_jumpCutOffGravity;
                }
            }
            else
            {
                //상승 중이고, 발판 위라면 기본값
                //움직이는 발판, 오르막 등
                m_gravityMultiflier = m_defaultGravityScale;
            }
        }
        else if (m_rigidbody.velocity.y < 0.01f)
        {
            if (!isOnGround)
            {
                //하강중이고, 발판 위가 아니라면, 하강 중력 계수로 설정
                m_gravityMultiflier = m_downwardGravityScale;
            }
            else
            {
                //하강중이고, 발판 위라면 하강 중력 계수로 설정
                //움직이는 발판, 내리막 등
                m_gravityMultiflier = m_defaultGravityScale;
            }
        }
        else
        {
            //Y축으로 이동하고 있지 않다면
            m_gravityMultiflier = m_defaultGravityScale;
        }
    }

    private void CheckJumpEnded()
    {

        if (m_rigidbody.velocity.y>=-0.01f&&
            m_rigidbody.velocity.y<=0.01f&&
            isOnGround)
        {
            //Y축 속도가 0이고, 땅 위라면 더이상 점프 중이 아님
            isJumping = false;
            m_jumpCount = 0;
            Debug.Log("점프 초기화");
        }
    }

    private void OnDrawGizmos()
    {
        //Scene View의 
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
