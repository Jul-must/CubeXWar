using UnityEngine;
using System.Collections;

public class PlayerLabel : MonoBehaviour 
{
    //the health bar texture is attatched to this in the inspector

    public Texture healthTex;

    private Camera myCamera;

    private Transform myTransform;
    private Transform triggerTransform;
    private HealthAndDamage HDScript;


    private Vector3 worldPosition = new Vector3();
    private Vector3 screenPosition = new Vector3();
    //private float minimumZ = 1.5f;

    private int labelTop = 18;
    private int labelWidth = 110;
    private int labelHeight = 15;
    private int barTop = 1;
    private float adjustment = 1;

    //used for player name

    public string playerName;
    private GUIStyle myStyle = new GUIStyle();

	// Use this for initialization
	void Start () 
    {
        //this script will only run for the other player characters
        // we do not need a health bar above our own player in our game.
        if (!networkView.isMine)
        {
            myTransform = transform;
            myCamera = Camera.main;

            myStyle.normal.textColor = Color.blue;
            myStyle.fontSize = 12;
            myStyle.fontStyle = FontStyle.Bold;

            //allow the text to extend beyond the width of the label
            myStyle.clipping = TextClipping.Clip;
        }
        else
        {
            enabled = false;
        }

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnGUI()
    {
        worldPosition = new Vector3(myTransform.position.x, myTransform.position.y + adjustment, myTransform.position.z);
        
        screenPosition = myCamera.WorldToScreenPoint(worldPosition);
        //draw the player name.

        GUI.Label(new Rect(screenPosition.x - labelWidth / 2, Screen.height - screenPosition.y - labelTop, labelWidth, labelHeight), playerName, myStyle);
    }
}
