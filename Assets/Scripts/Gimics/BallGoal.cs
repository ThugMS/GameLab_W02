using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoal : MonoBehaviour
{
    #region PublicVariables
    public GameObject balpan;
    public string goalName;
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
            if(goalName.Equals(other.gameObject.name))
            {
                balpan.SetActive(true);
                other.transform.position = transform.position;
                other.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    #endregion
}
