using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour 
{
    public string playerName;
    public bool newPlayer = true;

   public void Awake()
    {
        // when the player spawns into the game retrive the game from player prefs.
        if (networkView.isMine)
        {
            newPlayer = GameObject.Find("GameManager").GetComponent<GameManager>().newPlayer;
            playerName = PlayerPrefs.GetString("PlayerName");
            Debug.Log("newName" + playerName);
            //check if any other player has the same name as this player, and 
            //if they do assigne a random numer as thier name.

            if (playerName == "SpartanKnug")
            {
                float x = Random.Range(0, 1000);
                playerName = "(" + x.ToString() + ")";

                PlayerPrefs.SetString("PlayerName", playerName);
            }
            //GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject))  Julmust-----------------------------------------------------
            if (newPlayer == true)
            {
                foreach (PlayerDataClass player in GameObject.Find("GameManager").GetComponent<PlayerDataBase>().PlayerList)
                {
                    if (playerName == player.playerName)
                    {
                            float x = Random.Range(0, 1000);
                            playerName = "(" + x.ToString() + ")";

                            PlayerPrefs.SetString("PlayerName", playerName);
                    }
                }
            }

            //update the local game manager so that thier name is appended to the player list
            UpdateLocalGameManager(playerName);

            //Send out an RPC to ensure this player'spell name is correct
            networkView.RPC("UpdateMyNameEverywhere", RPCMode.AllBuffered, playerName);

            GameObject.Find("GameManager").GetComponent<GameManager>().newPlayer = false;
        }
        else
        {
            enabled = false;
        }
    }

    void UpdateLocalGameManager(string pName)
    {
        //tell the player data base tp append this player to the player'spell name to the list

        GameObject gameManager = GameObject.Find("GameManager");

        PlayerDataBase dataScript = gameManager.GetComponent<PlayerDataBase>();

        dataScript.nameSet = true;
        dataScript.playerName = pName;

        // supply the communication window script with the players name.

        CommunicationWindow commScript = gameManager.GetComponent<CommunicationWindow>();
        commScript.playerName = pName;
    }
    [RPC]
    void UpdateMyNameEverywhere(string pName)
    {
        gameObject.name = pName;
        playerName = pName;

        //supply the PlayerLabel script with the player´spell name
        PlayerLabel labelScript = transform.GetComponent<PlayerLabel>();
        labelScript.playerName = pName;
    }
}