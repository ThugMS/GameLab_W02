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
            List<Transform> availableSpawnPoints = new List<Transform>(m_spawnPoints); // ��� ������ ���� ����Ʈ ����Ʈ�� ����

            // ������ ���� ����Ʈ���� �����ϰ� �����Ͽ� ��ֹ� ����
            for (int i = 0; i < halfSpawnCount; i++)
            {
                if (availableSpawnPoints.Count == 0)
                    break;

                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex); // ���õ� ���� ����Ʈ�� ��� �Ұ����� ����Ʈ���� ����

                GameObject obstaclePrefab = m_obstaclePrefabs[Random.Range(0, m_obstaclePrefabs.Count)];
                Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(m_spawnInterval);
        }
    }
}
