using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;

    [SerializeField] private int multiPlayerSceneIndex;
    [SerializeField] private int menuSceneIndex;

    [SerializeField] private int playerCount;
    [SerializeField] private int Roomsize;

    [SerializeField] private int minPlayerToStart;

    [SerializeField] private Text roomCountDisplay;
    [SerializeField] private Text timerToStartDisplay;

    [SerializeField] private bool readyToCountDown;
    [SerializeField] private bool readyToStart;
    [SerializeField] private bool startingGame;

    [SerializeField] private float timerToStartGame;
    [SerializeField] private float notFullGameTimer;
    [SerializeField] private float fullGameTimer;

    [SerializeField] private float maxWaitTime;
    [SerializeField] private float MaxFullGameWaitTime;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = MaxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {

        playerCount = PhotonNetwork.PlayerList.Length;
        Roomsize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + "/" + Roomsize;

        if (playerCount == Roomsize)
        {
            readyToStart = true;
        }
        
        if (playerCount > minPlayerToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }

        Debug.Log(PhotonNetwork.PlayerList);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if(playerCount <= 1)
        {
            ResetTimer();
        }

        if(readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if(timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = MaxFullGameWaitTime;
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiPlayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
