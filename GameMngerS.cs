using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameMngerS : MonoBehaviour
{
    public GameObject Box;
    public GameObject NewBall;
    public Transform[] BoxTransform;
    public GameObject EatBall;
    public GameObject Melon;
    public GameObject Orange;
    public static int level;
    public float speedshoot;
    public GameObject DisplayCount;
    public Text LevelText;
    public Text HighScoreText;
    public Button SpeedBoost;
    public Button TapToReplay;
    public Button MainMenu;
    public Button ContinueButton;
    public Button CallBackBall;
    public GameObject Admob;
    public GameObject line2;

    public static List<GameObject> ListBox;
    public static List<GameObject> ListBall;
    public static GameState gamestate;

    [HideInInspector] public static bool LetInstantiateBox;
    static List<GameObject> clonelist;
    static List<GameObject> abc;
    Text CountText;
    Vector3 FirstPos3, SecondPos3;
    LineRenderer GunLine;
    LineRenderer Gunline2;
    Ray2D RayShoot = new Ray2D();
    AudioSource NextShootAudio;
    bool letshoot;
    int HighScore;
    float timer;
    bool sound=true;
    bool easy;
    public static bool CallAdmob;
    bool okforcontinue;

    private void Awake()
    {
        ListBox = new List<GameObject>();
        ListBall = new List<GameObject>();
        clonelist = new List<GameObject>();
        abc = new List<GameObject>();
    }

    private void Start()
    {
       // CallBackBall.gameObject.SetActive(false);
       // DisplayCount.gameObject.SetActive(false);
        CountText = DisplayCount.GetComponentInChildren<Text>();
        GameObject CreateBall = Instantiate(NewBall, new Vector3(0, -5.8f, 0), Quaternion.identity) as GameObject;
        CreateBall.GetComponent<Rigidbody2D>().isKinematic = true;
        ListBall.Add(CreateBall);
        gamestate = GameState.InstanBox;
        LetInstantiateBox = true;
        easy = UI2.Easy;
        NextShootAudio = GetComponent<AudioSource>();
        level = 0;
        GunLine = GetComponent<LineRenderer>();
        timer = 0;
        SpeedBoost.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        TapToReplay.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
        sound = UI2.sound;
        CallAdmob = CompleteProject.Purchaser.CallAdmob;
        Gunline2 = line2.gameObject.GetComponent<LineRenderer>();
        okforcontinue = true;
        if (easy)
        {
            HighScore = PlayerPrefs.GetInt("EasyMode");
            HighScoreText.text = "HScore/Easy/" + HighScore;
        }
        else
        {
            HighScore = PlayerPrefs.GetInt("HardMode");
            HighScoreText.text = "HScore/Hard/" + HighScore;
        }
    }

    private void Update()
    {
        switch (gamestate)
        {
            case GameState.InstanBox:
                {
                    if (LetInstantiateBox)
                    {
                        SpeedBoost.gameObject.SetActive(false);
                        CallBackBall.gameObject.SetActive(false);
                        Time.timeScale = 1;
                        level = level + 1;
                        LetInstantiateBox = false;
                        StartCoroutine(AddBox());
                    }
                }
                break;
                
            case GameState.WaitforShoot:
                {
                    if (Input.touchCount >= 2 || Time.timeScale==0) return;
                    if (Input.GetMouseButtonDown(0))
                    {
                        FirstPos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        FirstPos3.z = 0f;
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        SecondPos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        SecondPos3.z = 0f;
                        DisPlayRange(FirstPos3, SecondPos3);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        if (letshoot)
                        {
                            for (int i = 0; i < ListBall.Count; i++)
                            {
                                ListBall[i].transform.position = new Vector3(ListBall[i].transform.position.x, -5.8f, 0f);
                            }
                            StartCoroutine(Shoot());
                            BotScript.Count = ListBall.Count;
                            BotScript.Number = 0;
                            gamestate = GameState.Idle;
                            letshoot = false;
                            GunLine.enabled = false;
                            Gunline2.enabled = false;
                        }
                        return;
                    }
                    
                }
                break;

            case GameState.Idle:
                {
                    timer += Time.deltaTime;
                    if (timer > 7f)
                    {
                        SpeedBoost.gameObject.SetActive(true);
                        CallBackBall.gameObject.SetActive(true);
                    }
                }
                break;
            case GameState.GameOver:
                {
                    SpeedBoost.gameObject.SetActive(false);
                    MainMenu.gameObject.SetActive(true);
                    TapToReplay.gameObject.SetActive(true);
                    if(okforcontinue)
                    ContinueButton.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                    if (CallAdmob)
                    {
                        Admob.GetComponent<AdmobController>().ShowInterstitial();
                        CallAdmob = false;
                    }
                }
                break;
        }

    }

    void DeActiveCollider()
    {
        for(int i =0;i<ListBall.Count;i++)
        {
            ListBall[i].GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void ActiveCollider()
    {
        for(int i=0;i<ListBall.Count;i++)
        {
            ListBall[i].GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    IEnumerator AddBox()
    {
        {
            for (int i = 0; i < BoxTransform.Length - 1; i++)
            {
                int r = Mathf.RoundToInt((Random.Range(i, BoxTransform.Length-1)));
                Transform transform = BoxTransform[i];
                BoxTransform[i] = BoxTransform[r];
                BoxTransform[r] = transform;
            }

            int rbd=0;
            if (!easy)
            {
                if (level < 5)
                    rbd = Mathf.RoundToInt(Random.Range(1, 5));
                else if (level < 13)
                    rbd = Mathf.RoundToInt(Random.Range(2, 6));
                else if (level >= 40 && level % 3 == 0)
                    rbd = Mathf.RoundToInt(Random.Range(3, 8));
                else
                    rbd = Mathf.RoundToInt(Random.Range(2, 7));
            }
            else
            {
                if (level < 5)
                    rbd = Mathf.RoundToInt(Random.Range(1, 5));
                else if (level < 13)
                    rbd = Mathf.RoundToInt(Random.Range(2, 6));
                else if (level >= 40 && level % 4 == 0)
                    rbd = Mathf.RoundToInt(Random.Range(3, 8));
                else
                    rbd = Mathf.RoundToInt(Random.Range(1, 7));
            }           

                Debug.Log(rbd);
            for (int m = 0; m < rbd; m++)
            {
                ListBox.Add(Instantiate(Box, BoxTransform[m].position, Quaternion.identity) as GameObject);
            }
            if (level % 10 != 0 && level!=1 && rbd<=7)
            {
                ListBox.Add(Instantiate(EatBall, BoxTransform[rbd].position, Quaternion.identity) as GameObject);
            }
            else if (rbd <= 7 && level % 10 == 0)
            {
                ListBox.Add(Instantiate(Melon, BoxTransform[rbd].position, Quaternion.identity) as GameObject);
            }
            if (level % 4 == 0 && rbd <= 6)
            {
                ListBox.Add(Instantiate(Orange, BoxTransform[rbd + 1].position, Quaternion.identity) as GameObject);
            }
            LevelText.text = "Level || " + level;
            if (level > HighScore)
            {
                if (easy)
                {
                    HighScore = level;
                    PlayerPrefs.SetInt("EasyMode", HighScore);
                    PlayerPrefs.Save();
                    HighScoreText.text = "HScore/Easy/" + HighScore;
                }
                else
                {
                    HighScore = level;
                    PlayerPrefs.SetInt("HardMode", HighScore);
                    PlayerPrefs.Save();
                    HighScoreText.text = "HScore/Hard/" + HighScore;
                }
            }
            MoveDownBox();
            DisplayCount.transform.position = ListBall[ListBall.Count - 1].transform.position - new Vector3(0, 0.4f, 0);
            CountText.text = "x" + ListBall.Count;
            DisplayCount.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            if (sound)
            NextShootAudio.Play();
            yield return new WaitForSeconds(0.2f);
            gamestate = GameState.WaitforShoot;
            timer = 0;
            if (level % 30 == 0)
                Admob.GetComponent<AdmobController>().ShowInterstitial();
        }
    }

    void MoveDownBox()
    {
        for (int i = 0; i < ListBox.Count; i++)
        {
            ListBox[i].transform.DOMove(ListBox[i].transform.position - new Vector3(0, 1, 0), 0.5f);
        }
    }

    void DisPlayRange(Vector3 vtor1,Vector3 vtor2)
    {
        float distance = Vector3.Distance(vtor1, vtor2);
        if (distance >= 0.4f)
        {
            RayShoot.origin = ListBall[0].transform.position;
            RayShoot.direction = (vtor2 - vtor1).normalized * -1;

            DeActiveCollider();
            RaycastHit2D shoothit = Physics2D.Raycast(RayShoot.origin, RayShoot.direction,15f);
            ActiveCollider();

            if (shoothit.collider.tag == "Return")
            {
                letshoot = false;
                GunLine.enabled = false;
                Gunline2.enabled = false;
                return;
            }
            else
            {
                Vector2 abc = (shoothit.point -(Vector2)ListBall[0].transform.position).normalized;
                GunLine.enabled = true;
                GunLine.SetPosition(0, ListBall[0].transform.position);
                GunLine.SetPosition(1,shoothit.point-abc/2.5f);
                GunLine.startWidth = 0.25f;
                GunLine.endWidth = 0.12f;
                Gunline2.SetPosition(0, shoothit.point - abc/3);
                Gunline2.enabled = true;
                Gunline2.SetPosition(1, shoothit.point);
                Gunline2.startWidth = 0.36f;
                Gunline2.endWidth = 0.01f;
                letshoot = true;
            }
        }
    }
    IEnumerator Shoot()
    { 
        for(int i=0;i<ListBall.Count;i++)
        {
            Rigidbody2D rb = ListBall[i].GetComponent<Rigidbody2D>();
            rb.velocity = RayShoot.direction * speedshoot;
            rb.isKinematic = false;
            CountText.text = "x" + (ListBall.Count -(i+1));
            if (i == ListBall.Count - 1) DisplayCount.SetActive(false);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public void ClickContinute()
    {
        if (AdmobController.loadedVideo)
        {
            MoveUpBox();
            gamestate = GameState.WaitforShoot;
            MainMenu.gameObject.SetActive(false);
            TapToReplay.gameObject.SetActive(false);
            ContinueButton.gameObject.SetActive(false);
            Time.timeScale = 1f;
            okforcontinue = false;
            Admob.GetComponent<AdmobController>().ShowRewardBasedVideo();
        }
        else
            ContinueButton.GetComponentInChildren<Text>().text = "Not Available";
    }

    void MoveUpBox()
    {
        for(int i=0;i<ListBox.Count;i++)
        {
            if(ListBox[i].transform.position.y ==2.5f || ListBox[i].transform.position.y ==3.5f)
            {
                clonelist.Add(ListBox[i]);
            }
        }
        
        for(int i=0;i<clonelist.Count;i++)
        {
            ListBox.Remove(clonelist[i]);
            Destroy(clonelist[i]);
        }
        clonelist.Clear();
        for(int i=0;i<ListBox.Count;i++)
        {
            ListBox[i].transform.position += 2 * Vector3.up;
        }
    }

    public void PressCallBackBalls()
    {
       // if (BotScript.Number > 0)
        {
            CallBackBall.gameObject.SetActive(false);
            Time.timeScale = 0f;
            for (int i = 0; i < ListBall.Count; i++)
            {
                if (ListBall[i].transform.position != BotScript.Position && ListBall[i].transform.position.y > -5f)
                {
                    abc.Add(ListBall[i]);
                }
            }
            for (int i = 0; i < abc.Count; i++)
            {
                Vector3 dir = abc[i].transform.position - BotScript.Position;
                abc[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                abc[i].GetComponent<Rigidbody2D>().isKinematic = true;
                abc[i].transform.DOMove(BotScript.Position + dir.normalized * 0.6f, 0.5f);
            }
            Time.timeScale = 1f;
            StartCoroutine(OkNow());
        }
      //  else return;
    }
    IEnumerator OkNow()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < abc.Count; i++)
        {
            Vector2 dir = abc[i].transform.position - BotScript.Position;
            abc[i].GetComponent<Rigidbody2D>().isKinematic = false;
            abc[i].GetComponent<Rigidbody2D>().velocity = -dir.normalized * speedshoot/2;
        }
        abc.Clear();
    }
}





    
