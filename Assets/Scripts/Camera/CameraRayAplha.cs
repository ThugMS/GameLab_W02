using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayAplha : MonoBehaviour
{
    public LayerMask m_raycastLayer;
    public float m_maxDistance = 100f;
    public float m_transparencyValue = 0.5f;
    private Transform m_character; // 캐릭터의 Transform을 할당하는 변수

    private RaycastHit hit;

    private void Start()
    {
        m_character = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // 카메라 위치에서 캐릭터 방향으로 레이캐스트 발사
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, m_maxDistance, m_raycastLayer))
        {
            // 레이캐스트 디버거를 그려줍니다.
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.red);

            // 레이캐스트로 맞은 객체의 머티리얼 가져오기
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 캐릭터와 맞은 지점 사이의 거리 계산
                Color materialColor = renderer.material.color;
                materialColor.a = m_transparencyValue;
                renderer.material.color = materialColor;
            }
        }
    }
}
