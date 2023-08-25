using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_targetPlayer;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    private void Update()
    {
        gameObject.transform.position = m_targetPlayer.transform.position;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
