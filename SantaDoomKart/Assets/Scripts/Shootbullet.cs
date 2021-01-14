using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Shootbullet : MonoBehaviourPunCallbacks
{
    enum theKindOfBullet
    {
        snowBallBomb,
        regularBullet,
        followingBullet,
    }


    //  Array[0] = snowball
    //  Array[1] = regular bullet
    //  Array[2] = bullet that follows

    private theKindOfBullet carryingBullet;

    [SerializeField] private Image[] ItemPicture;
    [SerializeField] private Sprite[] ItemPictures;
    [SerializeField] private GameObject bulletOutput;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] bulletKind;
    [SerializeField] private int bulletAmount; // might later be changed into just private int instead os serializeField. For it is like this to test things out
    [SerializeField] private Text[] bulletAmountToText;
    private GameObject shotBulletReference;
    [SerializeField] private AudioSource m_thisAudio;

    private void Start()
    {
        ItemPicture[0].color = new Color(0, 0, 0, 0);
        ItemPicture[1].color = new Color(0, 0, 0, 0);
        ItemPicture[2].color = new Color(0, 0, 0, 0);
        ItemPicture[3].color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyUp(KeyCode.Space))
        {
           shoot();
        }
        
    }

    public void shoot()
    {
        if (bulletAmount > 0)
        {
            string prefabName = Path.Combine("PhotonPrefabs", bullet.name);
            photonView.RPC("RPC_Shoot",RpcTarget.All);
            bulletAmount--;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";

            if (bulletAmount < 1)
            {
                bullet = null;
                ItemPicture[0].sprite = null;
                ItemPicture[0].color = new Color(0, 0, 0, 0);
                ItemPicture[1].sprite = null;
                ItemPicture[1].color = new Color(0, 0, 0, 0);
                ItemPicture[2].sprite = null;
                ItemPicture[2].color = new Color(0, 0, 0, 0);
                ItemPicture[3].sprite = null;
                ItemPicture[3].color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            return;
        }
    }
    
    [PunRPC] 
    void RPC_Shoot()
    {
      GameObject go = Instantiate(bullet, bulletOutput.transform.position, this.transform.rotation);
      go.GetComponentInChildren<Snowball_Bullet>().SetPlayerThatShot(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.tag == "Snowball" || photonView.IsMine && other.tag == "regularbull" || photonView.IsMine && other.tag == "stalkbull")
        {
            m_thisAudio.Play();
        }
        if (other.tag == "Snowball")
        {
            carryingBullet = theKindOfBullet.snowBallBomb;
            grabItemSnowBall();
            other.GetComponent<RespawnPowerup>().SetDisabled();
        }

        if (other.tag == "regularbull")
        {
            carryingBullet = theKindOfBullet.regularBullet;
            grabRegularBullet();
            other.GetComponent<RespawnPowerup>().SetDisabled();
        }
        if (other.tag == "stalkbull")
        {
            carryingBullet = theKindOfBullet.followingBullet;
            grabStalkBullet();
            other.GetComponent<RespawnPowerup>().SetDisabled();
        }
    }

    void checkWhichItem()
    {

    }

    void grabItemSnowBall() 
    {
        if (bullet == bulletKind[0])
        {
            bulletAmount++;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";
        }
        else
        {
            bullet = bulletKind[0];
            bulletAmount = 1;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";

            ItemPicture[0].color = new Color(1, 1, 1, 1);
            ItemPicture[0].sprite = ItemPictures[0];
            ItemPicture[1].color = new Color(1, 1, 1, 1);
            ItemPicture[1].sprite = ItemPictures[0];
            ItemPicture[2].color = new Color(1, 1, 1, 1);
            ItemPicture[2].sprite = ItemPictures[0];
            ItemPicture[3].color = new Color(1, 1, 1, 1);
            ItemPicture[3].sprite = ItemPictures[0];
        }
    }

    void grabRegularBullet()
    {
        if (bullet == bulletKind[1])
        {
            bulletAmount += 3;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";

        }
        else
        {
            bullet = bulletKind[1];
            bulletAmount = 3;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";


            ItemPicture[0].color = new Color(1, 1, 1, 1);
            ItemPicture[0].sprite = ItemPictures[1];
            ItemPicture[1].color = new Color(1, 1, 1, 1);
            ItemPicture[1].sprite = ItemPictures[1];
            ItemPicture[2].color = new Color(1, 1, 1, 1);
            ItemPicture[2].sprite = ItemPictures[1];
            ItemPicture[3].color = new Color(1, 1, 1, 1);
            ItemPicture[3].sprite = ItemPictures[1];
        }
    }

    void grabStalkBullet()
    {
        if (bullet == bulletKind[2])
        {
            bulletAmount += 3;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";
        }
        else
        {
            bullet = bulletKind[2];
            bulletAmount = 3;
            bulletAmountToText[0].text = bulletAmount.ToString() + " x";
            bulletAmountToText[1].text = bulletAmount.ToString() + " x";
            bulletAmountToText[2].text = bulletAmount.ToString() + " x";
            bulletAmountToText[3].text = bulletAmount.ToString() + " x";

            ItemPicture[0].color = new Color(1, 1, 1, 1);
            ItemPicture[0].sprite = ItemPictures[2];
            ItemPicture[1].color = new Color(1, 1, 1, 1);
            ItemPicture[1].sprite = ItemPictures[2];
            ItemPicture[2].color = new Color(1, 1, 1, 1);
            ItemPicture[2].sprite = ItemPictures[2];
            ItemPicture[3].color = new Color(1, 1, 1, 1);
            ItemPicture[3].sprite = ItemPictures[2];
        }
    }

    public void setImage(Image itemPicture)
    {
        ItemPicture[0] = itemPicture;
        ItemPicture[1] = itemPicture;
        ItemPicture[2] = itemPicture;
        ItemPicture[3] = itemPicture;

    }

    public void setBulletText(Text bulletAmountText)
    {
        this.bulletAmountToText[0] = bulletAmountText;
        this.bulletAmountToText[1] = bulletAmountText;
        this.bulletAmountToText[2] = bulletAmountText;
        this.bulletAmountToText[3] = bulletAmountText;
    }
    

}
