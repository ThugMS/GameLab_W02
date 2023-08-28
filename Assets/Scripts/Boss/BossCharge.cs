using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharge : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    [SerializeField] private float m_power = 20f;
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterManager.instance.Respawn();
            BossManager.instance.EnableBossPhaseStarter();
            BossManager.instance.StopPhase();
        }
    }
    #endregion
}
