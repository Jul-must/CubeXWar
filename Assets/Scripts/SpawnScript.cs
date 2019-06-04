using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// this script allows the player to spawn into the multiplayer game.
/// </summary>

public class SpawnScript : MonoBehaviour 
{
    private bool justConnectedToServer = false;
    public bool iAmOnNoTeam = false;
    public bool amIOnTheRedTeam = false;
    public bool amIOnTheBlueTeam = false;

    //the joinTeamWindow

    private Rect joinTeamRect;
    private string joinTeamWindowTitle = "Team Selection";
    private int joinTeamWindowWidth = 330;
    private int joinTeamWindowHeight = 100;
    private int joinTeamLeftIndent;
    private int joinTeamTopIndent;
    private int buttonHeight = 40;

    //player prefabs are connected to thease in the inspector

    public Transform redTeamPlayer;
    public Transform blueTeamPlayer;
    public Transform Hero1;
    public Transform HUD;

    private GameObject[] redSpawnPoints;
    private GameObject[] blueSpawnPoints;

    //My own 
    public bool playerIsDead = false;
    public string playerToRespawn;
    Rect respawnRect;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnConnectedToServer()
    {
        justConnectedToServer = true;
    }

    void RespawnWindow(int WindowID)
    {
        if (GUILayout.Button("Respawn", GUILayout.Height(buttonHeight)))
        {
            Respawn();
        }
    }

    void JoinTeamWindow(int WindowID)
    {
        //if (GUILayout.Button("Join Red Team", GUILayout.Height(buttonHeight)))
        //{
        //    amIOnTheRedTeam = true;
        //    justConnectedToServer = false;
        //    SpawnRedPlayer();
        //}
        //if (GUILayout.Button("Join Blue Team", GUILayout.Height(buttonHeight)))
        //{
        //    amIOnTheBlueTeam = true;
        //    justConnectedToServer = false;
        //    SpawnBluePlayer();
        //}

        if (GUILayout.Button("Join FFA", GUILayout.Height(buttonHeight)))
        {
            iAmOnNoTeam = true;
            justConnectedToServer = false;
            SpawnFFAPlayer(false);
        }
    }

    void OnGUI()
    {
        if (justConnectedToServer)
        {
            joinTeamLeftIndent = Screen.width / 2 - joinTeamWindowWidth / 2;
            joinTeamTopIndent = Screen.height / 2 - joinTeamWindowHeight / 2;

            joinTeamRect = new Rect(joinTeamTopIndent, joinTeamLeftIndent, 
                joinTeamWindowWidth, joinTeamWindowHeight);

            joinTeamRect = GUILayout.Window(0, joinTeamRect, JoinTeamWindow, 
                joinTeamWindowTitle);
        }

        if (playerIsDead)
        {
            if (!GameObject.Find("GameManager").GetComponent<GameManager>().GameOver)
            {
                joinTeamLeftIndent = Screen.width / 2 - joinTeamWindowWidth / 2;
                joinTeamTopIndent = Screen.height / 2 - joinTeamWindowHeight / 2;

                respawnRect = new Rect(joinTeamTopIndent, joinTeamLeftIndent,
                    joinTeamWindowWidth, joinTeamWindowHeight);
                respawnRect = GUILayout.Window(0, respawnRect, RespawnWindow, "");
            }
        }
    }

    void SpawnRedPlayer()
    {
        //find all red spawnpoints
        redSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnRedTeam");

        // randomly pick a spawnpoint

        GameObject randomRedSpawn = redSpawnPoints[Random.Range(0, redSpawnPoints.Length)];

        Network.Instantiate(Hero1, randomRedSpawn.transform.position, 
            randomRedSpawn.transform.rotation, 0);
    }

    void SpawnBluePlayer()
    {
        //find all blue spawnpoints
        blueSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnBlueTeam");

        // randomly pick a spawnpoint

        GameObject randomBlueSpawn = blueSpawnPoints[Random.Range(0, blueSpawnPoints.Length)];

        Network.Instantiate(Hero1, randomBlueSpawn.transform.position,
            randomBlueSpawn.transform.rotation, 0);
    }

    void SpawnFFAPlayer(bool Respawned)
    {
        GameObject randomSpawn;
        if (Random.Range(0, 2) == 1)
        {
            blueSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnBlueTeam");

            // randomly pick a spawnpoint

            randomSpawn = blueSpawnPoints[Random.Range(0, blueSpawnPoints.Length)];
        }
        else
        {
            redSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnRedTeam");

            // randomly pick a spawnpoint

            randomSpawn = redSpawnPoints[Random.Range(0, redSpawnPoints.Length)];
        }

        Network.Instantiate(Hero1, randomSpawn.transform.position,
            randomSpawn.transform.rotation, 0);
        //if (Respawned)
        //{
        //    Debug.Log("SpellsWorking");
        //    GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName).GetComponent<SpellCaster>().
        //        BuySpells(GameObject.Find("GameManager").GetComponent<GameManager>().SavedSpells);
        //}
        GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName).GetComponent<SpellCaster>().Reset();  //to make sure all spells are off cooldown when the game starts

        //turn the HUD ON
        GameObject.Find("HUD").GetComponent<HUDScript>().ON = true;
    }

    void Respawn()
    {
        Debug.Log("Respawned");

        GameObject playerToRespawn = GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName);
        playerIsDead = false;

        //used for resetSpawntype. start
        //save the spells the player had when he died.
        //GameObject.Find("GameManager").GetComponent<GameManager>().SaveSpells(playerToRespawn.GetComponent<SpellCaster>().GetSpells());
        //playerToRespawn.GetComponent<HealthAndDamage>().DestroyPlayer();
        //Destroy(GameObject.Find("Healthbar(Clone)"));
        //GameObject.Find("HUD").GetComponent<HUDScript>().FlushSpells();
        //Network.RemoveRPCs(Network.player);
        //end

        //NetworkViewID Vid = GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName).GetComponent<NetworkView>().viewID;
        //networkView.RPC("DestroyEveryWhere", RPCMode.Others, GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName, Vid);  // player destruktion

        GameObject randomSpawn;
        if (Random.Range(0, 2) == 1)
        {
            blueSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnBlueTeam");

            // randomly pick a spawnpoint

            randomSpawn = blueSpawnPoints[Random.Range(0, blueSpawnPoints.Length)];
        }
        else
        {
            redSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnRedTeam");

            // randomly pick a spawnpoint

            randomSpawn = redSpawnPoints[Random.Range(0, redSpawnPoints.Length)];
        }

        playerToRespawn.transform.position = randomSpawn.transform.position;
        playerToRespawn.GetComponent<HealthAndDamage>().Reset();
        playerToRespawn.GetComponent<SpellCaster>().Reset();


        //used for resetSpawntype. start
        //SpawnFFAPlayer(true);
    }

    [RPC]
    void DestroyEveryWhere(string player, NetworkViewID Vid)
    {
        //Network.RemoveRPCs(GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName).GetComponent<NetworkView>().viewID);
        //Network.Destroy(GameObject.Find(player));
    }

    [RPC]
    void ResetPlayerColor(string player)
    {
        Color oldColor = GameObject.Find(player).GetComponentInChildren<Renderer>().material.color;
        Color newColor = new Color(oldColor.r, oldColor.b, oldColor.g, 1f);
        GameObject.Find(player).GetComponentInChildren<Renderer>().material.SetColor("_Color", newColor);                        // så att spelaren inte är transparant när han spawnar.
    }
}