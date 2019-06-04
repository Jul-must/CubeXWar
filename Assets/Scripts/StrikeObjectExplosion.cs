using UnityEngine;
using System.Collections;

public class StrikeObjectExplosion : MonoBehaviour 
{
    public float Timer = 0;
    public float DelayTime = 0;
    public ParticleSystem LoadingUpParticle;
    public ParticleSystem ExplosionParticle;

    void Start ()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }
	
	void Update () 
    {
        Timer += Time.deltaTime;
        if (Timer >= DelayTime)
        {
            LoadingUpParticle.enableEmission = false;
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        if (Timer >= DelayTime + 0.4f)
        {
            Destroy(gameObject);
        }
	}
}
