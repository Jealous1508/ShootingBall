using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class AddBall : MonoBehaviour
{
    public GameObject partical;
    public GameObject Ball;
    

    private void Update()
    {
        if (this.gameObject.transform.position.y <= -5.5f)
            GameMngerS.gamestate = GameState.GameOver;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            GameObject k = Instantiate(partical, transform.position, Quaternion.identity);
            Destroy(k, 3f);
            GameMngerS.ListBox.Remove(this.gameObject);
            
            GameObject m = Instantiate(Ball, new Vector3(this.transform.position.x, -5.8f, 0), Quaternion.identity) as GameObject;
            m.GetComponent<Rigidbody2D>().isKinematic = true;
            m.GetComponent<SpriteRenderer>().color = Color.cyan;
            BotScript.ReadyList.Add(m);
            Destroy(this.gameObject);
        }
    }
}


