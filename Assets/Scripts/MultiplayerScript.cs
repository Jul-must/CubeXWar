using UnityEngine;
using System.Collections;

/// <summary>
/// this script is attatched to the multiplayer manager.
/// this is the foundation for our multiplayer connection.
/// </summary> 

public class MultiplayerScript : MonoBehaviour
{
    private string titelMessage = "";
    private string connectToIP;
    private int connectionPort = 26500;
    private bool useNAT = false;
    private string ipAdress;
    private string port;
    private int numberOfPlayers = 10;
    private int winningScore = 9999999;
    private int maxPlayerNameLenght = 15;

    public string playerName;
    public string serverName;
    public string serverNameForClient;

    private bool IWantToSetUpAServer = false;
    private bool IWantToConnectToAServer = false;
    private bool IWantToChangeOptions = false;
    private bool IWantToChangeInGameOptions = false;



    //main window
    private Rect connectionWindowRect;
    private int connectionWindowWidth = 400;
    private int connectionWindowHeight = 350;
    private int buttonHeight = 60;
    private int leftIndent;
    private int topIndent;

    //shutdown window
    private Rect serverDisWindowRect;
    private int serverDisWindowWidth = 300;
    private int serverDisWindowHeight = 150;
    private int serverDisWindowLeftIndent = 10;
    private int serverDisWindowTopIndent = 10;

    //client disconnect window
    private Rect clientDisWindowRect;
    private int clientDisWindowWidth = 300;
    private int clientDisWindowHeight = 170;

    public bool showDisconnectWindow = false;

    public float Volume = 0.1f;        // GameManager hämtar detta värde i sin Update funktion.

    public GUISkin MySkin;

    // Use this for initialization
    void Start()
    {
        serverName = PlayerPrefs.GetString("ServerName");
        connectToIP = PlayerPrefs.GetString("ServerIP");
        numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers");
        winningScore = PlayerPrefs.GetInt("WinningScore");
        playerName = PlayerPrefs.GetString("PlayerName");

        if (!PlayerPrefs.HasKey("BackgroundVolume"))         // if the player have yet to set the options for volume then set the volume to 0.1 (a good Volume)
        {
            PlayerPrefs.SetFloat("BackgroundVolume", 0.1f);
        }
        Volume = PlayerPrefs.GetFloat("BackgroundVolume");

        if (connectToIP == "")
        {
            connectToIP = "127.0.0.1";
        }

        if (serverName == "")
        {
            serverName = "Server";
        }

        if (playerName == "")
        {
            playerName = "N00b";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showDisconnectWindow = !showDisconnectWindow;
            IWantToChangeInGameOptions = false;
        }
    }

    void ConnectWindow(int windowID)  // --------------------------------------------------------------- connection (first window)
    {
        // leav a gap from the header.

        GUILayout.Space(15);

        // when the player starts the game the player have the option 
        //to connect or start a server.

        if (IWantToSetUpAServer == false && IWantToConnectToAServer == false && IWantToChangeOptions == false)
        {
            if (GUILayout.Button("Setup a server", GUILayout.Height(buttonHeight)))
            {
                IWantToSetUpAServer = true;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight)))
            {
                IWantToConnectToAServer = true;
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Options", GUILayout.Height(buttonHeight)))
            {
                IWantToChangeOptions = true;
            }

            GUILayout.Space(10);

            if (Application.isWebPlayer == false && Application.isEditor == false)
            {
                if (GUILayout.Button("Exit", GUILayout.Height(buttonHeight)))
                {
                    Application.Quit();
                }
            }
        }

        if (IWantToChangeOptions == true)
        { 
                       //*100 så att volymen blir till 0-100.
            string t = ((int)(Volume * 100)).ToString();           // to set a max numer amount shown. not nice if there was 10 decimals.

            GUILayout.Label("Background Music Volume :  " + t);  

            Volume = GUILayout.HorizontalSlider(Volume, 0, 1);

            GUILayout.Space(10);

            if (GUILayout.Button("Go Back", GUILayout.Height(30)))
            {
                PlayerPrefs.SetFloat("BackgroundVolume", Volume);
                IWantToChangeOptions = false;
            }
        }


