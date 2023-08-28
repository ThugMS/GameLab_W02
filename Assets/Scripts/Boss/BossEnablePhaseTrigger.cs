using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnablePhaseTrigger : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            BossManager.instance.EnableBossPhaseStarter();
            BossManager.instance.StopPhase();
        }
    }
    #endregion
}
