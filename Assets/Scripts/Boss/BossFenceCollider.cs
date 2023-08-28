using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFenceCollider : MonoBehaviour
{
    #region PublicVariables
    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    private void OnCollisionExit(Collision collision)
    {
        
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            collision.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("emfdjdha");
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    #endregion
}
