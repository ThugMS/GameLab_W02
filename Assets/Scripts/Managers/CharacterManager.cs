using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region PublicVariables
    public static CharacterManager instance;

    public bool m_canMove = true;
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
    #endregion

    #region PrivateMethod
    #endregion
}
