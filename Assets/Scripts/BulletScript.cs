using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
    //All variables can be set from the AoESpell.
{
    float speed = 8f;
    Vector3 direction;
    string owner;
    public float LifeTime = 1;
	void Start () 
    {
        owner = gameObject.GetComponent<SpellScript>().Owner;
	}
	void Update () 
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        Destroy(gameObject, LifeTime);
	}
}
