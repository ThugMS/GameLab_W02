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
    public float jumpTime = 1.0f;       //점프의 시간
    public float jumpHeight = 2.0f;     //점프의 높이


    #endregion
    #region PrivateVariables
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;

    private bool isPressingJump = false;    //물리적으로 점프키가 눌렸는지를 확인합니다.
    private bool isDesiredJump = false;     //실질적으로 점프를 시작하는 판정입니다.

    //Ground의 Ray 그리기 위한 값입니다.
    [Header("Ground Ray")]
    [SerializeField] float gapFromGround = 0.1f;        //아래로 Ray 발사, Ground만 확인한다.
    float gapOnRadius = 0.02f;          //Ray는 실제 콜라이더보다 좌우를 아주 약간 작게 쏩니다. 그 값을 결정합니다. (실제 콜라이더와 완전히 같으면 좌우 충돌도 onGround로 판정할 수가 있습니다.)

    private Vector3 colliderCenter;
    private Vector3 rayDirection = new Vector3(0.0f, -1.0f, 0.0f);      //Ray의 방향을 확인합니다.
    
    //내부 물리 처리를 위한 변수들입니다.
    private Vector3 velocity;       //내부적으로 계산하기 위한 velocity입니다.
    private Vector3 gravity;     //중력값을 조율하기 위한 값입니다.
    #endregion
    #region PublicMethod
    public void Jump()
    {
        if (isOnGround)
        {
            //얼마나 뛸지, 몇초 뛸지
            velocity = body.velocity;
            //땅 위에 있을 때 점프를 실행합니다.
            //땅에 닿는 순간 Fs = mas = mgh: (1/2)mv^2 = mgh, v = sqrt(2gh)
            //점프 시간을 정하려면 중력 값을 수정해야함 -> FixedUpdate의 CalculateGravity
            velocity.y = Mathf.Sqrt(-2f * gravity.y * jumpHeight);
            
            body.velocity = velocity;
            isDesiredJump = false;
        }
    }

    public void OnJumpPressed(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            //점프가 눌린 순간(KeyDown) 호출됩니다.
            isPressingJump = true;
            isDesiredJump = true;
        }

        if (callback.canceled)
        {
            //점프 키가 떼어진 순간(KeyUp) 호출됩니다.
            isPressingJump = false;
        }
    }

    public void OnMovePressed(InputAction.CallbackContext callback)
    {
        
        Debug.Log("움직임: " + callback.ReadValue<Vector2>());
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
            //DesiredJump: 점프키가 눌렸거나 점프가 가능한 상태라면 true.
            Jump();
            return; //점프를 적용하여 rigidbody.velocity가 수정되었으니 이번 프레임엔 중력 변화값 계산을 적용하지 않음.
        }
        
        CalculateGravity();
    }

    private float lt = 0f;
    private void SetGravityByJumpTime()
    {
        //변위 s(t) = (1/2)at^2
        //점프 높이(변위): 고정, 점프 시간: 고정 -> 중력가속도를 수정해야함
        //a = 2s/(t^2)
        gravity = new Vector3(0f, (-2f*jumpHeight)/(jumpTime*jumpTime), 0f);
        //중력을 a로 만들어주기 위해, a-g만큼 힘을 가해줌
        body.AddForce(gravity-Physics.gravity);
    }

    private bool CheckOnGround()
    {
        //Update에서 호출됩니다. 매 프레임 땅 위에 있는지를 확인합니다.
        RaycastHit hit;
        colliderCenter = transform.position + capsuleCollider.center;

        return Physics.SphereCast(
            colliderCenter,
            capsuleCollider.radius - gapOnRadius,
            rayDirection,
            out hit,
            (capsuleCollider.height * 0.5f) - capsuleCollider.radius + gapFromGround + gapOnRadius, //radius가 실제 콜라이더보다 약간 작기 때문에, gapOnRadius를 더해주어 상하길이는 실제 콜라이더와 비슷하게 보정합니다.
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
