using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using DG.Tweening;

public class BotScript : MonoBehaviour
{
    public static int Number;
    public static Vector3 Position;
    public static int Count;
    public static List<GameObject> ReadyList;

    private void Start()
    {
        Number = -5;
        ReadyList = new List<GameObject>();
        Position = new Vector3(0, -5.8f, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Number >Count) return;
        if (Number == 0)
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            Position = other.gameObject.transform.position;
            Number += 1;
        }
        else
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            other.gameObject.transform.DOMove(Position, 0.5f);
            Number += 1;
        }
        if (Number == Count)
            MoveAddBall();
    } 

    void MoveAddBall()
    {
        if (ReadyList.Count == 0)
        {
            if (Count == 1)
            {
                GameMngerS.gamestate = GameState.InstanBox;
                GameMngerS.LetInstantiateBox = true;
            }
            else
                StartCoroutine(ChangeState());
        }
        else
        {
            for (int i = 0; i < ReadyList.Count; i++)
            {
                ReadyList[i].transform.DOMove(Position, 0.5f);
                ReadyList[i].GetComponent<SpriteRenderer>().color = Color.white;
                GameMngerS.ListBall.Add(ReadyList[i]);
            }
            ReadyList.Clear();
            StartCoroutine(ChangeState());
        }
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(0.6f);
        GameMngerS.gamestate = GameState.InstanBox;
        GameMngerS.LetInstantiateBox = true;
    }
}
