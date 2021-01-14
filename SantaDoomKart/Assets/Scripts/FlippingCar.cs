using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingCar : MonoBehaviour
{

    [SerializeField] private GameObject playerObject;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor") //Tag needs to go, need to find an alternative
        {
            flipCarOnWheels();
        }
    }

    void flipCarOnWheels()
    {
        playerObject.transform.position += new Vector3(0, 1f, 0);
        playerObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
