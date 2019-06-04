using UnityEngine;
using System.Collections;

public class CursorControl : MonoBehaviour 
{
    private GameObject multiplayerManager;
    private MultiplayerScript multiplayerScript;

    private GameObject gameManager;
    private CommunicationWindow commScript;

    private GameManager gameManagerScript;

    public bool locked = false;
	// Use this for initialization
	void Start () 
    {
            multiplayerManager = GameObject.Find("MultiplayerManager"); 
            multiplayerScript = multiplayerManager.GetComponent<MultiplayerScript>();

            gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

            gameManager = GameObject.Find("GameManager");

        
	}
	
	// Update is called once per frame
	void Update () 
    {
        multiplayerScript = multiplayerManager.GetComponent<MultiplayerScript>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        commScript = gameManager.GetComponent<CommunicationWindow>();

        if (multiplayerScript.showDisconnectWindow == false || commScript.lockCursor == false || gameManagerScript.GameOver == false)
        {
            locked = false;
        }

        if (multiplayerScript.showDisconnectWindow == true || commScript.lockCursor == true || gameManagerScript.GameOver == true)
        {
            locked = true;
        }
	}
}
