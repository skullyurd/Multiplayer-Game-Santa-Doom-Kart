using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stalkingBullet : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private bool hastarget;
    [SerializeField] private Rigidbody thisRigidBody;

    // Update is called once per frame
    void Update()
    {
        if (hastarget == true)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed);
        }
        if(hastarget == false)
        {
            thisRigidBody.AddForce(transform.forward * 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "AI" && hastarget == false)
        {
            target = other.gameObject.transform;
            hastarget = true;
        }
    }
}
