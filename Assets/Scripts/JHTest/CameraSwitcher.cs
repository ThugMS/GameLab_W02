using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject[] m_camera;
    private int m_cameranumber = 0;
    
    private void Update()
    {
        for (int i = 0; i < m_camera.Length; i++)
        {
            if (i == m_cameranumber)
            {
                m_camera[i].SetActive(true);
            }
            else
            {
                m_camera[i].SetActive(false);
            }
        }
    }

    public void OnCameraSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Ä«¸Þ¶ó :"+ (m_cameranumber+1));
            if (m_cameranumber < m_camera.Length-1)
            {
                m_cameranumber++;
            }
            else
            {
                m_cameranumber = 0;
            }
        }
        
    }

}