using UnityEngine;
using System.Collections;

/// <summary>
/// this script is attatched to the trigger gameobject on the player.
/// </summary>

public class HealthAndDamage : MonoBehaviour 
{
    //figuring out on whose comp the damage should be applied to

    public string myAttacker;

    //this is used to prevent the player from getting hit while they are undergoing destruction.

    public bool limbo = false;

    //health management.

    public float myHealth = 100;
    public float maxHealth = 100;

    //mine
    string myKiller;

    public bool invulnerable = false;

    public GameObject Healthbar;

	// Use this for initialization
	void Start () 
    {
        myHealth = maxHealth;
        if (networkView.isMine)
        {
            GameObject.Instantiate(Healthbar);
        }
	}

    public void Reset()
    {
        networkView.RPC("ResetEverywhere", RPCMode.All); 
    }

    public void TakeDamage(float damage, string killer)
    {
        if (networkView.isMine)
        {
            int trimDMG = (int)damage;
            networkView.RPC("TakeDamageEverywhere", RPCMode.All, damage, trimDMG, killer);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (myHealth <= 0 && networkView.isMine == true && limbo == false)
        {
            GameObject.Find("SpawnManager").GetComponent<SpawnScript>().playerIsDead = true;
            limbo = true;
            networkView.RPC("UpdateLimboEverywhere", RPCMode.All, true);
            gameObject.GetComponent<PlayerScore>().died = true;
        }
        
        if (myHealth > maxHealth)  //Can not have more heath than max heath.
        {
            myHealth = maxHealth;
        }
        if (myHealth < 0)  //Can not have negative heath.
        {
            myHealth = 0;
        }
	}

    public void DestroyPlayer()
    {
        networkView.RPC("DestroySelf", RPCMode.All);
    }

    [RPC]
    void UpdateMyCurrentAttackerEverywhere(string attacker)
    {
        myAttacker = attacker;
    }

    [RPC]
    void TakeDamageEverywhere(float damage, int trimDMG, string killer)
    {
        if (!invulnerable)
        {
            GameObject.Find("TextManager").GetComponent<TextManager>().SpawnText(trimDMG.ToString(), gameObject.transform.position);
            myHealth = myHealth - damage;
            if (myHealth <= 0 && networkView.isMine == true)
            {
                Debug.Log(killer);
                networkView.RPC("UpdateKillerEverywhere", RPCMode.Others, killer);
            }
        }
        else
        {
            GameObject.Find("TextManager").GetComponent<TextManager>().SpawnText("invulnerable", gameObject.transform.position);
        }
    }

    [RPC]
    void ResetEverywhere()
    {
        Debug.Log("fullHP");
        myHealth = maxHealth;
        limbo = false;
    }

    [RPC]
    void UpdateLimboEverywhere(bool limbo)
    {
        this.limbo = limbo;
    }

    [RPC]
    void UpdateKillerEverywhere(string killer)
    {
        GameObject.Find(killer).GetComponent<PlayerScore>().scored = true;
    }

    [RPC]
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}