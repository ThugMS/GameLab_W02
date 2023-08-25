using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    #region PublicVariables
    public CinemachineVirtualCamera myCamera;
    public int mainPriority = 300;
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
            myCamera.Priority = mainPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myCamera.Priority = 0;
        }
    }
    #endregion
}
