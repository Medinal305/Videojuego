using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviourPunCallbacks
{

    string Nombredeljuegador = "Jugador 1";

    string gameVersion = "0.9";

    List<RoomInfo> CreandoPartida = new List<RoomInfo>();

    string nombredelaSala = "Sala 1";
    Vector2 ListadeSala = Vector2.zero;
    bool Unirse = false;


    void Start()
    {

        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {

            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("lista de Salas");

        CreandoPartida = roomList;
    }

    void OnGUI()
    {
        GUI.Window(0, new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400), LobbyWindow, "Lobby");
    }

    void LobbyWindow(int index)
    {

        GUILayout.BeginHorizontal();

        GUILayout.Label("Estado: " + PhotonNetwork.NetworkClientState);

        if (Unirse || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }

        GUILayout.FlexibleSpace();


        nombredelaSala = GUILayout.TextField(nombredelaSala, GUILayout.Width(250));

        if (GUILayout.Button("Creando Sala", GUILayout.Width(125)))
        {
            if (nombredelaSala != "")
            {
                Unirse = true;

                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsOpen = true;
                roomOptions.IsVisible = true;
                roomOptions.MaxPlayers = (byte)10;

                PhotonNetwork.JoinOrCreateRoom(nombredelaSala, roomOptions, TypedLobby.Default);
            }
        }

        GUILayout.EndHorizontal();


        ListadeSala = GUILayout.BeginScrollView(ListadeSala, true, true);

        if (CreandoPartida.Count == 0)
        {
            GUILayout.Label("Aún no se han creado Sala...");
        }
        else
        {
            for (int i = 0; i < CreandoPartida.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(CreandoPartida[i].Name, GUILayout.Width(400));
                GUILayout.Label(CreandoPartida[i].PlayerCount + "/" + CreandoPartida[i].MaxPlayers);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Uniser a Sala"))
                {
                    Unirse = true;


                    PhotonNetwork.NickName = Nombredeljuegador;


                    PhotonNetwork.JoinRoom(CreandoPartida[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();


        GUILayout.BeginHorizontal();

        GUILayout.Label("Nombre del  juegador: ", GUILayout.Width(85));

        Nombredeljuegador = GUILayout.TextField(Nombredeljuegador, GUILayout.Width(250));

        GUILayout.FlexibleSpace();

        GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby || PhotonNetwork.NetworkClientState == ClientState.Disconnected) && !Unirse;
        if (GUILayout.Button("Actualizar", GUILayout.Width(100)))
        {
            if (PhotonNetwork.IsConnected)
            {

                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
            else
            {

                PhotonNetwork.ConnectUsingSettings();
            }
        }

        GUILayout.EndHorizontal();

        if (Unirse)
        {
            GUI.enabled = true;
            GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Connecting...");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
        Unirse = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        Unirse = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
        Unirse = false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Creando Sala");

        PhotonNetwork.NickName = Nombredeljuegador;

        PhotonNetwork.LoadLevel("Level1");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Unido a la Sala");
    }
}