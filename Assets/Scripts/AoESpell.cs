using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AoESpell : MonoBehaviour 
    //this script fires "disks" that are instansiaterd here.
{
    public GameObject Bullet;
    float counter;
    string owner;
    List<int> bullets = new List<int>();

	void Start () 
    {
        owner = gameObject.GetComponent<SpellScript>().Owner;
	}
	void Update ()
    {
        for (int i = 0; i < 12; i++)
        {
            transform.Rotate(Vector3.up, 15f);

            GameObject o = (GameObject)Instantiate(Bullet, transform.position, transform.rotation);
            o.GetComponent<SpellScript>().MinDMG = GetComponent<SpellScript>().MinDMG;
            o.GetComponent<SpellScript>().MaxDMG = GetComponent<SpellScript>().MaxDMG;
            o.GetComponent<SpellScript>().Owner = owner;
            bullets.Add(bullets.Count + 1);
        }

        if (bullets.Count == 12)
        {
            Destroy(gameObject);
        }
    }
}
