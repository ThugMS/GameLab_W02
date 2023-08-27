using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRespawnTrigger : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] GameObject m_savePoint;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterManager.instance.SetSavePoint(m_savePoint);
        }
    }
    #endregion
}
