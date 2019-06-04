using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public int ScoreToWin = 9999999;
    public int highestScore;
    public string winner;
    int connectionWindowWidth = 400;
    int connectionWindowHeight = 300;
    public bool GameOver = false;
    public Font MainFont;
    public Font FunFont;
    public bool newPlayer = true;
    public List<SpellScript> SavedSpells = null;
	void Start () 
    {
        Screen.SetResolution(900, 600, false);
        Network.RemoveRPCs(gameObject.GetComponent<PlayerDataBase>().networkPlayer); 
	}
	
	void Update () 
    {
        if (!GameOver)
        {
            gameObject.GetComponent<AudioSource>().volume = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerScript>().Volume;

            if (Network.isServer)
            {
                networkView.RPC("UpdateScoreToWin", RPCMode.Others, ScoreToWin);
            }

            if (GameObject.Find("ScoreBoard").GetComponent<ScoreBoardScript>().PlayerList.Count > 0)
            {
                highestScore = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardScript>().PlayerList[0].playerScore;
                winner = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardScript>().PlayerList[0].playerName;
                if (highestScore >= ScoreToWin)
                {
                    GameOver = true;
                }
            }
            else
            {
                highestScore = 0;
            }
        }

        if (GameOver)
        {
            if (highestScore < ScoreToWin)
            {
                GameOver = false;
            }

            if (Network.isServer)
            {
                Debug.Log("ServerSaysGameOver");
                networkView.RPC("TellEveryoneThatGameIsOverAndWhoWon", RPCMode.Others, GameOver, winner);   // make all the newcommers that enter 
                //the game after it has ended set thier GameOver to true
            }
        }
            networkView.RPC("UpdateMyScore", RPCMode.OthersBuffered, gameObject.GetComponent<PlayerDataBase>().playerScore,
                gameObject.GetComponent<PlayerDataBase>().playerDeaths, gameObject.GetComponent<PlayerDataBase>().playerName);
	}

    public void SaveSpells(List<SpellScript> spells)
    {
        Debug.Log("SpellsSaved!!!! " + spells.Count);
        SavedSpells = spells;
    }

    void OnGUI()
    {
        if (GameOver)
        {
            int leftIndent = Screen.width / 2 - connectionWindowWidth / 2;
            int topIndent = Screen.height / 2 - connectionWindowHeight / 2;

            Rect connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth,
                connectionWindowHeight);

            connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ServerRestartWindow,
                "Game Over");
        }
    }

    void ServerRestartWindow(int WindowID)
    {
        GUILayout.Label("The player to win this game was " + winner);

        if (Event.current.type == EventType.Layout)
        {
            if (Network.isServer)
            {
                if (GUILayout.Button("Restart server"))
                {
                    // lite buggig, vet inte om vi borde ha detta. får stå så länge.

                    List<PlayerDataClass> PList = GameObject.Find("GameManager").GetComponent<PlayerDataBase>().PlayerList;
                    foreach (PlayerDataClass player in PList)
                    {
                        Network.CloseConnection(Network.connections[player.networkPlayer - 1], true);
                    }

                    Application.LoadLevel(Application.loadedLevelName);

                }

                if (GUILayout.Button("Shutdown server"))
                {
                    Network.Disconnect();
                }
            }
            else
            {
                if (GUILayout.Button("Disconnect"))
                {
                    Network.Disconnect();
                }
            }
        }
    }

    [RPC]
    void UpdateMyScore(int score, int deaths, string playerName)
    {
        foreach (PlayerDataClass player in gameObject.GetComponent<PlayerDataBase>().PlayerList)
        {
            if (player.playerName == playerName)
            {
                player.playerScore = score;
                player.playerDeaths = deaths;
            }
        }
    }

    [RPC]
    void UpdateScoreToWin(int newScore)
    {
        ScoreToWin = newScore;
    }

    [RPC]
    void TellEveryoneThatGameIsOverAndWhoWon(bool gameOver, string TheWinner)
    {
        GameOver = gameOver;
        winner = TheWinner;
    }
}
