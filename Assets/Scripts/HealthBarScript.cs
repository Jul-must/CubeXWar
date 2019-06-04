using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour 
{

    // this scrits is attatched to the healthbar and the healthbar is attatched onto the Health and damage script on the player.
    public GUITexture Curr_Healthbar;
    public GUITexture Back_Healthbar;

    private float healthLength;
    private GameObject player;
    private Transform camTransform;
    private Transform thisTransform;

    private float maxHealth;
    private float currHealth;

    public Vector3 offset = Vector3.up;
	// Use this for initialization
    void Start()
    {
        thisTransform = transform;
        healthLength = Back_Healthbar.GetScreenRect().width;
        currHealth = maxHealth;
        camTransform = Camera.main.transform;
        player = GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName);
    }
	
	// Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find(GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName);
        }
        maxHealth = player.GetComponent<HealthAndDamage>().maxHealth;
        currHealth = player.GetComponent<HealthAndDamage>().myHealth;
        float temp;
        temp = healthLength * currHealth / maxHealth;
        Curr_Healthbar.pixelInset = new Rect(Curr_Healthbar.transform.position.x, Curr_Healthbar.transform.position.y, temp, Curr_Healthbar.GetScreenRect().height);

        transform.position = Camera.main.WorldToViewportPoint(player.transform.position + offset);   
    }
}
