using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    #region PublicVariables
    public float m_dashVelocity = 50f;
    public float m_accelerationDuration = 0.05f;
    public float m_decelerationDuration = 0.3f;
    public float m_dashDuration = 0.5f;
    #endregion

    #region PrivateVariables
    private Rigidbody rb;
    private float m_cameraDis = 10f;
    private float m_plus = 0.1f;
    private bool m_jumpDashCount = false;
    #endregion

    #region PublicMethod
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (!CharacterManager.instance.GetIsJump())
        {
            m_jumpDashCount=true;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !CharacterManager.instance.GetIsDash() && m_jumpDashCount)
        {
            CharacterManager.instance.SetIsDash(true);
            CharacterManager.instance.SetCanMove(false);
            Vector3 dashDirection = transform.forward;
            m_jumpDashCount = false;
            rb.velocity = Vector3.zero; // 초기 속도 초기화
            StartCoroutine(IE_mDash(dashDirection));            
        }
        
    }
    #endregion

    #region PrivateMethod   
    private IEnumerator IE_mDash(Vector3 dashDirection)
    {
        float startTime = Time.time;
        float accelerationEndTime = startTime + m_accelerationDuration;
        float decelerationStartTime = startTime + m_dashDuration - m_decelerationDuration;

        while (Time.time - startTime < m_dashDuration)
        {
            float elapsedTime = Time.time - startTime;

            float accelerationRatio = Mathf.Clamp01((Time.time - startTime) / m_accelerationDuration);
            float decelerationRatio = Mathf.Clamp01(1 - (Time.time - decelerationStartTime) / m_decelerationDuration);

            float currentSpeed = m_dashVelocity * accelerationRatio * decelerationRatio;

            rb.velocity = dashDirection * currentSpeed;

            yield return null;
        }
        rb.velocity = Vector3.zero; // 동작 종료 시 속도 초기화
        CharacterManager.instance.SetIsDash(false); 
        CharacterManager.instance.SetCanMove(true);
    }
    #endregion
}