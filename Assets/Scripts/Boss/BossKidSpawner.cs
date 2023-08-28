using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKidSpawner : MonoBehaviour
{
    #region PublicVariables
    public Transform[] m_spawnPosition;
    public PlayerChaser m_bossKid;
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
                
            }
            yield return new WaitForSeconds(_interval);
        }
    }

    #endregion

    #region PrivateMethod
    private void Start()
    {
        StartCoroutine(IE_SpawnKid(5f, 5f, 5));
    }
    #endregion
}
