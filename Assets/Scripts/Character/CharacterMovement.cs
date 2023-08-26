using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region PublicVariables
    public bool m_flipTurnTrigger = false;
    public bool m_rightAngleTurnTrigger = false;

    public GameObject m_followTransform;
    public CAMERA_TYPE m_cameraType = CAMERA_TYPE.BACK;
    #endregion

    #region PrivateVariables
    [SerializeField] private Rigidbody m_rigidbody;

    [SerializeField] private float m_maxSpeed = 10f;


    private Vector3 m_moveDirection = Vector3.zero;
    private Vector3 m_lastDir = Vector3.zero;
    private Vector3 m_look = Vector3.zero;

    [SerializeField] private float m_turnSpeed = 0.01f;


    [SerializeField] private float m_rotationPower = 3f;
    [SerializeField] private Quaternion m_nextRotation;
    [SerializeField] private float m_rotationLerp = 0.5f;

    //카메라 모드가 Fixed일 때 기준 방향
    [HideInInspector] private Vector3 m_forwardDirectionOnFixedMove;
    #endregion

    #region PublicMethod
    private void Start()
    {

    }
    private void Update()
    {
        #region fixedview camera
        if(CAMERA_TYPE.FIXED == m_cameraType)
        {
            if (m_moveDirection == Vector3.zero)
            {
                CharacterManager.instance.SetIsMove(false);
            }
            else
            {
                CharacterManager.instance.SetIsMove(true);
            }

        }
        #endregion

        
    }

    private void FixedUpdate()
    {
        #region shoulderview camera
        if (CAMERA_TYPE.BACK == m_cameraType)
        {
            if (m_moveDirection == Vector3.zero)
            {
                CharacterManager.instance.SetIsMove(false);
            }
            else
            {
                CharacterManager.instance.SetIsMove(true);
            }

            m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_look.x * m_rotationPower, Vector3.up);
            m_followTransform.transform.rotation *= Quaternion.AngleAxis(m_look.y * m_rotationPower, Vector3.right);

            var angles = m_followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = m_followTransform.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            m_followTransform.transform.localEulerAngles = angles;
        }
        #endregion

        #region FixedView Move
        if (CAMERA_TYPE.FIXED == m_cameraType)
        {
            if (m_lastDir != Vector3.zero)
            {
                Vector2 from = new Vector2(m_lastDir.x, m_lastDir.z);
                Vector2 to = new Vector2(m_forwardDirectionOnFixedMove.x, m_forwardDirectionOnFixedMove.z);
                Vector2 dir = new Vector2(from.x*to.y + from.y*to.x, from.y*to.y -  from.x*to.x);

                Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));    
                m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, 1);
            }
            if (CharacterManager.instance.GetIsMove() == true && CharacterManager.instance.GetIsDash() == false)
            {
                ApplyMovement();
            }
            else
            {
                m_rigidbody.angularVelocity = new Vector3(0, 0, 0);
            }

        }
        #endregion
        //if (m_lastDir != Vector3.zero)
        //{
        //    Quaternion rotation = Quaternion.LookRotation(m_lastDir);
        //    m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, rotation, m_turnSpeed);
        //}

        //if (CharacterManager.instance.GetCanMove() == true)
        //{
        //    ApplyMovement();
        //}

        #region Shoulderview Move
        if (CAMERA_TYPE.BACK == m_cameraType)
        {

            if(CharacterManager.instance.GetCanMove() == true)
            {
                //m_nextRotation = Quaternion.Lerp(m_followTransform.transform.rotation, m_nextRotation, m_rotationLerp);


                if (CharacterManager.instance.GetIsMove() == true && CharacterManager.instance.GetIsDash() == false)

                {
                    m_nextRotation = Quaternion.Lerp(m_followTransform.transform.rotation, m_nextRotation, m_rotationLerp);



                    if (CharacterManager.instance.GetIsMove() == true && CharacterManager.instance.GetIsDash() == false)
                    {
                        m_nextRotation = Quaternion.Euler(new Vector3(0, m_nextRotation.eulerAngles.y, 0));

                        Vector2 movedirection = new Vector2(m_lastDir.x, m_lastDir.z);
                        Vector2 a = new Vector2(0, 1f);
                        float angle = Vector2.Angle(a, movedirection);
                        if (movedirection.x < 0)
                        {
                            angle *= -1f;
                        }

                        transform.rotation = Quaternion.Euler(0, m_nextRotation.eulerAngles.y + angle, 0);

                        ApplyMovement();
                    }
                    else
                    {
                        m_rigidbody.angularVelocity = new Vector3(0, 0, 0);
                        //m_rigidbody.velocity = Vector3.zero;
                    }
                }
            }
        }
        #endregion
    }

    public void OnMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        if (input != null)
        {
            m_moveDirection = new Vector3(input.x, 0f, input.y);

            if (m_moveDirection != Vector3.zero)
            {
                m_lastDir = m_moveDirection;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext _context)
    {
        m_look = _context.ReadValue<Vector2>();
    }

    public void SetCameraType(CAMERA_TYPE _type, Vector3 _forwardDirectionOnFixedMove = new Vector3())
    {
        //특정 영역 입장 시 CameraType을 정하기 위해 사용
        m_cameraType = _type;
        m_forwardDirectionOnFixedMove = _forwardDirectionOnFixedMove.normalized;
    }
    #endregion

    #region PrivateMethod
    private void ApplyMovement()
    {
        #region IsoMetric Move
        //Vector3 move = m_moveDirection * m_maxSpeed;
        //m_rigidbody.velocity = new Vector3(move.x, m_rigidbody.velocity.y, move.z);
        #endregion
        #region Shoulderview Move
        Vector3 move = transform.forward * m_maxSpeed;
        m_rigidbody.velocity = new Vector3(move.x, m_rigidbody.velocity.y, move.z);
        #endregion
    }



    #endregion
}
