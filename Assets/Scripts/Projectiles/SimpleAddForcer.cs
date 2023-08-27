using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAddForcer : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
    #endregion
}
