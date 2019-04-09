using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

    [SyncVar] public string playerUniqueName;
    private NetworkInstanceId playerNetID;

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        // todo: get the network ID
        // todo: set the ID of the player
        GetNetIdentity();
        SetIdentity();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.name == "" || transform.name == "ironmanPrefab(Clone)")
        {
            SetIdentity();
        }
	}

    [Client]
    void GetNetIdentity() {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        // todo: tell all the clients about the player ID
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    void SetIdentity() {
        if (!isLocalPlayer)
        {
            this.transform.name = playerUniqueName;
        }
        else
        {
            // case when it is the local client player
            // we need to create the ID
            transform.name = MakeUniqueIdentity();
        }
    }

    string MakeUniqueIdentity() {
        return "Player" + playerNetID.ToString();
    }

    [Command]
    void CmdTellServerMyIdentity(string name) {
        playerUniqueName = name;
    }
}
