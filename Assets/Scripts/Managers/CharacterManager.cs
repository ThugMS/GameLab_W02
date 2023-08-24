using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region PublicVariables
    public static CharacterManager instance;

    public bool m_canMove = true;
    public bool m_isDash = false;
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

    public bool GetCanMove()
    {
        return m_canMove;
    }

    public void SetCanMove(bool _value)
    {
        m_canMove = _value;
    }

    public bool GetIsDesh()
    {
        return m_isDash;
    }

    public void SetIsDesh(bool _value)
    {
        m_isDash = _value;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
