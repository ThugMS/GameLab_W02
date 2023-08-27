using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageTrigger : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            GameManager.instance.StartBossStage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.EndBossStage();
        }
    }
    #endregion
}
