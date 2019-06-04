using UnityEngine;
using System.Collections;

public class DamageTextScript : MonoBehaviour
{
    float alpha = 1;
    public float speed = 3;
    public float fadeSpeed = 1;

    void Start()
    {
    }
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.Translate(0, speed * Time.deltaTime, 0);
        gameObject.GetComponent<TextMesh>().color = new Color(gameObject.GetComponent<TextMesh>().color.r,
            gameObject.GetComponent<TextMesh>().color.g, gameObject.GetComponent<TextMesh>().color.b, alpha);
        alpha -= fadeSpeed * Time.deltaTime;
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetText(string text)
    {
        gameObject.GetComponent<TextMesh>().text = text;
    }
}