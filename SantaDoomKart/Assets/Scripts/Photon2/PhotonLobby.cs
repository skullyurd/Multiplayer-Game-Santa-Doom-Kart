using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject startButton;
    public GameObject connecting;

    [SerializeField] private InputField inputField;

    private void Awake()
    {
        lobby = this;
    }
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        connecting.SetActive(false);
        startButton.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.LocalPlayer.NickName = inputField.text;
        startButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }
    
    public void OnCancelButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnJoinRandomFailed(short returncode, string message)
    {
        Debug.Log("Tried to join a random room but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = UnityEngine.Random.Range(0, 100000);
        RoomOptions roomOps = new RoomOptions(){IsVisible = true,IsOpen = true,MaxPlayers = 8};
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       Debug.Log("Tried to create a new room but failed, there must be a room with te same name");
       CreateRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created a room because there were none available");
    }
    

    public override void OnLeftRoom()
    {
        startButton.SetActive(true);
    }
    
}
