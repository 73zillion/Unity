using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRespawn : NetworkBehaviour {

    public GameObject localPlayer;
    public GameObject buttonRespawn;
    private PlayerHealth healthScript;

    bool respawning = false;
    public int countDownStartingValue = 9;
    private int countDownCurrentValue;

    // Use this for initialization
	void Start () {
        respawning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<PlayerHealth>().currentHealth <= 0 && !respawning)
        {
            respawning = true;
            Invoke("RpcRespawn", countDownStartingValue);
            if (isLocalPlayer)
            {
                //Invoke("RpcRespawn", countDownStartingValue);
                GameObject TextRespawnObject = GameObject.Find("TextRespwan");
                Text TextRespawn = TextRespawnObject.GetComponent<Text>();
                countDownCurrentValue = countDownStartingValue;
                InvokeRepeating("UndateRespawnText", 1.0f, 1.0f);
                TextRespawn.text = countDownCurrentValue.ToString();
            }
        }
	}

    public override void OnStartLocalPlayer()
    {
        healthScript = GetComponent<PlayerHealth>();
    }

    [ClientRpc]
    void RpcRespawn() {
        // need to get the location of the respawn point get this from the start location   
        Transform spawn = NetworkManager.singleton.GetStartPosition();
        transform.position = spawn.position;

        // reset the health from 0 to the starting health
        GetComponent<PlayerHealth>().currentHealth = GetComponent<PlayerHealth>().startingHealth;

        // we put the player in the "idle" animation
        GetComponent<Animator>().Play("IdleWalk");

        GetComponent<PlayerHealth>().isDead = false;
        respawning = false;
    }

    void UndateRespawnText() {
        GameObject TextRespawnObject = GameObject.Find("TextRespwan");
        Text TextRespawn = TextRespawnObject.GetComponent<Text>();
        countDownCurrentValue--;
        TextRespawn.text = countDownCurrentValue.ToString();
        if(countDownCurrentValue <= 0)
        {
            CancelInvoke("UndateRespawnText");
            TextRespawn.text = "";
        }
    }
}
