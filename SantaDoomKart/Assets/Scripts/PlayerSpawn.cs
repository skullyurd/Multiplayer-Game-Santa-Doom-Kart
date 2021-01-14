using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] cars = new GameObject[] { };
        [SerializeField] private GameObject[] characters = new GameObject[] { };
        [SerializeField] private CameraGuy cameraGuy;
        [SerializeField] private GameObject playGround;
        [SerializeField] private Joystick joyStick;
        [SerializeField] private PostProcessingBehaviour postProcessing;
        [SerializeField] private Image itemPicture;
        [SerializeField] private Stats_Player playerStats;
        [SerializeField] private int health;
        [SerializeField] private int score;
        [SerializeField] private Text healthText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private Shootbullet shootBulletScript;
        [SerializeField] private Text bulletText;
        [SerializeField] private Button shootButton;

        private int indexCar;
        private int indexPlayer;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

    void pureasehalpMii()
    {
        Debug.Log("Find Higher Ground ASAP");
    }

        public void Spawn()
        {
            indexPlayer = PlayerPrefs.GetInt("PlayerSelected");
            indexCar = PlayerPrefs.GetInt("CarSelected");
            //Temporary Debug log until player is implemented
            Debug.Log(" Selected player with index : " + indexPlayer + " And car with index : " + indexCar);

            //Instantiate the car and player
            var spawnedCar = Instantiate(cars[indexCar], spawnPoint, Quaternion.identity);
            Vector3 spawnPlayerHere = spawnedCar.GetComponent<PlayerPlacement>().getSpawnPos().transform.position;
            var spawnPlayer = Instantiate(characters[indexPlayer], spawnPlayerHere, Quaternion.identity,
                spawnedCar.transform);
            InputManager input = spawnedCar.GetComponent<InputManager>();
            Shootbullet shootBulletScript = spawnedCar.GetComponent<Shootbullet>();
            Hud hud = spawnedCar.GetComponent<Hud>();

            //assign all references
            input.SetJoyStick(joyStick);
            input.SetPlayer(spawnPlayer);

            shootBulletScript.setBulletText(bulletText);
            shootBulletScript.setImage(itemPicture);

            playerStats = spawnedCar.GetComponent<Stats_Player>();
            playerStats.setHealth(health);
        }
    }
