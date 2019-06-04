using UnityEngine;
using System.Collections;

public class WallSpellScript : MonoBehaviour {
    Vector3 targetPoint;
    public float LifeTime = 4;
	void Start () 
    {
        transform.LookAt(GameObject.Find(gameObject.GetComponent<SpellScript>().Owner).transform.position); 
	}
	void Update () 
    {
        Destroy(gameObject, LifeTime);
	}
}
