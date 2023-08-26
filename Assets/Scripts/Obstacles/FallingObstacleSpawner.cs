using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    public GameObject m_obstaclePrefab; // 떨어지는 장애물 프리팹
    public float m_spawnInterval = 2.0f; // 장애물 생성 간격

    public List<Transform> m_spawnPoints = new List<Transform>(); // 스폰 포인트 리스트
    private int m_spawnPointIndex = 0;

    void Start()
    {
        // 자식 오브젝트를 스폰 포인트로 추가
        AddSpawnPoints();

        StartCoroutine(IE_SpawnObstacl());
    }

    void AddSpawnPoints()
    {
        foreach (Transform child in transform)
        {
            m_spawnPoints.Add(child);
        }
    }

    IEnumerator IE_SpawnObstacl()
    {
        while (m_spawnPointIndex < m_spawnPoints.Count)
        {
            Transform currentSpawnPoint = m_spawnPoints[m_spawnPointIndex];

            // 선택한 스폰 포인트의 위치로 장애물 생성
            Instantiate(m_obstaclePrefab, currentSpawnPoint.position, Quaternion.identity);

            // 다음 스폰 포인트로 이동
            m_spawnPointIndex = (m_spawnPointIndex + 1) % m_spawnPoints.Count;

            yield return new WaitForSeconds(m_spawnInterval);
        }
    }
}
