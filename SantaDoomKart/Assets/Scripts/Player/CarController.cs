using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable
{

    public InputManager input;
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public List<Transform> steeringWheelsMesh;
    public float strengthCoefficient = 20000f;
    public float maxTurn = 20f;
    public float speed;

    private bool switchAudio;
    private bool active;

    [SerializeField] private Rigidbody _rigidbody;
    
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private float modifier;
    private float sendPitch;
    private float receivePitch;
    
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            speed = _rigidbody.velocity.magnitude / 100;
            float soundPitch = 1;
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.motorTorque = strengthCoefficient * Time.deltaTime * input.throttle;
            }

            foreach (WheelCollider wheel in steeringWheels)
            {
                wheel.steerAngle = maxTurn * input.steer;
            }

            UpdateWheelPos(steeringWheels[0], steeringWheelsMesh[0]);
            UpdateWheelPos(steeringWheels[1], steeringWheelsMesh[1]);


            if (speed > 0.02f)
                    {
                        soundPitch = 1.2f;
                    }

                    if (speed > 0.06f)
                    {
                        soundPitch = 1.3f;
                    }

                    if (speed > 0.10f)
                    {

                        soundPitch = 1.4f;
                    }

                    if (speed > 0.14f)
                    {
                        soundPitch = 1.5f;
                    }

                    if (speed > 0.20f)
                    {
                        soundPitch = 1.6f;
                    }

                    if (speed > 0.24f)
                    {
                        soundPitch = 1.7f;
                    }

                    if (speed > 0.30f)
                    {
                        soundPitch = 1.8f;
                    }
                    m_AudioSource.pitch = (speed * 35 / soundPitch) * modifier + .6f;
        }
        else
        {
            m_AudioSource.pitch = receivePitch;
        }

    }

    void PlayAudio(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            sendPitch = m_AudioSource.pitch;//this is your information you will send over the network
            stream.Serialize (ref sendPitch);//im pretty sure you have to use ref here, check that
        }
        else if(stream.IsReading){//this is the information you will recieve over the network
            stream.Serialize (ref receivePitch);//Vector3 position
        }
    }
    
    void UpdateWheelPos(WheelCollider col, Transform t)
    {
        Vector3 pos = t.position;
        Quaternion rot = t.rotation;
        col.GetWorldPose(out pos, out rot);
        rot = rot * Quaternion.Euler(new Vector3(0, 0, 0));
        t.position = pos;
        t.rotation = rot;
    }
}
