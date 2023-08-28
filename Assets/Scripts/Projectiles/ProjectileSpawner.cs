using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_toSpawn;
    public Transform m_spawnPosition;
    public float m_delay;
    public Vector3 euler = new Vector3(-90f, 0f, 0f);
    public bool useAddForce = false;
    public Vector3 m_force = new Vector3(1f, 0f, 0f);
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void Start()
    {
        StartCoroutine(IE_Spawn());
    }

    private IEnumerator IE_Spawn()
    {
        while (true)
        {
            GameObject spawned = Instantiate(m_toSpawn, transform.position, Quaternion.Euler(euler));
            if (useAddForce)
            {
                Rigidbody body = spawned.GetComponent<Rigidbody>();
                body.velocity = m_force;
                body.angularVelocity = new Vector3(100f, 100f, 100f);
            }
            yield return new WaitForSeconds(m_delay);
        }
    }
    #endregion
}
