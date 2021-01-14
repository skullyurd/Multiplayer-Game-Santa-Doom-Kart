using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnlyLocalCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject namePlate;
    [SerializeField] private GameObject localCamera;

    
    private void Update()
    {
        namePlate.transform.LookAt(Camera.main.transform.position);
        namePlate.transform.Rotate(0,180,0);
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            canvas.SetActive(true);
            localCamera.SetActive(true);
        }
        else
        {
            namePlate.SetActive(true);
            namePlate.GetComponentInChildren<TextMesh>().text = photonView.Owner.NickName;
        }
    }
}
