using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_toSpawn;
    public Transform m_spawnPosition;
    public float delay;
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
            Instantiate(m_toSpawn, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            yield return new WaitForSeconds(delay);
        }
    }
    #endregion
}
