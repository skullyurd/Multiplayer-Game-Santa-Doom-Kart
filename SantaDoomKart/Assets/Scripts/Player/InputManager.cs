using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviourPunCallbacks
{
    public float throttle;
    public float steer;
    
    [SerializeField] private Joystick Joystick;

    [SerializeField] private bool mobile;
    [SerializeField] public bool pc;
    [SerializeField] private bool fullMobileMotion;
    [SerializeField] private bool halfMobileMotion;
    [SerializeField] private bool buttonPressedGas;
    [SerializeField] private bool buttonPressedBrake;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject mobilePanel;
    [SerializeField] private GameObject PCPanel;
    [SerializeField] private GameObject halfMobileMotionPanel;
    [SerializeField] private GameObject fullMobileMotionPanel;

    private void Start()
    {
        if (pc)
        {
            PCPanel.SetActive(true);
            mobilePanel.SetActive(false);
            halfMobileMotionPanel.SetActive(false);
            fullMobileMotionPanel.SetActive(false);
        }

        if(mobile)
        {
            PCPanel.SetActive(false);
            mobilePanel.SetActive(true);
            halfMobileMotionPanel.SetActive(false);
            fullMobileMotionPanel.SetActive(false);
        }

        if(fullMobileMotion)
        {
            PCPanel.SetActive(false);
            mobilePanel.SetActive(false);
            halfMobileMotionPanel.SetActive(false);
            fullMobileMotionPanel.SetActive(true);
        }

        if(halfMobileMotion)
        {
            PCPanel.SetActive(false);
            mobilePanel.SetActive(false);
            halfMobileMotionPanel.SetActive(true);
            fullMobileMotionPanel.SetActive(false);
        }
    }

    void Update()
    {

        if (photonView.IsMine)
        {
            if (mobile) // full touchscreen
            {
                if (buttonPressedGas)
                {
                    throttle = 1;
                }

                if (!buttonPressedBrake && !buttonPressedGas)
                {
                    throttle = 0;
                }

                if (buttonPressedBrake)
                {
                    throttle = -1;
                }

                steer = Joystick.Horizontal;
            }

            if(pc) // WASD, spacebar
            {
                throttle = Input.GetAxis("Vertical");
                steer = Input.GetAxis("Horizontal");
            }

            if(fullMobileMotion)// gas, rem en sturen via acceleration
            {
                throttle = Input.acceleration.y;
                steer = Input.acceleration.x ;


                //Acceleration Filter
                

            }

            if (halfMobileMotion) //acceleration sturen, gas en rem hebben knoppen
            {
                steer = Input.acceleration.x;

                if (buttonPressedGas)
                {
                    throttle = 1;
                }

                if(!buttonPressedBrake && !buttonPressedGas)
                {
                    throttle = 0;
                }

                if (buttonPressedBrake)
                {
                    throttle = -1;
                }
            }

            animator.SetFloat("Horizontal", Joystick.Horizontal);
            animator.SetFloat("Vertical", Joystick.Vertical);
        }
    }
    
    
    public void SetJoyStick(Joystick joystick)
    {
        this.Joystick = joystick;
    }
    
    public void SetPlayer(GameObject player)
    {
        this.animator = player.GetComponent<Animator>();
    }

    public void giveGasDown()
    {
        buttonPressedGas = true;
    }

    public void giveGasUp()
    {
        buttonPressedGas = false;
    }

    public void giveBrakeDown()
    {
        buttonPressedBrake = true;
    }

    public void giveBrakeUp()
    {
        buttonPressedBrake = false;
    }

    public void toggleFullmotion()
    {
        fullMobileMotion = true;
        halfMobileMotion = false;
        mobile = false;
    }

    public void toggleHalfMotion()
    {
        halfMobileMotion = true;
        mobile = false;
        fullMobileMotion = false;
    }

    public void togglefullMobile()
    {
        if(pc == true)
        {
            mobile = false;
            halfMobileMotion = false;
            fullMobileMotion = false;
        }

        if(pc == false)
        {
            mobile = true;
            halfMobileMotion = false;
            fullMobileMotion = false;
        }
    }
}
