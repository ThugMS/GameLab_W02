using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase2 : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_pattern1;
    public GameObject m_pattern2;
    public GameObject m_pattern3;
    public GameObject m_cameraZone;
    #endregion

    #region PrivateVariables
    [SerializeField] private float m_pattern1CoolTime;
    [SerializeField] private float m_pattern2CoolTime;
    [SerializeField] private float m_pattern3CoolTime;
    #endregion

    #region PublicMethod

    public void StartPhase2()
    {
        StartCoroutine(nameof(IE_Phase2));
    }

    public void InitPhase2()
    {   
        m_cameraZone.SetActive(false);
        m_pattern1.SetActive(false);
        m_pattern2.SetActive(false);
        m_pattern3.SetActive(false);

        StopAllCoroutines();
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Phase2()
    {
        m_cameraZone.SetActive(true);
        m_pattern1.SetActive(true);
        yield return new WaitForSeconds(m_pattern1CoolTime);
        m_pattern2.SetActive(true);
        yield return new WaitForSeconds(m_pattern2CoolTime);
        m_pattern3.SetActive(true);
        yield return new WaitForSeconds(m_pattern3CoolTime);

        m_pattern1.SetActive(false);
        m_pattern2.SetActive(false);
        m_pattern3.SetActive(false);
        m_cameraZone.SetActive(false);
        BossManager.instance.SetPhase2Complete();
    }
    #endregion
}
