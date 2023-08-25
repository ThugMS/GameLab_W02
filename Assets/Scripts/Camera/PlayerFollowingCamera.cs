using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{
    #region PublicVariables
    public Transform m_player;
    public float m_lerpSpeed;
    #endregion

    #region PrivateVariables
    private Vector3 m_distanceFromPlayer;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //플레이어와의 거리를 계산합니다.
        m_distanceFromPlayer = transform.position - m_player.position;
    }

    private void Update()
    {
        //1. 실시간으로 목표 위치까지 선형보간
        Vector3 aim = m_player.position + m_distanceFromPlayer;
        if((aim - transform.position).magnitude>=0.001f)
        {
            //현재 위치와 있어야 할 위치가 다르다면 해당 위치까지 선형보간합니다.
            transform.position = Vector3.Lerp(transform.position,aim, m_lerpSpeed*Time.deltaTime);
        }
        else
        {
            transform.position = aim;
        }
    }
    #endregion
}
