using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour 
{
    float speed = 7f;
    public Texture2D texture2D;

	void Start ()
    {
        Destroy(gameObject, 2f);
	}
	void Update () 
    {
        transform.position += transform.forward * Time.deltaTime * speed;
	}
}
