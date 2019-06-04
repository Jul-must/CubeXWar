using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDScript : MonoBehaviour 
{
    //variabler för att modifiera actionbars.
    public int Height;
    public int Width;

    public List<SpellScript> spells;
    public List<GUITexture> spellTextures = new List<GUITexture>();
    public List<GUIText> spellTexts = new List<GUIText>();

    public GUITexture infoTexture;
    public Texture2D DefaultTexture;
    public Color DefaultColor;

    public bool ON = false; // changed in SpawnScript.
    // Use this for initialization
	void Start () 
    {
        DefaultColor = spellTextures[0].color;
        foreach (GUITexture texture in spellTextures)
        {
            texture.texture = DefaultTexture;
        }

        foreach (GUIText text in spellTexts)
        {
            text.color = Color.black;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (ON)
        {
            infoTexture.enabled = true;
            foreach (GUITexture texture in spellTextures)
            {
                texture.enabled = true;
            }
            foreach (GUIText text in spellTexts)
            {
                text.enabled = true;
            }
            string player = GameObject.Find("GameManager").GetComponent<PlayerDataBase>().playerName;
            spells = GameObject.Find(player).GetComponent<SpellCaster>().spells;

            UpdateActionbars();
        }
        else
        {
            infoTexture.enabled = false;
            foreach (GUITexture texture  in spellTextures)
            {
                texture.enabled = false;
            }
            foreach (GUIText text in spellTexts)
            {
                text.enabled = false;
            }
        }
	}

    public void FlushSpells()
    {
        spells.Clear();
        foreach (GUITexture spellTexture in spellTextures)
        {
            spellTexture.texture = DefaultTexture;
            spellTexture.color = DefaultColor;
        }
    }

    private void UpdateActionbars()  // is run in Update of the HUD script. updates the texture an texts on the actionbars.
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (spells[i] != null)
            {
                spellTextures[i].texture = spells[i].ActionbarTexture;
                if (spells[i].OnCD == true)
                {
                    spellTextures[i].color = Color.gray;
                    string cd = (spells[i].CD - spells[i].timeFromCD).ToString();
                    if (cd.Length > 3)
                    {
                        cd = (spells[i].CD - spells[i].timeFromCD).ToString().Substring(0, 3);// substring to shorten the length of the CD shown on screen
                    }
                    spellTexts[i].text = cd;
                    
                }
                else
                {
                    spellTextures[i].color = new Color(200,200,200);
                }
            }
            else
            {
                spellTextures[i].texture = DefaultTexture;
            }
        }
    }
}