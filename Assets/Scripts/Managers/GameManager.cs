using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PublicVariables
    public static GameManager instance;
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

    public void StartBossStage()
    {
        CharacterManager.instance.ChangeCharacterCameraType(CAMERA_TYPE.BACK);
    }
    #endregion

    #region PrivateMethod
    #endregion
}
