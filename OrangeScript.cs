using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeScript : MonoBehaviour
{
    int HP;
    bool easy;
	void OnEnable ()
    {
        easy = UI2.Easy;
        if (easy)
            HP = Mathf.RoundToInt(GameMngerS.level / 3f) + 1;
        else
            HP = Mathf.RoundToInt(GameMngerS.level / 2f);
	}
	

	void Update ()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
            GameMngerS.ListBox.Remove(this.gameObject);
        }
        transform.Rotate(new Vector3(0, 0, 300 * Time.deltaTime));

        if(this.gameObject.transform.position.y<= -4.5f)
        {
            GameMngerS.gamestate = Assets.Scripts.GameState.GameOver;
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.gameObject.transform.position = new Vector3(col.gameObject.transform.position.x, -4.2f, 0f);
            rb.isKinematic = false;
            rb.velocity = Vector3.down*5f;
            HP = HP - 1;
        }
    }
}
