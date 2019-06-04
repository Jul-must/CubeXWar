using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreBoardScript : MonoBehaviour 
{
    private Rect windowRect;

    private int windowLeft = 200;
    private int windowTop = 30;
    private int windowWidth = 450;
    private int windowHeight = 300;
    private int padding = 20;
    private int textFieldHeight = 30;
    private Vector2 scrollPosition;
    private GUIStyle myStyle = new GUIStyle();

    public List<PlayerDataClass> PlayerList;

    public bool Hide = true;

    public GUISkin MySkin;
	// Use this for initialization
	void Start () 
    {
        PlayerList = GameObject.Find("GameManager").GetComponent<PlayerDataBase>().PlayerList;	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            Hide = false;
        }
        else
        {
            Hide = true;
        }

        PlayerList = GameObject.Find("GameManager").GetComponent<PlayerDataBase>().PlayerList;

        //Keeping the player with the highest score on top of the list
        bool swaped = true;
        while (swaped)
        {
            swaped = false;
            for (int i = 1; i < PlayerList.Count; i++)
            {
                if (PlayerList[i - 1].playerScore < PlayerList[i].playerScore)
                {
                    PlayerDataClass p = PlayerList[i - 1];
                    PlayerList[i - 1] = PlayerList[i];
                    PlayerList[i] = p;
                    swaped = true;
                }
            }
        }
	}

    void ScoreboardWindow(int windowID)
    {
        //Begin a croll view so that as the label incresses 
        //with length the croll bar will 
        //appear and allow the player to view all scores

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowWidth - padding), GUILayout.Height(windowHeight - padding - 5));

            foreach (PlayerDataClass player in PlayerList)
            {
                // we need a padding to make the score appear on the same spot even if the players have a different length on their name.
                GUILayout.Label(player.playerName.PadRight(18, ' ') + " Kills/Deaths : " + player.playerScore + "/" + player.playerDeaths, myStyle);
            }
            GUILayout.EndScrollView();
    }

    void OnGUI()
    {
        GUI.skin = MySkin;
        if (!Hide)
        {
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                int newWinTop = Screen.height - windowHeight - textFieldHeight + windowTop;
                int newWinLeft = Screen.width/2 - windowWidth + windowLeft;
                windowRect = new Rect(windowLeft, windowTop, windowWidth, windowHeight);
                windowRect = GUI.Window(6, windowRect, ScoreboardWindow, "Scoreboard");

                GUILayout.BeginArea(new Rect(newWinLeft, newWinTop + windowHeight, windowWidth, windowHeight));

                GUILayout.EndArea();
            }
        }
    }
}
