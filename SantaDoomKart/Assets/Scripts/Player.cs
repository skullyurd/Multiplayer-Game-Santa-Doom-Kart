using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 1.0f;
    [SerializeField] private GameObject _kart;

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = Vector3.MoveTowards(_kart.transform.position, _kart.transform.forward * speed, step);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = Vector3.MoveTowards(_kart.transform.position, _kart.transform.forward * speed * -1, step);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

        }
    }
}
