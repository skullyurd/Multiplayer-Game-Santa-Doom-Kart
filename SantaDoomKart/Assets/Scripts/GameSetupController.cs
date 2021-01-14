using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviour
{

    [SerializeField] private Vector3[] spawnpositions;

    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), spawnpositions[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity, 0);
    }
}
