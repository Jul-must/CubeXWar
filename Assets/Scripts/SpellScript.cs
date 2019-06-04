using UnityEngine;
using System.Collections;

public class SpellScript : MonoBehaviour 
{
    public float CD = 0;
    public float timeFromCD = 0;
    public bool OnCD = false;
    public float MaxDMG = 1;
    public float MinDMG = 0;
    public string spellName;
    public string Owner;
    public bool dead = false;
    public bool destroyOnPlayerImpact = true;
    public Texture2D ActionbarTexture;
    public GameObject SoundPlayer;
    public AudioClip SoundEffect;
    public int BuyNumber;
	// Use this for initialization

    public void Initialize()
    {
        spellName = gameObject.name;
        timeFromCD = Time.time;
    }

    void Start()
    {
        if (SoundEffect != null)
        {
            SoundPlayer.GetComponent<AudioSource>().clip = SoundEffect;
            GameObject.Instantiate(SoundPlayer, GameObject.Find(Owner).transform.position, GameObject.Find(Owner).transform.rotation);
        }
    }

    public void UpdateScipt(SpellScript NewScipt)
    {
        CD = NewScipt.CD;
        MaxDMG = NewScipt.MaxDMG;
        MinDMG = NewScipt.MinDMG;
        Owner = NewScipt.Owner;
        destroyOnPlayerImpact = NewScipt.destroyOnPlayerImpact;
        spellName = NewScipt.spellName;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Environment" || other.tag == "Wall")
        {
            dead = true;
            Debug.Log("wallhit");
        }
    }

    void Update ()
    {
        if (dead)
        {
            Debug.Log("deadspell");
            Terminate();
        }
    }
	
	// UpdateCoolDown is called once per frame
	public void UpdateCoolDown() 
    {
        if (OnCD == true)
        {
            timeFromCD += Time.deltaTime;

            if (timeFromCD >= CD)
            {
                OnCD = false;
                timeFromCD = 0;
            }
        }
        else // needed becuz of respawn
        {
            timeFromCD = 0;
        }
	}

    public float GetDMG()
    {
        return Random.Range(MinDMG, MaxDMG);
    }

    void Terminate()
    {
        Destroy(gameObject);
    }
}
