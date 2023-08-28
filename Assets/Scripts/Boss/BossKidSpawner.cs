using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKidSpawner : MonoBehaviour
{
    #region PublicVariables
    [Header("On Test")]
    public float m_spawnDelay = 20f;
    public float m_interval = 10f;
    public int m_spawnCount = 5;

    [Header("About Spawn")]
    public Transform[] m_spawnPosition;
    public PlayerChaser m_bossKid;
    public GameObject m_player;


    [Header("Kid Specification")]
    public float m_lerpDelay;
    public float m_moveSpeed;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    public IEnumerator IE_SpawnKid(float _delay = 20f, float _interval = 10f, int _count = 5)
        //_delay: 시작 전, _interval: 소환 주기, _count: 소환 횟수
    {
        yield return new WaitForSeconds(_delay);
        for(int i = 0; i < _count; i++) { 
            for(int j = 0; j < m_spawnPosition.Length; j++)
            {
                PlayerChaser spawned = Instantiate(m_bossKid, m_spawnPosition[j]);
                spawned.m_player = m_player;
                spawned.m_lerpDelay = m_lerpDelay;
                spawned.m_moveSpeed = m_moveSpeed;
            }
            yield return new WaitForSeconds(_interval);
        }
    }

    #endregion

    #region PrivateMethod
    private void OnEnable()
    {
            StartCoroutine(IE_SpawnKid(m_spawnDelay, m_interval, m_spawnCount));
    }
    #endregion
}
