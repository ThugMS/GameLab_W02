using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{   
    public static BossManager instance;
    #region PublicVariables
    public Boss m_boss;
    public GameObject m_bossPhaseStartTrigger;

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

    public void EnableBossPhaseStarter()
    {
        m_bossPhaseStartTrigger.SetActive(true);
    }

    public void StopPhase()
    {
        m_boss.InitSetting();
    }

    public void BossAttackStart()
    {
        Phase1Start();
    }

    public void Phase1Start()
    {
        if(m_phase1Complete == true)
        {
            Phase2Start();
        }
        else
        {
            m_boss.StartPhase1();
        }
    }

    public void Phase2Start()
    {
        if(m_phase2Complete == true)
        {

        }
    }

    public void SetPhase1Complete()
    {
        m_phase1Complete = true;
    }

    public void SetPhase2Complete()
    {
        m_phase2Complete = true;
    }
    #endregion

    #region PrivateMethod
    #endregion
}
