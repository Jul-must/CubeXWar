using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster : MonoBehaviour 
{
    //must be named Fireball becuz of initiate.
    public GameObject Fireball;
    public GameObject TeleportSpell;
    public GameObject DiskSpray;
    public GameObject SunStrike;
    public GameObject WallSpell;
    public GameObject ConeSpell;
    private Transform myTransform;
    public List<SpellScript> spells = new List<SpellScript>();

	// Use this for initialization
	void Start () 
    {
        if (networkView.isMine)
        {
            myTransform = transform;
            spells.Add(null);
            spells.Add(null);
            spells.Add(null);
            spells.Add(null);
        }
        else
        {
            enabled = false;
        }
	}

    public void BuySpells(List<SpellScript> OldSpells)
    {
        //Debug.Log(OldSpells.Count);
        //for (int i = 0; i < OldSpells.Count; i++)
        //{
        //    Debug.Log(OldSpells[i].spellName);
        //    if (OldSpells[i].spellName == "Fireball")
        //    {
        //        Fireball.GetComponent<SpellScript>().Initialize();
        //        OldSpells[i] = Fireball.GetComponent<SpellScript>();
        //    }
        //    if (OldSpells[i].spellName == "TeleportSpell")
        //    {
        //        TeleportSpell.GetComponent<SpellScript>().Initialize();
        //        OldSpells[i] = TeleportSpell.GetComponent<SpellScript>();
        //    }
        //    if (OldSpells[i].spellName == "DiskSpray")
        //    {
        //        DiskSpray.GetComponent<SpellScript>().Initialize();
        //        OldSpells[i] = DiskSpray.GetComponent<SpellScript>();
        //    }
        //    if (OldSpells[i].spellName == "SunStrike")
        //    {
        //        SunStrike.GetComponent<SpellScript>().Initialize();
        //        OldSpells[i] = SunStrike.GetComponent<SpellScript>();
        //    }            
        //}
    }
	
	// Update is called once per frame
    void Update()
    {
        foreach (SpellScript spell in spells)
        {
            if (spell != null)
            {
                spell.UpdateCoolDown();
            }
        }

        if (!GameObject.Find("GameManager").GetComponent<CursorControl>().locked)
        {
            if (!gameObject.GetComponent<HealthAndDamage>().limbo)
            {
                UpdateBuyKeys();

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    SpawnSpell(0);
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    SpawnSpell(1);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SpawnSpell(2);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SpawnSpell(3);
                }
            }
            else
            {
                foreach (SpellScript spell in spells)
                {
                    if (spell != null)
                    {
                        spell.OnCD = false;
                    }
                }
            }
        }
    }

    public List<SpellScript> GetSpells()
    {
        return spells;
    }

    void SpawnSpell(int TheSpellNumber)
    {
        if (spells[TheSpellNumber] != null)
        {
            if (!spells[TheSpellNumber].OnCD)
            {
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                Vector3 targetPoint = new Vector3();
                if (playerPlane.Raycast(ray, out hitdist))
                {
                    targetPoint = ray.GetPoint(hitdist);
                }
                spells[TheSpellNumber].OnCD = true;
                networkView.RPC("Spawn" + spells[TheSpellNumber].name, RPCMode.All, myTransform.transform.position, myTransform.transform.rotation,
                    targetPoint, GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName);
            }
        }
    }

    void UpdateBuyKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("Fireball"))
                    {
                        Fireball.GetComponent<SpellScript>().Initialize();
                        spells[i] = Fireball.GetComponent<SpellScript>();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("TeleportSpell"))
                    {
                        TeleportSpell.GetComponent<SpellScript>().Initialize();
                        spells[i] = TeleportSpell.GetComponent<SpellScript>();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("DiskSpray"))
                    {
                        DiskSpray.GetComponent<SpellScript>().Initialize();
                        spells[i] = DiskSpray.GetComponent<SpellScript>();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("SunStrike"))
                    {
                        SunStrike.GetComponent<SpellScript>().Initialize();
                        spells[i] = SunStrike.GetComponent<SpellScript>();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("WallSpell"))
                    {
                        WallSpell.GetComponent<SpellScript>().Initialize();
                        spells[i] = WallSpell.GetComponent<SpellScript>();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i] == null)
                {
                    if (CheckIfYouHaveSpell("ConeSpell"))
                    {
                        ConeSpell.GetComponent<SpellScript>().Initialize();
                        spells[i] = ConeSpell.GetComponent<SpellScript>();
                    }
                }
            }
        }
    }

    bool CheckIfYouHaveSpell(string nameOfSpell)
    {
        foreach (SpellScript spell in spells)
        {
            if (spell != null)
            {
                if (spell.spellName == nameOfSpell)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Reset()
    {
        foreach (SpellScript spell in spells)
        {
            Debug.Log("SpellCD_reset");
            spell.OnCD = false;
        }
    }

    [RPC]
    void SpawnFireball(Vector3 PlayerPos, Quaternion playerRotation, Vector3 targetPoint, string owner)
    {
        GameObject obj = (GameObject)Instantiate(Fireball, PlayerPos, playerRotation);
        obj.transform.LookAt(targetPoint);
		obj.GetComponent<SpellScript>().Owner = owner;
    }

    [RPC]
    void SpawnTeleportSpell(Vector3 PlayerPos, Quaternion playerRotation, Vector3 targetPoint, string owner)
    {
        GameObject obj = (GameObject)Instantiate(TeleportSpell, PlayerPos, playerRotation);
        obj.transform.LookAt(targetPoint);
        obj.GetComponent<SpellScript>().Owner = owner;
    }

    [RPC]
    void SpawnDiskSpray(Vector3 PlayerPos, Quaternion playerRotation, Vector3 targetPoint, string owner)
    {
        GameObject obj = (GameObject)Instantiate(DiskSpray, PlayerPos, playerRotation);
        obj.transform.LookAt(targetPoint);
        obj.GetComponent<SpellScript>().Owner = owner;
    }

    [RPC]
    void SpawnSunStrike(Vector3 PlayerPos, Quaternion playerRotation, Vector3 MouseToWorldPosition, string owner)
    {
            GameObject obj = (GameObject)Instantiate(SunStrike, MouseToWorldPosition, Quaternion.identity);
            obj.GetComponent<SpellScript>().Owner = owner;
    }

    [RPC]
    void SpawnWallSpell(Vector3 PlayerPos, Quaternion playerRotation, Vector3 MouseToWorldPosition, string owner)
    {
        GameObject obj = (GameObject)Instantiate(WallSpell, MouseToWorldPosition, Quaternion.identity);
        obj.GetComponent<SpellScript>().Owner = owner;
    }

    [RPC]
    void SpawnConeSpell(Vector3 PlayerPos, Quaternion playerRotation, Vector3 MouseToWorldPosition, string owner)
    {
        GameObject obj = (GameObject)Instantiate(ConeSpell, PlayerPos, Quaternion.identity);
        obj.transform.LookAt(MouseToWorldPosition);
        obj.transform.Rotate(0, 90, 0);
        obj.GetComponent<SpellScript>().Owner = owner;
    }
}
