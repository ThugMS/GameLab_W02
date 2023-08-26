using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayAplha : MonoBehaviour
{
    public LayerMask m_raycastLayer;
    public float m_maxDistance = 100f;
    public float m_transparencyValue = 0.5f;
    private Transform m_character; // ĳ������ Transform�� �Ҵ��ϴ� ����

    private RaycastHit hit;

    private void Start()
    {
        m_character = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // ī�޶� ��ġ���� ĳ���� �������� ����ĳ��Ʈ �߻�
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, m_maxDistance, m_raycastLayer))
        {
            // ����ĳ��Ʈ ����Ÿ� �׷��ݴϴ�.
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.red);

            // ����ĳ��Ʈ�� ���� ��ü�� ��Ƽ���� ��������
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // ĳ���Ϳ� ���� ���� ������ �Ÿ� ���
                Color materialColor = renderer.material.color;
                materialColor.a = m_transparencyValue;
                renderer.material.color = materialColor;
            }
        }
    }
}
