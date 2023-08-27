using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseTrigger : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")){
            BossManager.instance.BossAttackStart();
            gameObject.SetActive(false);
        }
    }
    #endregion
}
