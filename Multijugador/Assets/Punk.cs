using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Punk : MonoBehaviourPun, IPunObservable
{
    public MonoBehaviour[] localScripts;

    public GameObject[] localObjects;

    Vector3 lattPos;
    Quaternion lattRot;


    void Start()
    {
        if (photonView.IsMine)
        {

        }
        else
        {

            for (int i = 0; i < localScripts.Length; i++)
            {
                localScripts[i].enabled = false;
            }
            for (int i = 0; i < localObjects.Length; i++)
            {
                localObjects[i].SetActive(false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {

            lattPos = (Vector3)stream.ReceiveNext();
            lattRot = (Quaternion)stream.ReceiveNext();
        }
    }

    //
    void Update()
    {
        if (!photonView.IsMine)
        {

            transform.position = Vector3.Lerp(transform.position, lattPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, lattRot, Time.deltaTime * 5);
        }
    }
}