using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class KillDisplayUI : MonoBehaviour
{
   [SerializeField] private  float destroyTime;
   [SerializeField] float speed;
   private PhotonView pV;

   private void Awake()
   {
      
       pV = transform.GetComponent<PhotonView>();
 
       object[] data=  pV.InstantiationData;
       this.GetComponent<Text>().text =
           data[0] + " killed " + data[1];
       this.transform.SetParent(GameObject.FindWithTag("Canvas").transform,false);
   }

   void Start ()
   {
       StartCoroutine(Wait());
   }

    void Update ()
    {
        
        var y=Time.deltaTime*speed;
        transform.Translate(0,-y,0);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
}
