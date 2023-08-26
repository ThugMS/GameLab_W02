using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlackObstacle : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] float m_power = 20f;
    [SerializeField] float m_stunTime = 1f;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(transform.GetComponent<Rigidbody>().velocity / m_power);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            
            playerRb.velocity = transform.GetComponent<Rigidbody>().velocity / m_power;

            CharacterManager.instance.SetCanMove(false);

            StartCoroutine(nameof(SetStunTime));
        }        
    }

    private IEnumerator SetStunTime()
    {
        yield return new WaitForSeconds(m_stunTime);

        CharacterManager.instance.SetCanMove(true);
    }
    #endregion
}
