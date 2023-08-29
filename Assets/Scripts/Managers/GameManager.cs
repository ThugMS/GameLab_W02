using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        CharacterManager.instance.m_virtualCamera.Priority = 400;
    }

    public void EndBossStage()
    {
        CharacterManager.instance.m_virtualCamera.Priority = 0;
    }

    public void StartStage2()
    {
        CharacterManager.instance.ChangeCharacterCameraType(CAMERA_TYPE.TOP);
    }
    #endregion

    #region PrivateMethod
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    #endregion
}
