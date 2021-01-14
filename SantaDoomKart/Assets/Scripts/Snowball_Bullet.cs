using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball_Bullet : MonoBehaviour
{

    [SerializeField] private Rigidbody rg_bullet;
    [SerializeField] private int speed;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject snowBall;
    [SerializeField] private GameObject startExplosion;
    [SerializeField] private DestroyMeOverTime m_DestroyMeOverTime;
    [SerializeField] private bool regularSnowball;
    [SerializeField] private AudioSource m_audioClip;
    [SerializeField] private AudioClip rumbleSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private GameObject playerThatShot;
    float time;

    private void Start()
    {
        startExplosion.SetActive(true);
        forwardBullet();
    }

    void Update()
    {
        if (!regularSnowball)
        {
            countTimeToExplode();   
        }
    }

    void forwardBullet()
    {
        m_audioClip.clip = shotSound;
        m_audioClip.Play();
        if (regularSnowball)
        {
            Vector3 shootHere = new Vector3(transform.forward.x,0.05f,transform.forward.z);
              rg_bullet.velocity = shootHere * speed;
        }
        else
        {
            StartCoroutine(PlayRumbleSound());
            rg_bullet.velocity = transform.forward * speed;
            m_audioClip.Play();
        }
    }

    void countTimeToExplode()
    {
        time += Time.deltaTime * 1;
        growingSnowBall();
        if (time > 3)
        {
            m_audioClip.Stop();
            Explode();
        }
    }

    void Explode()
    {
        explosionEffect.transform.position = this.transform.position;
        explosionEffect.SetActive(true);
        m_DestroyMeOverTime.StartCoroutine("DestroyTime", 2 );
        m_audioClip.clip = impactSound;
        m_audioClip.Play();
        snowBall.SetActive(false);
    }

    void growingSnowBall()
    {
        this.transform.localScale += new Vector3(0.04f, 0.04f, 0.04f);
    }

    IEnumerator ChanceToHit()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (checkIfPowerup(collision.collider))
        {
            
        }
        else
        {
            if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                Explode();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (checkIfPowerup(other))
        {
            
        }
        else
        {
            if (regularSnowball)
            {
                Explode();
            }
            else
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
                {
                    Explode();  
                }
            }
        }
    }

    public void SetPlayerThatShot(GameObject me)
    {
        this.playerThatShot = me;
    }

    public GameObject GetPlayerThatShot()
    {
        return this.playerThatShot;
    }
    
    IEnumerator PlayRumbleSound()
    {
        yield return new WaitForSeconds(m_audioClip.clip.length);
        m_audioClip.clip = rumbleSound;
        m_audioClip.Play();
    }

    private bool checkIfPowerup(Collider collider)
    {
          if (collider.CompareTag("invincibility") || collider.CompareTag("regularbull") || collider.CompareTag("Snowball") || collider.CompareTag("shield") || collider.CompareTag("SpeedUp") )
          {
              return true;
          }
          else
          {
              return false;
          }
    }
    
}