using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRedObstacle : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] float m_power = 20f;
    [SerializeField] float m_stunTime = 1f;
    [SerializeField] float m_destroyTime = 5f;
    #endregion

    #region PublicMethod
    private void OnEnable()
    {
        StartCoroutine(nameof(IE_DestroyTrigger));
    }
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            playerRb.velocity = transform.GetComponent<Rigidbody>().velocity * m_power;

            CharacterManager.instance.SetCanMove(false);

            StartCoroutine(nameof(SetStunTime));
        }        
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.layer == LayerMask.NameToLayer("Fence"))
    //    {
    //        GetComponent<Collider>().isTrigger = false;
    //    }
    //}

    private IEnumerator SetStunTime()
    {
        yield return new WaitForSeconds(m_stunTime);

        CharacterManager.instance.SetCanMove(true);
    }
    private IEnumerator IE_DestroyTrigger()
    {
        yield return new WaitForSeconds(m_destroyTime);

        Destroy(gameObject);
    }
    #endregion
}
