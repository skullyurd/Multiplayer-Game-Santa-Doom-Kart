using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class spawnPowerUps : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject[] itemsOrWeapons;
    [SerializeField] private Vector3[] powerupLocations;

    private void Awake()
    {
       spawnItems();
    }

    
    void spawnItems()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Vector3 poweruop in powerupLocations)
            {
                int powerupSelection = Random.Range(0, itemsOrWeapons.Length);
                string prefabName = Path.Combine("PhotonPrefabs", itemsOrWeapons[powerupSelection].name);
                GameObject powerup = PhotonNetwork.InstantiateSceneObject(prefabName, poweruop, Quaternion.identity, 0);
            }
        }
    }
}
