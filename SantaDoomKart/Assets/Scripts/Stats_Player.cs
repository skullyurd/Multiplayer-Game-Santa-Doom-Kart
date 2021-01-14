using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Lean.Gui;

public class Stats_Player : MonoBehaviourPunCallbacks
{
    [SerializeField] private int health; //temporarily SerializeField to test things out
    [SerializeField] private Vector3[] respawnPoints;
    [SerializeField] private int kills;
    [SerializeField] private int deaths;
    [SerializeField] private bool invincibility;
    [SerializeField] private bool hasPowerUp;
    [SerializeField] private bool shielded;
    [SerializeField] private GameObject activeShield;
    [SerializeField] private GameObject speedboostEffect;
    [SerializeField] private CarController m_CarController;
    private float oldStrength;
    
    //UIhealth
    [SerializeField] private Image[] hearts;
    [SerializeField] private int numOfHearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    //carmeshes for effects
    [SerializeField] private Renderer car_Mesh;
    [SerializeField] private Renderer char_Mesh;
    [SerializeField] private Material flashRed;
    [SerializeField] private Material[] rainbow;
    
    //old materials for reverting after effects
    private Material[] oldcarMeshes;
    private Material[] oldCharMeshes;
    
    //audio
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioSource invincibility_audiosource;
    
    //references for resetting power-ups coroutines
    private Coroutine shieldcoCoroutine;
    private Coroutine speedcoCoroutine;
    private Coroutine invinccoCoroutine;

    public LeanWindow LeanWindowSript;
    private bool PauseOpen;
    

    void Start()
    {
        Hashtable killsProperties = new Hashtable() {{"Kills", kills}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(killsProperties);
        
        Hashtable deathsProperties = new Hashtable() {{"Deaths", deaths}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(deathsProperties);
        
        oldStrength = m_CarController.strengthCoefficient;

        oldcarMeshes = car_Mesh.materials;
        oldCharMeshes = char_Mesh.materials;
    }

    private void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            switch (PauseOpen)
            {
                case true:
                    PauseOpen = false;
                    LeanWindowSript.TurnOff();
                    break;

                case false:
                    PauseOpen = true;
                    LeanWindowSript.TurnOn();
                    break;
            }
        }

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (hasPowerUp == false)
        {
            if (photonView.IsMine)
            {
                if (other.tag == "bullet" && shielded == false)
                {
                    ReceiveDamage(other, 1);
                }

                if (other.tag == "explosion" && shielded == false)
                {
                    ReceiveDamage(other, 5);
                }

                if (other.tag == "PlayerInvincible")
                {
                     ReceiveDamage(other, 5);
                }

                if (other.tag == "shield" || other.tag == "invincibility" || other.tag == "SpeedUp")
                {
                      m_AudioSource.Play();
                }
            }
                if (other.tag == "shield")
                {
                    getShield(); 
                    other.GetComponent<RespawnPowerup>().SetDisabled();
                }
                if (other.tag == "invincibility")
                {
                    turnInvincibilityOn();
                    this.gameObject.tag = "PlayerInvincible";
                    other.GetComponent<RespawnPowerup>().SetDisabled();
                }

                if (other.tag == "SpeedUp")
                {
                    turnSpeedBoostOn();
                    other.GetComponent<RespawnPowerup>().SetDisabled();
                }
        }
    }

    IEnumerator shieldOff()
    {
        yield return new WaitForSeconds(10);
        shielded = false;
        activeShield.SetActive(false);
    }
    
    IEnumerator speedBoost()
    {
        yield return new WaitForSeconds(5);
        m_CarController.strengthCoefficient = oldStrength;
        speedboostEffect.SetActive(false);
    }

    void turnSpeedBoostOn()
    {
        speedboostEffect.SetActive(true);
        m_CarController.strengthCoefficient = 40000;
        
        if (speedcoCoroutine != null)
        {
            StopCoroutine(speedcoCoroutine);
            speedcoCoroutine = null;
        }
        speedcoCoroutine = StartCoroutine( speedBoost());
    }


