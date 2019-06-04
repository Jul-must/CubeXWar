using UnityEngine;
using System.Collections;
using System;

public class TeleportSpell : MonoBehaviour
{
    public string owner;
    public int teleportDistance = 15;

    private GameObject OwnerGameObject;

    public void Start()
    {
        owner = gameObject.GetComponent<SpellScript>().Owner;
        OwnerGameObject = GameObject.Find(owner);


        gameObject.transform.position = OwnerGameObject.transform.position;

        //teleport
        OwnerGameObject.transform.position += gameObject.transform.forward * teleportDistance;

        //to hinder the player from wakling back to its former position.
        OwnerGameObject.GetComponent<PlayerMover>().targetPoint = GameObject.Find(owner).transform.position;

        //destroy after finnished teleporting.
        Destruct(2);
    }

    private void Destruct(float t)
    {
        Destroy(gameObject, t);
    }

    public void Update()
    {

    }
}