using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour {
    public GameObject prefab;
	void Start () 
    {
	}
	void Update () 
    {
	}
    public void SpawnText(string text, Vector3 position)
    {
        GameObject TEMP = (GameObject)Network.Instantiate(prefab, position, new Quaternion(), 0);
        TEMP.GetComponent<DamageTextScript>().SetText(text.ToString());
    }
}
