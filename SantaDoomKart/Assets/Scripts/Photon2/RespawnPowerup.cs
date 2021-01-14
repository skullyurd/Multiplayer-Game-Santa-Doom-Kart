using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RespawnPowerup : MonoBehaviour
{
    [SerializeField] private GameObject[] itemsOrWeapons;
    [SerializeField] private float setSpawnCoolDown;
    [SerializeField] private float actualCooldown;
    [SerializeField] private bool respawn;
    [SerializeField] private Vector3 startpos;
    [SerializeField] private bool staticPowerup;
    [SerializeField] private GameObject staticPowerUpToSpawn;

    private void Start()
    {
        actualCooldown = setSpawnCoolDown;
        startpos = transform.position;
    }

    void resetCoolDown()
    {
        actualCooldown -= Time.deltaTime * 1;

        if (actualCooldown < 1)
        {
            SpawnNewPowerup();
            actualCooldown = setSpawnCoolDown;
        }
    }

    void Update()
    {
        if (respawn)
        {
            resetCoolDown();
        }
    }

    public void SetDisabled()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }

        this.gameObject.GetComponent<BoxCollider>().enabled = false;

        if (gameObject.GetComponent<MeshRenderer>())
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        respawn = true;
    }

    private void SpawnNewPowerup()
    {
        string prefabName;
        if (staticPowerup)
        {
             prefabName = Path.Combine("PhotonPrefabs", staticPowerUpToSpawn.name);
        }
        else
        {
            int newPowerupIndex = Random.Range(0, itemsOrWeapons.Length);
             prefabName = Path.Combine("PhotonPrefabs", itemsOrWeapons[newPowerupIndex].name);
        }
        PhotonNetwork.InstantiateSceneObject(prefabName, startpos, Quaternion.identity, 0);
        PhotonNetwork.Destroy(this.gameObject);
    }

}
