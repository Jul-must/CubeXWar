using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// using System.Collections.Generic gives us the C# List
/// </summary>

public class PlayerDataBase : MonoBehaviour 
{
    public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();

    public NetworkPlayer networkPlayer;

    public bool nameSet = false;
    public string playerName;

    public bool scored = false;
    public int playerScore;

    public bool joinedTeam = false;
    public string playerTeam;

    public bool died = false;
    public int playerDeaths = 0;

    public bool setClientNumber = false;
    public int clientNumber = 0;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (setClientNumber)
        {
            networkView.RPC("EditPlayerListWithClientNumber", RPCMode.AllBuffered, Network.player, clientNumber);
            setClientNumber = false;
        }

        if (nameSet)
        {
            networkView.RPC("EditPlayerListWithName", RPCMode.AllBuffered,
                Network.player, playerName);
            nameSet = false;
        }

        if (scored)
        {
            networkView.RPC("EditPlayerListWithScore", RPCMode.AllBuffered,
                Network.player, playerScore);
            scored = false;
        }

        if (died)
        {
            networkView.RPC("EditPlayerListWithDeaths", RPCMode.AllBuffered, Network.player, playerDeaths);
            died = false;
        }

        if (joinedTeam)
        {
            networkView.RPC("EditPlayerListWithTeam", RPCMode.AllBuffered,
    Network.player, playerTeam);
            joinedTeam = false;
        }
	}

    void OnPlayerConnected(NetworkPlayer netPlayer)
    {
        //add the player to the list. this is executed on the server.

        networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, netPlayer);
    }

    void OnPlayerDisconnected(NetworkPlayer netPlayer)
    {
        networkView.RPC("RemovePlayerFromList", RPCMode.AllBuffered, netPlayer);
    }

    [RPC]
    void AddPlayerToList(NetworkPlayer nPlayer)
    {
        PlayerDataClass capture = new PlayerDataClass();
        capture.networkPlayer = int.Parse(nPlayer.ToString());
        PlayerList.Add(capture);
    }

    [RPC]
    void RemovePlayerFromList(NetworkPlayer nPlayer)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString()))
            {
                PlayerList.RemoveAt(i);
            }
        }
    }

    [RPC]
    void EditPlayerListWithName(NetworkPlayer nPlayer, string pName)
    {
        foreach (PlayerDataClass player in PlayerList)
        {
            if (player.networkPlayer == int.Parse(nPlayer.ToString()))
            {
                player.playerName = pName;
            }
        }
    }

    [RPC]
    void EditPlayerListWithScore(NetworkPlayer nPlayer, int pScore)
    {
        foreach (PlayerDataClass player in PlayerList)
        {
            if (player.networkPlayer == int.Parse(nPlayer.ToString()))
            {
                player.playerScore = pScore;
            }
        }
    }

    [RPC]
    void EditPlayerListWithDeaths(NetworkPlayer nPlayer, int pDeaths)
    {
        foreach (PlayerDataClass player in PlayerList)
        {
            if (player.networkPlayer == int.Parse(nPlayer.ToString()))
            {
                player.playerDeaths = pDeaths;
            }
        }
    }
    [RPC]
    void EditPlayerListWithTeam(NetworkPlayer nPlayer, string pTeam)
    {
        foreach (PlayerDataClass player in PlayerList)
        {
            if (player.networkPlayer == int.Parse(nPlayer.ToString()))
            {
                player.playerTeam = pTeam;
            }
        }
    }

    [RPC]
    void EditPlayerListWithClientNumber(NetworkPlayer nPlayer, int ClientNumber)
    {
        foreach (PlayerDataClass player in PlayerList)
        {
            if (player.networkPlayer == int.Parse(nPlayer.ToString()))
            {
                player.clientNumber = ClientNumber;
            }
        }
    }
}