    void getShield()
    {
        shielded = true;
        activeShield.SetActive(true);

        if (shieldcoCoroutine != null)
        {
            StopCoroutine(shieldcoCoroutine);
            shieldcoCoroutine = null;
        }
        shieldcoCoroutine = StartCoroutine(shieldOff());
    }

   public void ReceiveDamage(Collider other, int damage)
    {
        photonView.RPC("RPC_ReceiveDamage",RpcTarget.All, damage);
        if (health < 1)
        {
            String victimName = PhotonNetwork.LocalPlayer.NickName;
            String killerName;
            if (other.GetComponent<Snowball_Bullet>())
            {
               killerName =  other.GetComponent<Snowball_Bullet>().GetPlayerThatShot().GetPhotonView().Owner.NickName;
               GiveKill(other.GetComponent<Snowball_Bullet>().GetPlayerThatShot());
            }
            else
            {
                killerName = other.gameObject.GetPhotonView().Owner.NickName;
                GiveKill(other.gameObject);
            }
            Death();
            SpawnKillDisplay(victimName, killerName);
            Respawn();
        }
    }

    [PunRPC]
    void RPC_ReceiveDamage(int damage)
    {
        health = health - damage;
        StartCoroutine(FlashObject());
    }

    void SpawnKillDisplay(String victim, String killer)
    {
        object[] instanceData = new object[2];
        instanceData[0] = killer;
        instanceData[1] = victim;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "KillDisplay"), new Vector3(120, -200, 0), Quaternion.identity, 0,instanceData);
    }
    

    void GiveKill(GameObject killer)
    {
        int killerAmountOfKills = (int)killer.GetPhotonView().Owner.CustomProperties["Kills"];
        killerAmountOfKills++;
        Hashtable hash = new Hashtable();
        hash.Add("Kills", killerAmountOfKills);
        killer.GetPhotonView().Owner.SetCustomProperties(hash);
    }
    
    void Death()
    {
        deaths++;
        Hashtable hash = new Hashtable();
        hash.Add("Deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    

    void Respawn()
    {
        transform.position = respawnPoints[Random.Range(0, respawnPoints.Length)];
        health = 5;
    }

    void turnInvincibilityOn()
    {
        shielded = true;
        invincibility = true;
        
        
        if (invinccoCoroutine != null)
        {
            StopCoroutine(invinccoCoroutine);
            invinccoCoroutine = null;
        }
        invinccoCoroutine = StartCoroutine(RaindbowPlayer());
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    IEnumerator FlashObject()
    {
        Material[] mats = car_Mesh.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = flashRed;
        }

        car_Mesh.materials = mats;
        
        mats = char_Mesh.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = flashRed;
        }

        char_Mesh.materials = mats;
        
        yield return new WaitForSeconds(0.5f);

        car_Mesh.materials = oldcarMeshes;
        char_Mesh.materials = oldCharMeshes;
    }
    
    IEnumerator RaindbowPlayer()
    {
        invincibility_audiosource.Play();


        while (invincibility_audiosource.isPlaying)
        {
              foreach (var color in rainbow)
                        {
                            Material[] mats = car_Mesh.materials;
               
                            for (int e = 0; e < mats.Length; e++)
                            {
                                mats[e] = color;
                            }
            
            
                            car_Mesh.materials = mats;
                    
                    
                            mats = char_Mesh.materials;
            
            
                            for (int r = 0; r < mats.Length; r++)
                            {
                                mats[r] = color;
                            }
                            char_Mesh.materials = mats;
            
                            yield return new WaitForSeconds(0.02f);
                            
                        }
        }
        
        invincibility = false;
        shielded = false;
        this.gameObject.tag = "Player";
       
        car_Mesh.materials = oldcarMeshes;
        char_Mesh.materials = oldCharMeshes;
    }
}
