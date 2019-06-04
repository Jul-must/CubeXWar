using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour 
{
    // sitter på spelaren
    // tar hand om spelarens poäng / deaths
    public bool scored;
    public bool died;
	// Use this for initialization
	void Start () 
    {
        if (networkView.isMine)
        {
        }
        else
        {
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (scored)
        {
            GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerScore++;
            GameObject.Find("GameManager").GetComponent<PlayerDataBase>().scored = true;
            scored = false;
        }

        if (died)
        {
            GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerDeaths++;
            GameObject.Find("GameManager").GetComponent<PlayerDataBase>().died = true;
            died = false;
        }
	}
}
