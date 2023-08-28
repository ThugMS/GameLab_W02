using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERA_TYPE {
    TOP, SIDE, BACK, FIXED,
}


public class CharacterManager : MonoBehaviour
{
    #region PublicVariables
    public static CharacterManager instance;

    public CinemachineVirtualCamera m_virtualCamera;
    public CinemachineComponentBase m_cameraBase;
    public GameObject m_character;
    public GameObject m_savePoint;

    public float m_stunTime = 1f;
    public bool m_canMove = true;

    public bool m_isStun = false;
    public bool m_isMove = false;
    public bool m_isDash = false;
    public bool m_isJump = false;
    public bool m_isOnGround = false;
    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        m_cameraBase = m_virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
    }

    public bool GetCanMove()
    {
        return m_canMove;
    }

    public void SetCanMove(bool _value)
    {   
        if(_value == true && m_isStun == true)
        {
            return;
        }

        m_canMove = _value;
    }

    public bool GetIsMove()
    {
        return m_isMove;
    }

    public void SetIsMove(bool _value)
    {
        m_isMove = _value;
    }

    public bool GetIsDash()
    {
        return m_isDash;
    }

    public void SetIsDash(bool _value)
    {   
        if(_value == false)
        {
            m_canMove = true;
        }
        m_isDash = _value;
    }

    public bool GetIsJump()
    {
        return m_isJump;
    }

    public void SetIsJump(bool _value)
    {
        m_isJump = _value;
    }

    public bool GetIsOnGround()
    {
        return m_isOnGround;
    }

    public void SetIsOnGround(bool _value)
    {
        m_isOnGround = _value;
    }

    public void ChangeCameraDistance(float _dis)
    {
        (m_cameraBase as Cinemachine3rdPersonFollow).CameraDistance = _dis;
    }

    public void ChangeCharacterCameraType(CAMERA_TYPE _type, Vector3 _frontDirection = new Vector3())
    {

        m_character.GetComponent<CharacterMovement>().SetCameraType(_type, _frontDirection);
    }

    public void SetSavePoint(GameObject _point)
    {
        m_savePoint = _point;
    }

    public void Respawn()
    {
        m_character.transform.position = m_savePoint.transform.position;
    }

    public void Stun()
    {
        SetCanMove(false);
        m_isStun = true;

        StopAllCoroutines();
        StartCoroutine(nameof(IE_SetStunTime));
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_SetStunTime()
    {
        yield return new WaitForSeconds(m_stunTime);

        m_isStun = false;
        CharacterManager.instance.SetCanMove(true);
        
    }
    #endregion
}
