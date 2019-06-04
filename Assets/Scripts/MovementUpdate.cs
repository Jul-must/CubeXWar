using UnityEngine;
using System.Collections;

public class MovementUpdate : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Transform myTransform;

    public int framesPerUpdate = 2;
    //gets CurrentAnimation updated in the EnemyMovement script.

    int counter;

    // Use this for initialization
    void Start()
    {
        if (networkView.isMine)
        {
            myTransform = transform;

            networkView.RPC("UpdateMovement", RPCMode.Others, myTransform.position,
                myTransform.rotation);
        }
        else
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if (counter == framesPerUpdate)
        {
            networkView.RPC("UpdateMovement", RPCMode.Others, myTransform.position,
                myTransform.rotation);

            counter = 0;
        }
    }

    [RPC]
    void UpdateMovement(Vector3 newPosition, Quaternion newRotation)
    {
        transform.position = newPosition;
        transform.rotation = newRotation;
    }
}