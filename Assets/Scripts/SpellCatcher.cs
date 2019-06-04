using UnityEngine;
using System.Collections;

/// <summary>
/// is on the player. used to detect all the spells that hit the player.
/// </summary>

public class SpellCatcher : MonoBehaviour
{
    string killer;
    // Use this for initialization
    void Start()
    {
        if (networkView.isMine)
        {
        }
        else
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!gameObject.GetComponent<HealthAndDamage>().limbo)
        {
            if (other.gameObject.tag == "Spell")
            {
                if (other.GetComponent<SpellScript>().Owner != gameObject.name)
                {
                    killer = other.GetComponent<SpellScript>().Owner;
                    gameObject.GetComponent<HealthAndDamage>().TakeDamage(other.GetComponent<SpellScript>().GetDMG(), killer);

                    if (other.GetComponent<SpellScript>().destroyOnPlayerImpact == true)
                    {
                        other.GetComponent<SpellScript>().dead = true;
                    }
                }
            }
        }
    }
}
