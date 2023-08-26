using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    public float m_fadeDuration = 0.5f; // 페이드 아웃 지속 시간
    private Material m_objectMaterial; // 오브젝트의 머테리얼

    private void Start()
    {
        // 오브젝트의 머테리얼 가져오기
        m_objectMaterial = GetComponent<Renderer>().material;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Ground 충돌시 첫 번째 자식 오브젝트 확인 및 제거
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float startAlpha = m_objectMaterial.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < m_fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / m_fadeDuration);

            Color newColor = m_objectMaterial.color;
            newColor.a = alpha;
            m_objectMaterial.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
