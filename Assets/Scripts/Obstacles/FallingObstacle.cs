using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    public float m_fadeDuration = 0.5f; // ���̵� �ƿ� ���� �ð�
    private Material m_objectMaterial; // ������Ʈ�� ���׸���
    private Rigidbody m_rigidbody;

    private void Start()
    {
        // ������Ʈ�� ���׸��� ��������
        m_objectMaterial = GetComponent<Renderer>().material;
        m_rigidbody = GetComponent<Rigidbody>();
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Ground �浹�� ù ��° �ڽ� ������Ʈ Ȯ�� �� ����
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            StartCoroutine(FadeOutAndDestroy());
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Ground �浹�� ù ��° �ڽ� ������Ʈ Ȯ�� �� ����
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            m_rigidbody.isKinematic = true;
            m_rigidbody.velocity = Vector3.zero;
            StartCoroutine(FadeOutAndDestroy());
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterManager.instance.Respawn();
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
            //newColor.a = alpha;
            m_objectMaterial.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
