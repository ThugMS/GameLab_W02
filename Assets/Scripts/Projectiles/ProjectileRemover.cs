using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRemover : MonoBehaviour
{
    #region PublicVariables
    public string m_tagToRemove = "Projectile";
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_tagToRemove))
        {
            Destroy(other.gameObject);
        }

    }

    #endregion
}
