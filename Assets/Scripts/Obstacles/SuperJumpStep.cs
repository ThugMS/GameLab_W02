using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpStep : MonoBehaviour
{
    #region PublicVariables
    public Transform m_endTransform;
    public float m_eta = 5f;             //도착 예정 시간
    public float m_slerpConstantY = -20f;   //Slerp 시 center값을 y값으로 얼마나 내릴지
    #endregion

    #region PrivateVariables
    private Vector3 m_startPos;
    private Vector3 m_endPos;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod


    private IEnumerator IE_MakeSuperJump(GameObject _target)
    {
        //1. 플레이어의 이동 관련 연산을 모두 정지합니다.
        //2. 플레이어가 원형으로 날아가게 합니다.

        //1. 플레이어의 이동 관련 연산을 모두 정지합니다.
        CharacterMovement playerMovement = _target.GetComponent<CharacterMovement>();
        CharacterJump playerJump = _target.GetComponent<CharacterJump>();
        CharacterDash playerDash = _target.GetComponent<CharacterDash>();
        Rigidbody playerBody = _target.GetComponent<Rigidbody>();
        CharacterManager.instance.SetCanMove(false);
        playerJump.enabled = false;
        playerDash.enabled = false;

        //2. 플레이어가 원형으로 날아가게 합니다.
        //    일단은 사이각이 180보다 약간 작다고 가정하고 Slerp를 진행합니다.
        Vector3 center = (m_startPos+ m_endPos)*0.5f + new Vector3(0f, m_slerpConstantY, 0f);
        Vector3 a = m_startPos - center;
        Vector3 b = m_endPos - center;
        float i = 0f;
        float delta = 1 / m_eta;
        while (i < m_eta)
        {
            Vector3 interpolation = Vector3.Slerp(a,b, i*delta);
            _target.transform.position = center + interpolation;
            i += Time.deltaTime;
            yield return null; 
        }


        CharacterManager.instance.SetCanMove(true);
        playerJump.enabled = true;
        playerDash.enabled = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //하늘로 날려보냄 
            m_startPos = collision.gameObject.transform.position;
            m_endPos = m_endTransform.position;
            StartCoroutine(IE_MakeSuperJump(collision.gameObject));
        }
    }
    #endregion
}
