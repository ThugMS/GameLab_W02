using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    #region PublicVariables
    public CinemachineVirtualCamera m_camera;   //�� Zone���� Ȱ��ȭ�� VirtualCamera�Դϴ�.
    public static int s_superPriority = 300;    //Zone�� ������ ������ priority ���Դϴ�.
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
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���Դϴ�.
            m_camera.Priority = s_superPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���Դϴ�.
            m_camera.Priority = 0;
        }
    }
    #endregion
}
