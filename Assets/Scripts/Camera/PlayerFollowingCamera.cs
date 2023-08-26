using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{
    #region PublicVariables
    public Transform m_player;
    public Transform m_distanceTarget;      //�ʱ⿡ �Ÿ��� ����� Target�Դϴ�.
    public float m_lerpSpeed;               //Lerp�� �ӵ��� �����մϴ�.
    #endregion

    #region PrivateVariables
    private Vector3 m_distanceFromPlayer;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //OnEnable �� �÷��̾���� �Ÿ�
        m_distanceFromPlayer = transform.position - m_distanceTarget.position;
        if (!m_player)
        {
            GameObject.Find("Player");
        }
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        //1. �ǽð����� ��ǥ ��ġ���� ��������
        Vector3 aim = m_player.position + m_distanceFromPlayer;
        if ((aim - transform.position).magnitude >= 0.001f)
        {
            //���� ��ġ�� �־�� �� ��ġ�� �ٸ��ٸ� �ش� ��ġ���� ���������մϴ�.
            transform.position = Vector3.Lerp(transform.position, aim, m_lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = aim;
        }
    }
    #endregion
}
