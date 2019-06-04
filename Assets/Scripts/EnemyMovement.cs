using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{
    //needs the MovementUpdate Script

    public GameObject Target = null;
    bool Pulled = false;
    public int triggerCounter = 0;
    MovementUpdate movementUpdateScript;
    Transform Spawn;
    
	void Start () 
    {
        if (networkView.isMine)
        {
			Spawn = GameObject.Find("SpartanSpawn").transform;
            movementUpdateScript = gameObject.GetComponent<MovementUpdate>();
        }
        else
        {
            enabled = false;
        }
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hero")
        {
            Pulled = true;
            Target = other.gameObject;
        }
    }
	void Update () 
    {
        float spawnDistance = Vector3.Distance(Spawn.transform.position, transform.position);

        if (Target == null)
        {
            Pulled = false;
        }

        if (Pulled == true)
        {
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            transform.LookAt(Target.transform);
            GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
            if (distance > 4.0f)
            {
                networkView.RPC("UpdateAnimation", RPCMode.OthersBuffered, "walk");
            }
        }
        if (Pulled == false)
        {
            if(spawnDistance > 4.0f)
            transform.LookAt(Spawn);
            GetComponent<NavMeshAgent>().SetDestination(Spawn.position);
            triggerCounter = 0;
            networkView.RPC("UpdateAnimation", RPCMode.OthersBuffered, "walk");

            if (spawnDistance < 2.0f)
            {
                networkView.RPC("UpdateAnimation", RPCMode.OthersBuffered, "idle");
            }
        }
        if (Mathf.Sqrt(((Spawn.transform.position.z - transform.position.z) * (Spawn.transform.position.z - transform.position.z))
            + ((Spawn.transform.position.x - transform.position.x) * (Spawn.transform.position.x - transform.position.x))) > 30)
        {
            Pulled = false;
        }

        //if (triggerCounter <= 1)
        //{
        //    //if (Mathf.Sqrt(((Target.transform.position.z - transform.position.z) * (Target.transform.position.z - transform.position.z))
        //    //    + ((Target.transform.position.x - transform.position.x) * (Target.transform.position.x - transform.position.x))) < 5)
        //    //{
        //        Pulled = true;
        //        triggerCounter += 1;
        //    //}
        //}
	}

    [RPC]
    void UpdateAnimation(string newAnimation)
    {
                animation.Play(newAnimation);
    }
}
