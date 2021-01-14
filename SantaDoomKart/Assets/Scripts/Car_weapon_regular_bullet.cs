using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_weapon_regular_bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rg_bullet;
    [SerializeField] private int speed;
    
    private void Start()
    {
        forwardBullet();
    }

    void forwardBullet()
    {
        Debug.Log(transform.forward);
        Vector3 shootHere = new Vector3(transform.forward.x,0.15f,transform.forward.z);
        rg_bullet.velocity = shootHere * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
           Debug.Log("Hit Something"); 
        }
    }
}
