using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpStep : MonoBehaviour
{
    #region PublicVariables
    public Transform m_endTransform;
    public float m_eta = 5f;             //���� ���� �ð�
    public float m_slerpConstantY = -20f;   //Slerp �� center���� y������ �󸶳� ������
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
        //1. �÷��̾��� �̵� ���� ������ ��� �����մϴ�.
        //2. �÷��̾ �������� ���ư��� �մϴ�.

        //1. �÷��̾��� �̵� ���� ������ ��� �����մϴ�.
        CharacterMovement playerMovement = _target.GetComponent<CharacterMovement>();
        CharacterJump playerJump = _target.GetComponent<CharacterJump>();
        CharacterDash playerDash = _target.GetComponent<CharacterDash>();
        Rigidbody playerBody = _target.GetComponent<Rigidbody>();
        CharacterManager.instance.SetCanMove(false);
        playerJump.enabled = false;
        playerDash.enabled = false;

        //2. �÷��̾ �������� ���ư��� �մϴ�.
        //    �ϴ��� ���̰��� 180���� �ణ �۴ٰ� �����ϰ� Slerp�� �����մϴ�.
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
            //�ϴ÷� �������� 
            m_startPos = collision.gameObject.transform.position;
            m_endPos = m_endTransform.position;
            StartCoroutine(IE_MakeSuperJump(collision.gameObject));
        }
    }
    #endregion
}
