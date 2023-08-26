using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{
    #region PublicVariables
    public Transform m_player;
    public Transform m_distanceTarget;      //초기에 거리를 계산할 Target입니다.
    public float m_lerpSpeed;               //Lerp의 속도를 결정합니다.
    #endregion

    #region PrivateVariables
    private Vector3 m_distanceFromPlayer;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //OnEnable 시 플레이어와의 거리
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
        //1. 실시간으로 목표 위치까지 선형보간
        Vector3 aim = m_player.position + m_distanceFromPlayer;
        if ((aim - transform.position).magnitude >= 0.001f)
        {
            //현재 위치와 있어야 할 위치가 다르다면 해당 위치까지 선형보간합니다.
            transform.position = Vector3.Lerp(transform.position, aim, m_lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = aim;
        }
    }
    #endregion
}
