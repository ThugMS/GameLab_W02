using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    public GameObject m_obstaclePrefab; // �������� ��ֹ� ������
    public float m_spawnInterval = 2.0f; // ��ֹ� ���� ����

    public List<Transform> m_spawnPoints = new List<Transform>(); // ���� ����Ʈ ����Ʈ
    private int m_spawnPointIndex = 0;

    void Start()
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
        while (m_spawnPointIndex < m_spawnPoints.Count)
        {
            Transform currentSpawnPoint = m_spawnPoints[m_spawnPointIndex];

            // ������ ���� ����Ʈ�� ��ġ�� ��ֹ� ����
            Instantiate(m_obstaclePrefab, currentSpawnPoint.position, Quaternion.identity);

            // ���� ���� ����Ʈ�� �̵�
            m_spawnPointIndex = (m_spawnPointIndex + 1) % m_spawnPoints.Count;

            yield return new WaitForSeconds(m_spawnInterval);
        }
    }
}
