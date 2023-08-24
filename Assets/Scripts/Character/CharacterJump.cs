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
    #endregion
    #region PublicMethod
    public void Jump()
    {
        if (isOnGround)
        {
            velocity = body.velocity;
            //땅 위에 있을 때 점프를 실행합니다.
            velocity.y = 10.0f;
            
            body.velocity = velocity;
            isDesiredJump = false;
        }
    }

    public void OnJumpPressed(InputAction.CallbackContext callback)
    {
        Debug.Log("점프 눌림");

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
    }

    private void FixedUpdate()
    {
        if (isDesiredJump)
        {
            //DesiredJump: 점프키가 눌렸거나 점프가 가능한 상태라면 호출됩니다.
            Jump();
        }
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
