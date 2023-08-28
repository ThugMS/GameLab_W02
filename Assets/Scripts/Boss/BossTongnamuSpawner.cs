using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTongnamuSpawner : MonoBehaviour
{
    #region PublicVariables
    [Header("About Test")]
    public bool isTest = true;
    public float m_spawnDelay = 10f;
    public float m_spawnInterval = 3f;

    [Header("About Spawn")]
    public ProjectileMove m_tongnamu;
    public Transform m_spawnPostion;

    [Header("About Tongnamu")]
    public Vector3 m_direction = new Vector3(0,0,-1f);
    public float m_speed = 15;
    public float m_accel = 0f;
    #endregion

    #region PrivateVariables
    [HideInInspector]public bool isSpawning = false;     //false가 되면 stop합니다.
    #endregion

    #region PublicMethod

    public IEnumerator IE_StartTongnamuPhase(float _delay, float _interval, Vector3 _direction, float _speed = 15, float _accel = 0f )
    {
        isSpawning = true;
        yield return new WaitForSeconds(_delay);
        while (isSpawning)
        {
            if (!isSpawning)
            {
                yield break;
            }

            ProjectileMove spawned = Instantiate(m_tongnamu, m_spawnPostion.position, m_tongnamu.transform.rotation);
            spawned.m_direction = _direction;
            spawned.m_speed = _speed;
            spawned.m_accel = _accel;

            yield return new WaitForSeconds(_interval);
        }
    }

    #endregion

    #region PrivateMethod
    private void OnEnable()
    {
        StartCoroutine(IE_StartTongnamuPhase(m_spawnDelay, m_spawnInterval, Vector3.back, m_speed, m_accel));

    }

    #endregion
}
