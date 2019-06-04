using UnityEngine;
using System.Collections;

public class CommunicationWindow : MonoBehaviour 
{
    //this is supplied by the playername script

    public string playerName;

    //these are used when sending a message

    private string messageToSend;
    private string communication;
    private bool showTextBox = false;
    private bool sendMessage = false;
    private int messagesSent = 0;

    public bool lockCursor = false;

    //these are used to difine the communication window

    private Rect windowRect;

    private int windowLeft = 10;
    private int windowTop;
    private int windowWidth = 300;
    private int windowHeight = 140;
    private int padding = 20;
    private int textFieldHeight = 30;
    private Vector2 scrollPosition;
    private GUIStyle myStyle = new GUIStyle();

    //quick references

    private GameObject spawnManager;
    private SpawnScript spawnScript;

    private Font mainFont;
    private Font funFont;

    private float lastMessageTime = 0;
    private float fadeTime = 5;

    public bool Hide = false;
    public bool DoNotHide = false;

    void Awake()
    {
        //allow my pressing the returnkey to be recognised when using the text field
        Input.eatKeyPressOnTextFieldFocus = false;

        messageToSend = "";

        myStyle.normal.textColor = Color.white;
        myStyle.wordWrap = true;
    }

	// Use this for initialization
	void Start () 
    {
        mainFont = GameObject.Find("GameManager").GetComponent<GameManager>().MainFont;
        funFont = GameObject.Find("GameManager").GetComponent<GameManager>().FunFont;
        myStyle.font = mainFont;

        spawnManager = GameObject.Find("SpawnManager");
        spawnScript = spawnManager.GetComponent<SpawnScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            //if the player presses the T key then set show textbox to true
            if ((Input.GetButtonDown("Send Message")) && showTextBox == false)
            {
                lastMessageTime = Time.realtimeSinceStartup;  //so that you can press enter to "unhide" the chat.
                DoNotHide = true;
                showTextBox = true;
            }
            else if (Input.GetButtonDown("Send Message") && showTextBox == true)
            {
                sendMessage = true;
            }
            //if the player presses SendMessage and textfield is on then send message
            
            if (Time.realtimeSinceStartup - lastMessageTime >= fadeTime && DoNotHide == false)
            {
                Hide = true;
            }
            else
            {
                Hide = false;
            }
        }
	}

    void CommLogWindow(int windowID)
    {
        //Begin a scroll view so that as the label incresses 
        //with length the scroll bar will 
        //appear and allow the player to view past messages
        //only show chat if not hidden.

        if (Hide == false)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowWidth - padding), GUILayout.Height(windowHeight - padding - 5));

            GUILayout.Label(communication, myStyle);

            GUILayout.EndScrollView();
        }
    }

    void OnGUI()
    {
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            windowTop = Screen.height - windowHeight - textFieldHeight;
            windowRect = new Rect(windowLeft, windowTop, windowWidth, windowHeight);

            // Do not display the communication log if the player is not in the game.

            if (spawnScript.iAmOnNoTeam || spawnScript.amIOnTheRedTeam || spawnScript.amIOnTheBlueTeam || Network.isServer)
            {
                windowRect = GUI.Window(5, windowRect, CommLogWindow,"", myStyle);

                GUILayout.BeginArea(new Rect(windowLeft, windowTop + windowHeight, windowWidth, windowHeight));


                // if true then show the text feild that will allow players to write
                if (showTextBox)
                {
                    lockCursor = true;

                    // give the textfiled a name

                    GUI.SetNextControlName("MyTextField");
                    messageToSend = GUILayout.TextField(messageToSend, GUILayout.Width(windowWidth));

                    //give focus to the text field so that the player do not need to click the text field.
                    GUI.FocusControl("MyTextField");

                    if (sendMessage)
                    {
                        //only send if the text field is empty.
                        //if it is empty then we close the textfield

                        if (messageToSend != "")
                        {
                            if (Network.isClient == true)
                            {
                                networkView.RPC("SendMessageToEveryone", RPCMode.All, messageToSend, playerName);
                            }
                            if (Network.isServer)
                            {
                                networkView.RPC("SendMessageToEveryone", RPCMode.All, messageToSend, "Server");
                            }
                        }

                        sendMessage = false;
                        showTextBox = false;
                        lockCursor = false;

                        //reset message

                        messageToSend = "";
                    }
                }

                GUILayout.EndArea();
            }
        }
    }

    [RPC]
    void SendMessageToEveryone(string message, string pName)
    {
        lastMessageTime = Time.realtimeSinceStartup;

        //must check the size before we add the new line, to get the "old" text.
        Vector2 sizeOfLabel = myStyle.CalcSize(new GUIContent(communication));

        //this string is displayed by the label in the comlog window.
        communication = communication + "\n" + pName + " : " + message;

        //if the scroll bar is on the bottom of the scroll befor we add the new line, follow that line with the scroll bar.
        //+115 is becuz they mesure the scroller from the top i think.
        if (scrollPosition.y + 115 >= sizeOfLabel.y)
        {
            scrollPosition.y = Mathf.Infinity;
        }
    }
}
