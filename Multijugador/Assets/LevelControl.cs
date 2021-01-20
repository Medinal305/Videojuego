using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelControl : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public Transform spawnPoint;


    void Start()
    {

        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Is not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
            return;
        }


        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);
    }

    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;


        if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
        {
            PhotonNetwork.LeaveRoom();
        }


        GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);


        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {

            string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
            GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
        }
    }

    public override void OnLeftRoom()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }
}