using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2 : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_pattern1;
    public GameObject m_pattern2;
    public GameObject m_pattern3;
    #endregion

    #region PrivateVariables
    [SerializeField] private float m_pattern1CoolTime;
    [SerializeField] private float m_pattern2CoolTime;
    [SerializeField] private float m_pattern3CoolTime;
    #endregion

    #region PublicMethod
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPhase2();
        }
    }

    public void StartPhase2()
    {
        StartCoroutine(nameof(IE_Phase2));
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Phase2()
    {
        m_pattern1.SetActive(true);
        yield return new WaitForSeconds(m_pattern1CoolTime);
        m_pattern2.SetActive(true);
        yield return new WaitForSeconds(m_pattern2CoolTime);
        m_pattern3.SetActive(true);
        yield return new WaitForSeconds(m_pattern3CoolTime);

        m_pattern1.SetActive(false);
        m_pattern2.SetActive(false);
        m_pattern3.SetActive(false);

        Debug.Log("End");
    }
    #endregion
}
