using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour 
{
	public string target;
	public float attackTime = 1.0f;
	public float coolDown = 2.0f;
    public float damage = 10;

	void Start () 
    {
        if (networkView.isMine)
        {
            gameObject.name = "SpartanKnug"; //------------------------------------låt stå för att koden letar efter namnet SpartanKnug
        }
        else
        {
            enabled = false;
        }
	}

    void Update()
    {
        if (gameObject.GetComponent<EnemyMovement>().Target != null)
        {
            target = gameObject.GetComponent<EnemyMovement>().Target.name;
            networkView.RPC("UpdateTarget", RPCMode.Others, target);

            if (attackTime == 0)
            {
                Attack();
                attackTime = coolDown;
            }
        }
        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        if (attackTime < 0)
            attackTime = 0;
    }
	
 	private void Attack() 
    {
		float distance = Vector3.Distance(GameObject.Find(target).transform.position, transform.position);
		
		
		Vector3 dir = (GameObject.Find(target).transform.position - transform.position).normalized;
		float direction = Vector3.Dot(dir, transform.forward);
		
				
		if(distance < 4.5f) 
        {
			if(direction > 0) 
            {
                networkView.RPC("AttackEverywhere", RPCMode.All);
			}
		}
    }

    [RPC]
    void AttackEverywhere()
    {
        animation.Play("attack");
        GameObject.Find(target).GetComponent<HealthAndDamage>().TakeDamage(damage, "SpartanKnug");
    }

    [RPC]
    void UpdateTarget(string newTarget)
    {
        target = newTarget;
    }
}