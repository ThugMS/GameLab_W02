using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss : MonoBehaviour
{
    #region PublicVariables
    public bool m_isPhase1 = false;
    public bool m_isAttackThrow = false;
    public bool m_isChargeAttack = false;
    public bool m_canMakeObs = true;
    public bool m_isChangePos = false;
    public float m_chargePosOdd = -70;
    public float m_chargePosEven = 35;
    #endregion

    #region PrivateVariables
    [SerializeField] private Vector3 m_initPosition;
    [SerializeField] private Quaternion m_initRotation;

    [SerializeField] private GameObject m_obstacleBlack;
    [SerializeField] private GameObject m_obstacleRed;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private GameObject m_blockCollider;
    [SerializeField] private GameObject m_savePoint;

    [SerializeField] private GameObject m_idleEye;
    [SerializeField] private GameObject m_AngryEye;

    [SerializeField] private float m_attackRangeX = 40f;
    [SerializeField] private float m_attackRangeY = 15f;
    [SerializeField] private float m_attackZOdd = 20;
    [SerializeField] private float m_attackZEven = -70f;
    [SerializeField] private float m_attackPower = 10f;
    [SerializeField] private int m_attackDirection = -1;
    [SerializeField] private float m_attackTime = 10f;
    [SerializeField] private int m_attackCnt = 0;

    [SerializeField] private float m_attackCoolTime = 0.5f;
    [SerializeField] private float m_phase2CoolTime = 3f;
    [SerializeField] private bool m_isChange = false;
    [SerializeField] private float m_chargeDis = 100f;
    #endregion

    #region PublicMethod
    public void InitSetting()
    {
        StopAllCoroutines();

        transform.position = m_initPosition;
        transform.rotation = m_initRotation;

        m_isPhase1 = false;
        m_isAttackThrow = false;
        m_isChargeAttack = false;
        m_canMakeObs = false;
        m_isChangePos = false;

        m_attackDirection = -1;

        m_idleEye.SetActive(true);
        m_AngryEye.SetActive(false);
        CharacterManager.instance.SetSavePoint(m_savePoint);
    }

    public void StartPhase1()
    {   
        m_isPhase1 = true;
        m_isAttackThrow = true;
        m_canMakeObs = true;
        StartCoroutine(nameof(IE_ChangeChargeAttack));
    }

    private void Start()
    {
        //InitPhase1();
        m_initPosition = transform.position;
        m_initRotation = transform.rotation;
    }

    private void Update()
    {
        if (m_isAttackThrow == true && m_canMakeObs == true)
        {
            SpawnObstacleBlack(3);
            SpawnObstacleRed();
            m_canMakeObs = false;

            StartCoroutine(nameof(IE_CheckAttackCoolTime));
        }
        
        if(m_isChargeAttack == true)
        {
            StartCoroutine(nameof(IE_WaitPhase2));
        }

        if(m_isChangePos == false)
        {
            m_rigidbody.velocity = Vector3.zero;
        }

        if(m_attackCnt == 4)
        {
            m_isPhase1 = false;
            StopAllCoroutines();
        }
    }
    #endregion

    #region PrivateMethod
    private void SpawnObstacleBlack(int _cnt) 
    {   
        for(int i = 0; i < _cnt; i++)
        {
            float rangeX = Random.Range(-m_attackRangeX, m_attackRangeX);
            float rangeY = Random.Range(5, m_attackRangeY);
            float angularV = Random.Range(1f, 20f);

            GameObject obj;

            if (m_attackDirection < 0)
            {
                obj = Instantiate(m_obstacleBlack, new Vector3(rangeX, rangeY, m_attackZOdd), Quaternion.identity);
            }
            else
            {
                obj = Instantiate(m_obstacleBlack, new Vector3(rangeX, rangeY, m_attackZEven), Quaternion.identity);
            }
            

            obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, m_attackDirection * m_attackPower);
            obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(angularV, angularV, angularV);
        }
    }

    private void SpawnObstacleRed()
    {
        float rangeX = Random.Range(-m_attackRangeX, m_attackRangeX);
        float rangeY = Random.Range(5, m_attackRangeY);
        float angularV = Random.Range(1f, 20f);
        GameObject obj;

        if (m_attackDirection < 0)
        {
            obj = Instantiate(m_obstacleRed, new Vector3(rangeX, rangeY, m_attackZOdd), Quaternion.identity);
        }
        else
        {
            obj = Instantiate(m_obstacleRed, new Vector3(rangeX, rangeY, m_attackZEven), Quaternion.identity);
        }

        obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, m_attackDirection * m_attackPower);
        obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(angularV, angularV, angularV);
    }

    private IEnumerator IE_CheckAttackCoolTime()
    {
        yield return new WaitForSeconds(m_attackCoolTime);

        m_canMakeObs = true;
    }

    private IEnumerator IE_WaitPhase2()
    {   
        m_idleEye.SetActive(false);
        m_AngryEye.SetActive(true);
        m_isChargeAttack = false;
        yield return new WaitForSeconds(m_phase2CoolTime);

        ChangePosition();
    }

    private IEnumerator IE_ChangeChargeAttack()
    {
        yield return new WaitForSeconds(m_attackTime);

        m_isAttackThrow = false;
        m_isChargeAttack = true;
    }

    private void ChangePosition()
    {
        m_blockCollider.SetActive(false);
        m_rigidbody.velocity = new Vector3(0, 0, 200 * m_attackDirection);
        m_isChangePos = true;
    }

    public void CheckPositon()
    {   
        if (m_attackDirection < 0)
        {
            if (transform.position.z <= m_chargePosOdd)
            {
                m_rigidbody.velocity = Vector3.zero;
                m_rigidbody.rotation = Quaternion.Euler(0, 180, 0);
                m_isChangePos = false;
                m_attackDirection *= -1;
                m_blockCollider.SetActive(true);
                m_idleEye.SetActive(true);
                m_AngryEye.SetActive(false);
                Debug.Log("yes");
            }
            m_attackCnt++;
            
            if (m_attackCnt < 4)
            {
                StartCoroutine(nameof(IE_ChangeChargeAttack));
                m_isAttackThrow = true;
                m_canMakeObs = true;
            }
            else
            {
                BossManager.instance.SetPhase1Complete();
            }

        }
        else
        {
            if (transform.position.z >= m_chargePosEven)
            {
                m_rigidbody.velocity = Vector3.zero;
                m_rigidbody.rotation = Quaternion.Euler(0, 0, 0);
                m_isChangePos = false;
                m_attackDirection *= -1;
                m_blockCollider.SetActive(true);
                m_idleEye.SetActive(true);
                m_AngryEye.SetActive(false);
            }
            m_attackCnt++;

            if (m_attackCnt < 4)
            {
                StartCoroutine(nameof(IE_ChangeChargeAttack));
                m_isAttackThrow = true;
                m_canMakeObs = true;

            }
            else
            {
                BossManager.instance.SetPhase1Complete();
            }
        }
    }
    #endregion
}
