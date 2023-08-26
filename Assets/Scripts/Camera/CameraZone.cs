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
    #endregion

    #region PrivateVariables

    private Vector3 startPosition;

    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        //ó�� ������ ��ġ�� ����ϰ�, Enable �� �ش� ��ġ�� �����մϴ�.
        if (isInitPositionOnEnable)
        {
            startPosition = m_camera.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���Դϴ�.
            m_camera.Priority = s_superPriority;
            m_camera.gameObject.SetActive(true);
            //������� �ʴ� ZoneCamera�� Disable�մϴ�.


            if (isInitPositionOnEnable)
            {
                //�������� ó�� ������ ��ġ�� �ٽ� �̵��մϴ�.
                //�Ϻ� ����� ī�޶��, �ڵ����� �̵��ϴ� ��찡 �־�, �ٽ� Ȱ��ȭ ���� �� ��ġ�� �̻��� ��찡 �ֽ��ϴ�.
                m_camera.transform.position = startPosition;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��̾ ������ ������ ������ ī�޶��� �켱������ ���Դϴ�.
            m_camera.Priority = 0;
            m_camera.gameObject.SetActive(false);
            //������� �ʴ� ZoneCamera�� Disable�մϴ�.
        }
    }
    #endregion
}
