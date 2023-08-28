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
        // �����ϸ� ù ��° ������Ʈ Ȱ��ȭ
        ActivateNextObject();
        // 10�ʸ��� ActivateObjectsCoroutine �Լ��� ȣ���ϴ� �ڷ�ƾ ����
        StartCoroutine(IE_RandomActivateObjectsCoroutine());
    }

    private void ActivateNextObject()
    {
        // ���� �ε����� ������Ʈ ��Ȱ��ȭ
        if (m_currentObjectIndex < m_ObstacleType.Length)
            m_ObstacleType[m_currentObjectIndex].SetActive(false);

        // ���� ������Ʈ �ε����� �̵�
        m_currentObjectIndex = (m_currentObjectIndex + 1) % m_ObstacleType.Length;

        // ���� �ε����� ������Ʈ Ȱ��ȭ
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

            yield return new WaitForSeconds(m_changeTime); // 10�� ���
        }
    }

    private IEnumerator IE_ActivateObjectsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_changeTime); // 10�� ���
            ActivateNextObject(); // ���� ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
        }
    }
}
