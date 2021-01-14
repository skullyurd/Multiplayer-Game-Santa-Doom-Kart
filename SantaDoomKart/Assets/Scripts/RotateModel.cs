using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    [SerializeField] private bool powerup;
    [SerializeField] private bool activeAbility;
    private float degreesPerSecond = 15.0f;
    private float amplitude = 0.5f;
    private float frequency = 1f;
    private Vector3 posOffset;
    private Vector3 tempPos;

    void Start () {
        posOffset = new Vector3();
        tempPos = new Vector3();
        posOffset = transform.position;
    }


    // Update is called once per frame
    void Update()
    {

        if (activeAbility)
        {
            transform.RotateAround(transform.parent.parent.position, transform.parent.parent.up, 40 * Time.deltaTime);
        }
        else
        {
            transform.Rotate (0,50*Time.deltaTime,0); //rotates 50 degrees per second around z axis

            if (powerup)
            {
                // Float up/down with a Sin()
                tempPos = posOffset;
                tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
                transform.position = tempPos;
            }
        }
    }
}
