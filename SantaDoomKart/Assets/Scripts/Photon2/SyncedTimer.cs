using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SyncedTimer : MonoBehaviourPunCallbacks
{
    
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    private float actualTimeForDisplay;
    
    ExitGames.Client.Photon.Hashtable CustomeValue;
    
    [SerializeField] private float timerLength;
    [SerializeField] private Text timeText;
    [SerializeField] private Image circle;
    [SerializeField] private GameObject scoreboardObject;
    [SerializeField] private Scoreboard m_ScoreboardScript;

    void Start()
    {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTimeLobby"].ToString());
            startTimer = true;

    }
    
    void Update()
    {
        if (!startTimer) return;
        
        timerIncrementValue = PhotonNetwork.Time - startTime;
        actualTimeForDisplay = timerLength - (float)timerIncrementValue;
        
        UpdateUITimer();
        
        if (actualTimeForDisplay <= 0)
        {
            startTimer = false;
            m_ScoreboardScript.UpdateScoreboard();
            scoreboardObject.SetActive(true);
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
