using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongnamuAddForcer : MonoBehaviour
{
    #region PublicVariables
    public Vector3 m_direction = new Vector3(1f, 1f, 0f);
    [HideInInspector] public bool useStun = true;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            playerRb.velocity = m_direction * m_power;


            if (useStun)
            {
                CharacterManager.instance.SetCanMove(false);
                StartCoroutine(nameof(SetStunTime));
            }
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
