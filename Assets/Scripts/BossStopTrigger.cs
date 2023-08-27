using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStopTrigger : MonoBehaviour
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
        if(other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Debug.Log("okay");
            other.GetComponent<Boss>().CheckPositon();
        }
    }
    #endregion
}
