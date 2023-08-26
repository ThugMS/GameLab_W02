using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    public bool m_fallingOddEeven = false;
    public GameObject m_obstaclePrefab; // �������� ��ֹ� ������
    public float m_spawnInterval = 2.0f; // ��ֹ� ���� ����

    public List<Transform> m_spawnPoints = new List<Transform>(); // ���� ����Ʈ ����Ʈ
    private int m_spawnPointIndex = 0;
    private bool m_useOddSpawnPoints = true;

    private void OnEnable()
    {
        // �ڽ� ������Ʈ�� ���� ����Ʈ�� �߰�
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
        if (m_fallingOddEeven)
        {
            while (true)
            {
                int startIdx = m_useOddSpawnPoints ? 1 : 0;
                for (int i = startIdx; i < m_spawnPoints.Count; i += 2)
                {
                    Instantiate(m_obstaclePrefab, m_spawnPoints[i].position, transform.rotation);
                }

                m_useOddSpawnPoints = !m_useOddSpawnPoints;
                yield return new WaitForSeconds(m_spawnInterval);
            }
        }
        else
        {
            while (m_spawnPointIndex < m_spawnPoints.Count)
            {
                Transform currentSpawnPoint = m_spawnPoints[m_spawnPointIndex];

                // ������ ���� ����Ʈ�� ��ġ�� ��ֹ� ����
                Instantiate(m_obstaclePrefab, currentSpawnPoint.position, transform.rotation);

                // ���� ���� ����Ʈ�� �̵�
                m_spawnPointIndex = (m_spawnPointIndex + 1) % m_spawnPoints.Count;

                yield return new WaitForSeconds(m_spawnInterval);
            }
        }        
    }
}
