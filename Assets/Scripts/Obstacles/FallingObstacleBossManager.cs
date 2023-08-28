using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleBossManager : MonoBehaviour
{
    public GameObject[] m_ObstacleType;
    public float m_changeTime = 10f;
    private int m_currentObjectIndex = 0;

    private void Start()
    {
        // 시작하면 첫 번째 오브젝트 활성화
        ActivateNextObject();
        // 10초마다 ActivateObjectsCoroutine 함수를 호출하는 코루틴 시작
        StartCoroutine(IE_RandomActivateObjectsCoroutine());
    }

    private void ActivateNextObject()
    {
        // 현재 인덱스의 오브젝트 비활성화
        if (m_currentObjectIndex < m_ObstacleType.Length)
            m_ObstacleType[m_currentObjectIndex].SetActive(false);

        // 다음 오브젝트 인덱스로 이동
        m_currentObjectIndex = (m_currentObjectIndex + 1) % m_ObstacleType.Length;

        // 다음 인덱스의 오브젝트 활성화
        m_ObstacleType[m_currentObjectIndex].SetActive(true);
    }

    private IEnumerator IE_RandomActivateObjectsCoroutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, m_ObstacleType.Length);

            for (int i = 0; i < m_ObstacleType.Length; i++)
            {
                m_ObstacleType[i].SetActive(i == randomIndex);
            }

            yield return new WaitForSeconds(m_changeTime); // 10초 대기
        }
    }

    private IEnumerator IE_ActivateObjectsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_changeTime); // 10초 대기
            ActivateNextObject(); // 다음 오브젝트 활성화/비활성화
        }
    }
}
