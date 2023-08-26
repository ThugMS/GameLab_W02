using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    #region PublicVariables
    public CinemachineVirtualCamera m_camera;   //이 Zone에서 활성화할 VirtualCamera입니다.
    public static int s_superPriority = 300;    //Zone에 들어오면 지정될 priority 값입니다.

    [Header("Remember First position of Camera")]
    public bool isInitPositionOnEnable = true;  //Enable 시, 처음 지정한 위치로 순간이동할지 여부. True: OnEnable에서 위치를 초기 위치로 자동 지정함

    [Header("Set Player's Camera Type")]
    public CAMERA_TYPE m_cameraType = CAMERA_TYPE.FIXED;        //해당 Zone에서 사용할 카메라의 type을 설정합니다.
    public Vector3 m_forwardDirectionOnFixedCameraType = new Vector3(0, 0, 1);       //해당 Zone에서 사용할 카메라 Type이 Fixed라면, Fixed에서의 앞 방향을 결정합니다.
    #endregion

    #region PrivateVariables

    private Vector3 m_startPosition;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //처음 지정한 위치를 기억하고, Enable 시 해당 위치로 지정합니다.
        if (isInitPositionOnEnable)
        {
            m_startPosition = m_camera.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //플레이어가 들어오면 나에게 지정된 카메라의 우선순위를 높이고, 활성화합니다.
            m_camera.gameObject.SetActive(true);

            CameraManager.Instance.SetCamera(m_camera, m_cameraType,m_forwardDirectionOnFixedCameraType);
            //CharacterManager.instance.ChangeCharacterCameraType(m_cameraType, m_forwardDirectionOnFixedCameraType);


            if (isInitPositionOnEnable)
            {
                //포지션을 처음 지정한 위치로 다시 이동합니다.
                //일부 버츄얼 카메라는, 자동으로 이동하는 경우가 있어, 다시 활성화 했을 때 위치가 이상한 경우가 있습니다.
                m_camera.transform.position = m_startPosition;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //플레이어가 나가면 나에게 지정된 카메라의 우선순위를 낮추고, 비활성화합니다.
            m_camera.gameObject.SetActive(false);

            CameraManager.Instance.RemoveCamera(m_camera);
            //사용하지 않는 ZoneCamera는 Disable합니다.
        }
    }
    #endregion
}
