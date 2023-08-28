using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner_RD : MonoBehaviour
{
    public bool m_fallingOddEven = false;
    public List<GameObject> m_obstaclePrefabs;
    public float m_spawnInterval = 2.0f;

    public List<Transform> m_spawnPoints = new List<Transform>();
    private int m_spawnPointIndex = 0;

    private void OnEnable()
    {
        AddSpawnPoints();
        StartCoroutine(IE_SpawnObstacles());
    }

    void AddSpawnPoints()
    {
        foreach (Transform child in transform)
        {
            m_spawnPoints.Add(child);
        }
    }

    IEnumerator IE_SpawnObstacles()
    {
        while (true)
        {
            

            int halfSpawnCount = m_spawnPoints.Count / 2;
            List<Transform> availableSpawnPoints = new List<Transform>(m_spawnPoints); // 사용 가능한 스폰 포인트 리스트를 복사

            // 절반의 스폰 포인트에서 랜덤하게 선택하여 장애물 생성
            for (int i = 0; i < halfSpawnCount; i++)
            {
                if (availableSpawnPoints.Count == 0)
                    break;

                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex); // 선택된 스폰 포인트를 사용 불가능한 리스트에서 제거

                GameObject obstaclePrefab = m_obstaclePrefabs[Random.Range(0, m_obstaclePrefabs.Count)];
                Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(m_spawnInterval);
        }
    }
}
