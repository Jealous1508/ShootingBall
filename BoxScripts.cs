using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxScripts : MonoBehaviour
{

    int BoxHp;
    SpriteRenderer spriteR;
    Text HpText;
    AudioSource SoundPlay;
    bool sound;
    bool easy;

    private void OnEnable()
    {
        easy = UI2.Easy;
        spriteR = GetComponent<SpriteRenderer>();
        HpText = GetComponentInChildren<Text>();
        int k=0;
        if (easy)
        {
            k = Mathf.RoundToInt(Random.Range(1f, 1.8f));
        }
        else
        {
            k = Mathf.RoundToInt(Random.Range(0.8f, 2.6f));
        }
        BoxHp = k * GameMngerS.level;
        DisPlayHp();
        SoundPlay = GetComponent<AudioSource>();
        sound = UI2.sound;
        
	}
		
	void Update ()
    {
        DisPlayHp();
        SetColorBox();
        if (BoxHp <= 0)
        {
            GameMngerS.ListBox.Remove(this.gameObject);
            Destroy(this.gameObject);
            GameMngerS.ListBox.Remove(this.gameObject);
        }
        if (this.gameObject.transform.position.y <= -5.5f)
            GameMngerS.gamestate = Assets.Scripts.GameState.GameOver;
	}

    void SetColorBox()
    {
        if (BoxHp > 0 && BoxHp < 3)
            spriteR.color = Color.cyan;
        else if (BoxHp >= 3 && BoxHp < 8)
            spriteR.color = Color.yellow;
        else if (BoxHp >= 8 && BoxHp < 15)
            spriteR.color = Color.green;
        else if (BoxHp >= 15 && BoxHp < 25)
            spriteR.color = new Color(5, 176, 186);
        else if (BoxHp >= 25 && BoxHp < 40)
            spriteR.color = Color.white;
        else if (BoxHp >= 40 && BoxHp < 60)
            spriteR.color = Color.magenta;
        else if (BoxHp >= 60 && BoxHp < 90)
            spriteR.color = Color.cyan;
        else if (BoxHp >= 90)
            spriteR.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            BoxHp -= 1;
        }
        if(sound)
        SoundPlay.Play();
    }

    void DisPlayHp()
    {
        HpText.text = BoxHp.ToString();
    }
}

