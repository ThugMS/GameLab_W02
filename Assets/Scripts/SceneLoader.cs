using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    public void LoadScene(int SceneNum)
    {
        SceneManager.LoadScene(SceneNum);
    }
}
