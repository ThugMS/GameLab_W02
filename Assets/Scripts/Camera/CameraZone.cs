using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    #region PublicVariables
    public CinemachineVirtualCamera m_camera;   //이 Zone에서 활성화할 VirtualCamera입니다.
    public static int s_superPriority = 300;    //Zone에 들어오면 지정될 priority 값입니다.
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
            //플레이어가 들어오면 나에게 지정된 카메라의 우선순위를 높입니다.
            m_camera.Priority = s_superPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //플레이어가 나가면 나에게 지정된 카메라의 우선순위를 높입니다.
            m_camera.Priority = 0;
        }
    }
    #endregion
}
