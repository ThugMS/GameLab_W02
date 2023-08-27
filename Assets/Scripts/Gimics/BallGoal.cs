using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoal : MonoBehaviour
{
    #region PublicVariables
    public GameObject m_balpan;
    public string m_goalName;
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if(m_goalName.Equals(other.gameObject.name))
            {
                m_balpan.SetActive(true);
                other.transform.position = transform.position;
                other.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    #endregion
}
