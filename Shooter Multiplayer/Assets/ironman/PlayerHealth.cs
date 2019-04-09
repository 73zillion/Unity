using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	public int startingHealth = 100;


    [SyncVar] public int currentHealth = 100;

	float shakingTimer = 0;
	public float timeToShake = 1.0f;
	public float shakeIntensity = 3.0f;
	bool isShaking = false;

	public bool isDead = false;
	Animator anim;

    public Text healthText;

	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		anim = GetComponent<Animator> ();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
        SetHealthText();

        if (!isDead)
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Death();
            }
        }

    }


	public void TakeDamage(int amount){
        if (!isServer)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

		//ShakeCamera ();

		currentHealth -= amount;
		
	}

	public void Death(){
		if (isDead)
        {
            return;
        }

        if (isServer)
        {
            RpcDeath();
        }
        else
        {
            anim.SetTrigger("Death");
            isDead = true;
        }
	}

    [ClientRpc]
    public void RpcDeath() {
        if (isDead)
        {
            return;
        }
        anim.SetTrigger("Death");
        isDead = true;
    }

    void SetHealthText() {
        if (isLocalPlayer)
        {
            healthText.text = "HP: " + currentHealth.ToString();
        }
    }


}
