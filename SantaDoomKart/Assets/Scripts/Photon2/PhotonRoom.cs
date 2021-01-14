using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Manager info
    public Sprite[] carIconsList;
    public Sprite[] CharacterIconList;

    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;
    public int multiplayerScene;

    //Player info
    public Photon.Realtime.Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;
    public int carNumber;
    public int characterNumber;
    public string resultName;
    public Image characterIcon;
    public Image carIcon;
    
    //Spawn positions
    public Vector3[] spawnpositions;
    
    [SerializeField] private LeanWindow lobbyDisplay;
    public Text[] playersNameDisplays;
    public GameObject[] playerDisplayObjectReferences;

    [SerializeField] private SyncedTimerLobby m_SyncedTimerLobby;

    public void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        carNumber = 0;
        characterNumber = 0;
        checkCarNumber();
        checkCharacterNumber();
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
        }
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a room");
        UpdateLobbyPlayers();
        lobbyDisplay.TurnOn();
        m_SyncedTimerLobby.StartTimer();
        
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        Debug.Log( "Player Number " + player.ActorNumber + " joined the room");
        UpdateLobbyPlayers();
    }
    
    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        Debug.Log( "Player Number " + player.ActorNumber + " left the room");
        foreach (var reference in playerDisplayObjectReferences)
        {
            reference.SetActive(false);
        }
        UpdateLobbyPlayers();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerScene);
    }


    private void CreatePlayer()
    {
        combineSelection();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", resultName), spawnpositions[PhotonNetwork.LocalPlayer.ActorNumber -1], Quaternion.identity,0);
    }

    private void UpdateLobbyPlayers() 
    {
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            playersNameDisplays[i].text = photonPlayers[i].NickName;
            playerDisplayObjectReferences[i].SetActive(true);
        }
    }
    
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left current room");
        lobbyDisplay.TurnOff();
        foreach (var reference in playerDisplayObjectReferences)
        {
            reference.SetActive(false);
        }
        m_SyncedTimerLobby.CancelTimer();
    }

    public void characterDown()
    {
        return;
        characterNumber--;
        checkCharacterNumber();
    }

    public void characterUp()
    {
        return;
        characterNumber++;
        checkCharacterNumber();
    }

    public void carDown()
    {
        carNumber--;
        checkCarNumber();
    }

    public void CarUp()
    {
        carNumber++;
        checkCarNumber();
    }

    void checkCarNumber()
    {


        if(carNumber > 6)
        {
            carNumber = 0;
        }

        if(carNumber < 0)
        {
            carNumber = 6;
        }

        carIcon.sprite = carIconsList[carNumber];
    }

    void checkCharacterNumber()
    {


        if (characterNumber > 6)
        {
            characterNumber = 0;
        }

        if(characterNumber < 0)
        {
            carNumber = 6;
        }

        characterIcon.sprite = CharacterIconList[characterNumber];
    }

    void combineSelection()
    {
        if(characterNumber == 0 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer";

        }
        if (characterNumber == 0 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 0 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer3";

        }
        if (characterNumber == 0 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer4";

        }
        if (characterNumber == 0 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer5";

        }
        if (characterNumber == 0 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer6";

        }
        if (characterNumber == 0 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer7";

        }
        if (characterNumber == 1 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 1 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 2 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 3 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 4 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 0)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 1)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 2)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 3)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 4)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 5)
        {
            resultName = "PhotonNetworkPlayer2";

        }
        if (characterNumber == 5 && carNumber == 6)
        {
            resultName = "PhotonNetworkPlayer2";

        }

    }
}
