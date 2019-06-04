using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour 
{
    float startTime = 0;
    public float Delay = 1;
	void Start () 
    {
        gameObject.GetComponent<ParticleEmitter>().emit = false;
	}
	
	void Update () 
    {
        startTime += Time.deltaTime;

        if (startTime >= Delay)
        {
            gameObject.GetComponent<ParticleEmitter>().emit = true;
        }
	}
}
