using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Text[] scoreboardNameText;
    [SerializeField] private Text[] scoreboardKillsText;
    [SerializeField] private Text[] scoreboardDeathsText;
    [SerializeField] private Text[] scoreboardKDAText;
    [SerializeField] private GameObject[] playerDisplays;
    private int playerCount;

    private void Start()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        
        
        for (int i = 0; i < playerCount; i++)
        {
            playerDisplays[i].SetActive(true);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            UpdateScoreboard();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }
    
   public void UpdateScoreboard()
   {
       List<Photon.Realtime.Player> playerList = PhotonNetwork.PlayerList.ToList();
       
       playerList.Sort(SortByKills);
       
       for (int i = 0; i < playerList.Count; i++)
        {
            scoreboardNameText[i].text = playerList[i].NickName;
            scoreboardKillsText[i].text = playerList[i].CustomProperties["Kills"].ToString();
            scoreboardDeathsText[i].text = playerList[i].CustomProperties["Deaths"].ToString();
            if ((int)playerList[i].CustomProperties["Deaths"] == 0)
            {
                scoreboardKDAText[i].text = ((int)playerList[i].CustomProperties["Kills"]).ToString();
            }
            else
            {
                scoreboardKDAText[i].text = ((int)playerList[i].CustomProperties["Kills"] / (int)playerList[i].CustomProperties["Deaths"]).ToString();
            }
        }
    }
   
   public static int SortByKills (Photon.Realtime.Player a, Photon.Realtime.Player b){
       return Convert.ToInt32(b.CustomProperties["Kills"]).CompareTo(((int)a.CustomProperties["Kills"]));
   }
    
}
