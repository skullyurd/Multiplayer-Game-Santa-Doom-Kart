using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject delayStartButton;
    [SerializeField] private GameObject delayCancelButton;
    [SerializeField] private int roomsize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartButton.SetActive(true);
    }

    public void DelayStart()
    {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Delay Started");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomsize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... try again");
        CreateRoom();
    }

    public void DelayCancel()
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(transform);
        PhotonNetwork.LeaveRoom();
    }
}
