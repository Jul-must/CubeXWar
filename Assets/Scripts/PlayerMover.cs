using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour 
{
    private Transform myTransform;
    public Vector3 targetPoint;
    public Quaternion targetRotation = new Quaternion(0,0,0,0);
    public float rotationSpeed = 5;
    public bool wasDead = true;
    public Vector3 force;
    public Vector3 force2;
	// Use this for initialization
	void Start () 
    {
        if (networkView.isMine)
        {
            myTransform = transform;
            myTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            enabled = false;
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(0,0,0);
        if (!GameObject.Find("GameManager").GetComponent<CursorControl>().locked)
        {
            if (!gameObject.GetComponent<HealthAndDamage>().limbo)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        targetPoint = ray.GetPoint(hitdist);
                        targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    }
                }

                if (wasDead == true)
                {
                    targetPoint = gameObject.transform.position;
                    wasDead = false;
                }

                if (myTransform.rotation.x >= targetRotation.x - 25 || myTransform.rotation.x <= targetRotation.x + 25)
                {
                    GetComponent<NavMeshAgent>().SetDestination(targetPoint);
                }

                float speed = Mathf.Min(rotationSpeed * Time.deltaTime, 0.9f);
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, speed);
            }
            else
            {
                networkView.RPC("SetPlayerDeadColor", RPCMode.All);

                wasDead = true;
            }
        }
    }

    public void DestroyPlayer()
    {
        //Destroy(GameObject.Find("Healthbar(Clone)"));
        //GameObject.Find("HUD").GetComponent<HUDScript>().FlushSpells();
        //Destroy(gameObject);
    }

    [RPC]
    void SetPlayerDeadColor()
    {
        Color oldColor = gameObject.GetComponentInChildren<Renderer>().material.color;
        Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, 0.1f);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", newColor);                        // så att spelaren inte är transparant när han spawnar.

    }
}
