using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    private Camera myCamera;
    private float speed = 2;
    // Use this for initialization
    void Start()
    {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //This only applyes to the players
        if (GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName != "")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                myCamera.transform.Translate(0, 0, speed, Space.World);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                myCamera.transform.Translate(0, 0, -speed, Space.World);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                myCamera.transform.Translate(-speed, 0, 0, Space.World);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                myCamera.transform.Translate(speed, 0, 0, Space.World);
            }

            myCamera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * -speed, 0, Space.World); 
        }
    }
}
