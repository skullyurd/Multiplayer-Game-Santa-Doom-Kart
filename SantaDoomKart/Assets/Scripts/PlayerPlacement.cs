using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacement : MonoBehaviour
{
    [SerializeField] private GameObject spawnPos;


    public GameObject getSpawnPos()
    {
        return this.spawnPos;
    }
}
