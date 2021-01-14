using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DestroyMeOverTime : MonoBehaviour
{   
    IEnumerator DestroyTime(float time)
    {
        yield return new WaitForSeconds(time);
       Destroy(gameObject);
    }

}
