using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    #region PublicVariables
    public CinemachineVirtualCamera m_camera;   //�� Zone���� Ȱ��ȭ�� VirtualCamera�Դϴ�.
    public static int s_superPriority = 300;    //Zone�� ������ ������ priority ���Դϴ�.

    [Header("Remember First position of Camera")]
    public bool isInitPositionOnEnable = true;  //Enable ��, ó�� ������ ��ġ�� �����̵����� ����. True: OnEnable���� ��ġ�� �ʱ� ��ġ�� �ڵ� ������

    [Header("Set Player's Camera Type")]
    public CAMERA_TYPE m_cameraType = CAMERA_TYPE.FIXED;        //�ش� Zone���� ����� ī�޶��� type�� �����մϴ�.
    public Vector3 m_forwardDirectionOnFixedCameraType = new Vector3(0, 0, 1);       //�ش� Zone���� ����� ī�޶� Type�� Fixed���, Fixed������ �� ������ �����մϴ�.
    #endregion

    #region PrivateVariables

    private Vector3 m_startPosition;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //ó�� ������ ��ġ�� ����ϰ�, Enable �� �ش� ��ġ�� �����մϴ�.
        if (isInitPositionOnEnable)
        {
            m_startPosition = m_camera.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���̰�, Ȱ��ȭ�մϴ�.
            m_camera.gameObject.SetActive(true);

            CameraManager.Instance.SetCamera(m_camera, m_cameraType,m_forwardDirectionOnFixedCameraType);
            //CharacterManager.instance.ChangeCharacterCameraType(m_cameraType, m_forwardDirectionOnFixedCameraType);


            if (isInitPositionOnEnable)
            {
                //�������� ó�� ������ ��ġ�� �ٽ� �̵��մϴ�.
                //�Ϻ� ����� ī�޶��, �ڵ����� �̵��ϴ� ��찡 �־�, �ٽ� Ȱ��ȭ ���� �� ��ġ�� �̻��� ��찡 �ֽ��ϴ�.
                m_camera.transform.position = m_startPosition;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���߰�, ��Ȱ��ȭ�մϴ�.
            m_camera.gameObject.SetActive(false);

            CameraManager.Instance.RemoveCamera(m_camera);
            //������� �ʴ� ZoneCamera�� Disable�մϴ�.
        }
    }
    #endregion
}