        if (IWantToSetUpAServer == true)
        {
            //The user can type a name of thier server into the text field.

            GUILayout.Label("Enter a name for your server");

            serverName = GUILayout.TextField(serverName);

            GUILayout.Space(5);

            //Port number

            GUILayout.Label("Server Port");

            connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));

            GUILayout.Space(20);

            //Number Of Players

            GUILayout.Label("Players");

            numberOfPlayers = int.Parse(GUILayout.TextField(numberOfPlayers.ToString()));

            //Score to win

            GUILayout.Label("Winning Score");

            winningScore = int.Parse(GUILayout.TextField(winningScore.ToString()));

            GUILayout.Space(2);

            if (GUILayout.Button("Start my server", GUILayout.Height(30)))
            {
                //några saker måste vara uppfylda för att servern ska få starta.
                if (winningScore != 0 && numberOfPlayers != 0)
                {
                    //Creat server

                    Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);

                    //Creat server nödvändigheter

                    //har så att man kan se att denna client är servern.
                    GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName = "Server";

                    //Sätter alla variabler som blev valda tidiagre.
                    GameObject.Find("GameManager").GetComponent<GameManager>().ScoreToWin = winningScore;

                    //spartanKnug spawnas så att han tillhör servern.
                    //Network.Instantiate(SpartanKing, GameObject.Find("SpartanSpawn").transform.position, GameObject.Find("SpartanSpawn").transform.rotation, 0);

                    //Save the settings using playerPrefs.

                    PlayerPrefs.SetString("ServerName", serverName);
                    PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);
                    PlayerPrefs.SetInt("WinningScore", winningScore);

                    IWantToSetUpAServer = false;
                }
            }

            if (GUILayout.Button("Go Back", GUILayout.Height(30)))
            {
                IWantToSetUpAServer = false;
            }
        }

        //så att spelaren inte har för långt namn.
        if (playerName.Length > maxPlayerNameLenght)
        {
            playerName = playerName.Substring(0, maxPlayerNameLenght);
        }

        //when you connect to a server.
        if (IWantToConnectToAServer == true)
        {
                GUILayout.Label("Enter your player name");

                playerName = GUILayout.TextField(playerName);

                GUILayout.Space(5);

                GUILayout.Label("Type in server IP");

                connectToIP = GUILayout.TextField(connectToIP);

                GUILayout.Space(5);

                GUILayout.Label("Server Port");

                connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));

                GUILayout.Space(5);

                //Connection button

                if (GUILayout.Button("Connect", GUILayout.Height(25)))
                {
                    if (GameObject.Find("GameManager").GetComponent<GameManager>().GameOver == false)
                    {
                        if (playerName == "")
                        {
                            playerName = "Target";
                        }
                        else
                        {
                            Network.Connect(connectToIP, connectionPort);

                            PlayerPrefs.SetString("PlayerName", playerName);
                            PlayerPrefs.SetString("ServerIP", connectToIP);
                        }
                    }
                }
            

            if (GUILayout.Button("Go Back", GUILayout.Height(30)))
            {
                IWantToConnectToAServer = false;
            }
        }
    }

    void ClientDisconnectWindow(int WindowID)
    {
        if (IWantToChangeInGameOptions)
        {
                             //*100 så att volymen blir till 0-100.
            string t = ((int)(Volume*100)).ToString();           // to set a max numer amount shown. not nice if there was 10 decimals.

            GUILayout.Label("Background Music Volume :  " + t);

            Volume = GUILayout.HorizontalSlider(Volume, 0, 1);

            GUILayout.Space(10);

            if (GUILayout.Button("Go Back", GUILayout.Height(30)))
            {
                PlayerPrefs.SetFloat("BackgroundVolume", Volume);
                IWantToChangeInGameOptions = false;
            } 
        }
        if (!IWantToChangeInGameOptions)
        {
            GUILayout.Label("Connected to server: " + serverName);

            GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));

            GUILayout.Space(7);

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
            }

            if (GUILayout.Button("Options"))
            {
                Debug.Log("OptionButton");
                IWantToChangeInGameOptions = true;
                                                          // måste fixa. ???
            }

            if (GUILayout.Button("Return to game", GUILayout.Height(25)))
            {
                showDisconnectWindow = false;
            }
        }
    }

    void ServerDisconnectWindow(int WindowID)
    {
        GUILayout.Label("Server name: " + serverName);

        GUILayout.Label("Numers of players connected: " + Network.connections.Length);

        if (Network.connections.Length >= 1)
        {
            GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
        }

        if (GUILayout.Button("Shutdown server"))
        {
            Network.Disconnect();
        }
    }

    void OnDisconnectedFromServer()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnPlayerDisconnected(NetworkPlayer networkPlayer)
    {
        Network.RemoveRPCs(networkPlayer);
        Network.DestroyPlayerObjects(networkPlayer);
    }

    void OnPlayerConnected(NetworkPlayer networkPlayer)
    {
        networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
    }

    void OnGUI()
    {
        // to set the Font to the MainFont
        GUI.skin = MySkin;

        //if the player is dissconected then run the connect window function.

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //Determin the position of the window

            leftIndent = Screen.width / 2 - connectionWindowWidth / 2;
            topIndent = Screen.height / 2 - connectionWindowHeight / 2;

            connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth,
                connectionWindowHeight);

            connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titelMessage);
        }

        if (Network.peerType == NetworkPeerType.Server)
        {
            serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowWidth
                , serverDisWindowHeight);

            serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
        }

        if (Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
        {
            clientDisWindowRect = new Rect(Screen.width / 2 - clientDisWindowWidth / 2, Screen.height / 2
                - clientDisWindowHeight / 2, clientDisWindowWidth, clientDisWindowHeight);

            clientDisWindowRect = GUILayout.Window(1, clientDisWindowRect, ClientDisconnectWindow, "");
        }
    }

    [RPC]
    void TellPlayerServerName(string serverName)
    {
        this.serverName = serverName;
    }
}