using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeSpell : MonoBehaviour
{
    public float Timer = 0;
    public float DelayTime = 0;
    public float DestroyTime = 0;

    public List<GameObject> HitBox = new List<GameObject>();
	void Start () 
    {
        foreach (GameObject hitbox in HitBox)
        {
            hitbox.GetComponent<SpellScript>().UpdateScipt(GetComponent<SpellScript>());
        }
	}
	
	void Update () 
    {
        Timer += Time.deltaTime;
        if (Timer >= DelayTime)
        {
            foreach (GameObject hitbox in HitBox)
            {
                //hitbox.GetComponent<BoxCollider>().enabled = false;
            }
        }
        if (Timer >= DestroyTime)
        {
            Destroy(gameObject);
        }    
	}
}
