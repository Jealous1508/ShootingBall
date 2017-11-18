using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonScript : MonoBehaviour
{

    public GameObject AddBall;
    public GameObject Partical;

    private void Update()
    {
        if(this.gameObject.transform.position.y <=-5.5f)
        {
            GameMngerS.gamestate = Assets.Scripts.GameState.GameOver;
        }
        transform.Rotate(new Vector3(0, 0, 270*Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            GameObject gsj = Instantiate(AddBall, new Vector3((this.transform.position.x), -5.8f, 0f), Quaternion.identity) as GameObject;
            gsj.GetComponent<Rigidbody2D>().isKinematic = true;
            gsj.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
            BotScript.ReadyList.Add(gsj);

            GameObject gsj1 = Instantiate(AddBall, new Vector3((this.transform.position.x+0.3f), -5.8f, 0f), Quaternion.identity) as GameObject;
            gsj1.GetComponent<Rigidbody2D>().isKinematic = true;
            gsj1.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
            BotScript.ReadyList.Add(gsj1);

            GameObject gsj2 = Instantiate(AddBall, new Vector3((this.transform.position.x-0.3f), -5.8f, 0f), Quaternion.identity) as GameObject;
            gsj2.GetComponent<Rigidbody2D>().isKinematic = true;
            gsj2.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
            BotScript.ReadyList.Add(gsj2);


            Destroy((Instantiate(Partical, this.transform.position, Quaternion.identity) as GameObject),3f);
            Destroy((Instantiate(Partical, this.transform.position + new Vector3(0.2f, 0f, 0f), Quaternion.identity) as GameObject),3f);
            Destroy((Instantiate(Partical, this.transform.position + new Vector3(-0.2f, 0f, 0f), Quaternion.identity) as GameObject),3f);

            Destroy(this.gameObject);
            GameMngerS.ListBox.Remove(this.gameObject);

        }
        else return;

    }
}
