using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongnamuAddForcer : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] float m_power = 20f;
    [SerializeField] float m_stunTime = 1f;
    [SerializeField] float m_destroyTime = 5f;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            playerRb.velocity = (new Vector3(1.0f,1.0f,0.0f)) * m_power;

            CharacterManager.instance.SetCanMove(false);

            StartCoroutine(nameof(SetStunTime));
        }
    }


    private IEnumerator SetStunTime()
    {
        yield return new WaitForSeconds(m_stunTime);

        while (!CharacterManager.instance.GetIsOnGround())
        {
            yield return null;
        }

        CharacterManager.instance.SetCanMove(true);
    }
    #endregion
}
