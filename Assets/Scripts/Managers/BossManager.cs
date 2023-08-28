using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BossManager : MonoBehaviour
{   
    public static BossManager instance;
    #region PublicVariables
    public Boss m_boss;
    public BossPhase2 m_bossPhase2;
    public GameObject m_bossPhaseStartTrigger;
    public Rigidbody m_body;
    public Image m_crossFader;

    public bool m_phase1Complete = false;
    public bool m_phase2Complete = false;
    #endregion

    #region PrivateVariables

    #endregion

    #region PublicMethod
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
    }

    private void Start()
    {
        m_body = m_boss.GetComponent<Rigidbody>();
    }

    public void EnableBossPhaseStarter()
    {
        m_bossPhaseStartTrigger.SetActive(true);
    }

    public void StopPhase()
    {
        m_boss.InitSetting();
        m_bossPhase2.InitPhase2();
    }

    public void BossAttackStart()
    {
        Phase1Start();
    }

    public void Phase1Start()
    {   if(m_phase1Complete == true && m_phase2Complete == true)
        {
            BossKill();
        }
        else if(m_phase1Complete == true)
        {
            Phase2Start();
        }
        else
        {
            m_boss.StartPhase1();
        }
    }

    public void BossKill()
    {
        m_body.mass = 1;
        m_boss.m_isChangePos = true;
        m_body.velocity = (new Vector3(0, 50, 100));
        m_body.angularVelocity = new Vector3(0, 10, 10);
        StartCoroutine(nameof(IE_EndTum));
    }

    public void Phase2Start()
    {
        m_bossPhase2.StartPhase2();
    }

    public void SetPhase1Complete()
    {
        m_phase1Complete = true;

        StartCoroutine(nameof(IE_Phase2Tum));
    }

    public void SetPhase2Complete()
    {
        m_phase2Complete = true;
        EnableBossPhaseStarter();
    }
    #endregion

    #region PrivateMethod
    private IEnumerator IE_Phase2Tum()
    {
        yield return new WaitForSeconds(5f);
        Phase2Start();
    }

    private IEnumerator IE_EndTum()
    {
        yield return new WaitForSeconds(2f);
        Color startColor = m_crossFader.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // 알파값을 1로 설정합니다.
        float elapsedTime = 0.0f;
        float duration = 2.0f; // 애니메이션 지속 시간

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            m_crossFader.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_crossFader.color = endColor;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
    #endregion
}
