using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SyncedTimerLobby : MonoBehaviourPunCallbacks
{
    
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    private float actualTimeForDisplay;
    
    ExitGames.Client.Photon.Hashtable CustomeValue;
    
    [SerializeField] private float timerLength;
    [SerializeField] private Text timeText;
    [SerializeField] private Image circle;
    [SerializeField] private PhotonRoom m_PhotonRoom;
    

    public void StartTimer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTimeLobby", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTimeLobby"].ToString());
            startTimer = true;
        }
    }

    public void CancelTimer()
    {
        startTimer = false;
        actualTimeForDisplay = timerLength;
    }
    
    
    void Update()
    {
        if (!startTimer) return;
        
        timerIncrementValue = PhotonNetwork.Time - startTime;
        actualTimeForDisplay = timerLength - (float)timerIncrementValue;
        
        UpdateUITimer();
        
        if (actualTimeForDisplay <= 0 && PhotonNetwork.IsMasterClient)
        {
            m_PhotonRoom.StartGame();
            Destroy(this);
        }

        if (actualTimeForDisplay <=0)
        {
            Destroy(this);
        }
    }

    void UpdateUITimer()
    {
        string seconds = (actualTimeForDisplay % 59).ToString("f0");
        string minutes = ((int)actualTimeForDisplay / 59).ToString();

        fillLoading();
        timeText.text = minutes + ":" + seconds;
    }
    
    void fillLoading()
    {
        float fill = (float)actualTimeForDisplay/timerLength;
        circle.fillAmount = fill;
    }
}
