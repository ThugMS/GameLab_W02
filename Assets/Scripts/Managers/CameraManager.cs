using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    #region PublicVariables
    public static CameraManager Instance { get { return instance; } }
    public CharacterMovement player;
    #endregion

    #region PrivateVariables
    private class CameraBundle
    {
        public CinemachineVirtualCamera m_vcam;
        public CAMERA_TYPE m_type;
        public Vector3 m_forwardDirectionOnFixedMove;
        public CameraBundle(CinemachineVirtualCamera vcam, CAMERA_TYPE type, Vector3 forwardDirectionOnFixedMove = new Vector3())
        {
            m_vcam = vcam;
            m_type = type;
            m_forwardDirectionOnFixedMove = forwardDirectionOnFixedMove;
        }

        public static bool operator ==(CameraBundle _bundle, CinemachineVirtualCamera _vcam)
        {
            return _bundle.m_vcam == _vcam;
        }
        public static bool operator !=(CameraBundle _bundle, CinemachineVirtualCamera _vcam)
        {
            return _bundle.m_vcam == _vcam;
        }
    }

    private static CameraManager instance;

    private CinemachineVirtualCamera m_mainVcam;          //��� �ٲ�
    [SerializeField] private CinemachineVirtualCamera m_basicVcam;       //�÷��̾ �Ѵ� �⺻���� ī�޶�
    private List<CameraBundle> m_zoneVcams = new List<CameraBundle>();

    private int m_mainPriority = 100;
    private int m_subPriority = 50;
    #endregion

    #region PublicMethod
    public void SetCamera(CinemachineVirtualCamera _vcam, CAMERA_TYPE _type, Vector3 _forwardDirectionOnFixedMove = new Vector3())
    {
        m_zoneVcams.Add(new CameraBundle(_vcam, _type, _forwardDirectionOnFixedMove));
        m_mainVcam.Priority = m_subPriority;
        m_mainVcam = _vcam;
        m_mainVcam.Priority = m_mainPriority;
        player.SetCameraType(_type, _forwardDirectionOnFixedMove);
    }

    public CinemachineVirtualCamera GetMainCamera()
    {
        return m_mainVcam;
    }

    public void RemoveCamera(CinemachineVirtualCamera _vcam)
    {
        m_zoneVcams.RemoveAll(x => x==_vcam);
        if (_vcam == m_mainVcam)
        {
            if (m_zoneVcams.Count > 0)
            {
                //ZoneCamera�� �ִٸ�, ���� �������� �߰��� ī�޶�� ���
                m_mainVcam = m_zoneVcams[0].m_vcam;
                m_mainVcam.Priority = m_mainPriority;
                player.SetCameraType(m_zoneVcams[0].m_type, m_zoneVcams[0].m_forwardDirectionOnFixedMove);
            }
            else
            {
                //ZoneCamera�� ���ٸ�, �⺻ ķ�� ���
                m_mainVcam = m_basicVcam;
                m_mainVcam.Priority = m_mainPriority;
                player.SetCameraType(CAMERA_TYPE.BACK);
            }
            m_mainVcam.Priority = m_mainPriority;
        }
    }
    public void ResetCameraList()
    {
        m_zoneVcams.Clear();
        m_mainVcam = m_basicVcam;
        m_mainVcam.Priority = m_mainPriority;
        player.SetCameraType(CAMERA_TYPE.BACK);
    }
    #endregion

    #region PrivateMethod

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        ResetCameraList();
    }

    #endregion
}