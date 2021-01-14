using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
  public void OnLeaveGame()
   {
      PhotonNetwork.LeaveRoom();
      PhotonNetwork.LoadLevel(0);
   }

   public void OnQuitApplication()
   {
      Application.Quit();
   }
}
