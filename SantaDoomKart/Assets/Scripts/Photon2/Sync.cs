using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class Sync : MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable
{
    private Vector3 realPosition = Vector3.zero;
    private Quaternion realRotation;
    private Vector3 sendPosition = Vector3.zero;
    private Quaternion sendRotation;
 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) {//this is your information you will send over the network
            sendPosition = this.transform.position;
            sendRotation = this.transform.rotation;
            stream.Serialize (ref sendPosition);//im pretty sure you have to use ref here, check that
            stream.Serialize (ref sendRotation);//same with the ref here...
        }else if(stream.IsReading){//this is the information you will recieve over the network
            stream.Serialize (ref realPosition);//Vector3 position
            stream.Serialize (ref realRotation);//Quaternion postion
        }
    }
    void Update()
    {
        if(!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(this.transform.position, realPosition, Time.deltaTime * 15);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, realRotation, Time.deltaTime * 30);
        }
    }
}
