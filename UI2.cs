using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI2 : MonoBehaviour
{
    public Image GuideImage;
    public Button SoundOn;
    public Button SoundOff;
    public Button EasyMode;
    public Button HardMode;
    public Button QuitButton;
    [HideInInspector] public static bool sound;
    [HideInInspector] public static bool Easy;

    private void Start()
    {
#if UNITY_IPHONE
        QuitButton.gameObject.SetActive(false);
#endif
        sound = true;
        SoundOn.gameObject.SetActive(true);
        SoundOff.gameObject.SetActive(false);
        Easy = true;
        EasyMode.gameObject.SetActive(true);
        HardMode.gameObject.SetActive(false);
        GuideImage.gameObject.SetActive(false);
    }
    public void Play()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public void Guide()
    {
        GuideImage.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HideImage()
    {
        GuideImage.gameObject.SetActive(false);
    }

    public void PressSoundOn()
    {
        SoundOn.gameObject.SetActive(false);
        SoundOff.gameObject.SetActive(true);
        sound = false;
    }
    public void PressSoundOff()
    {
        SoundOn.gameObject.SetActive(true);
        SoundOff.gameObject.SetActive(false);
        sound = true;
    }

    public void ClickEasyMode()
    {
        Easy = false;
        EasyMode.gameObject.SetActive(false);
        HardMode.gameObject.SetActive(true);
    }
    public void ClickHardMode()
    {
        Easy = true;
        EasyMode.gameObject.SetActive(true);
        HardMode.gameObject.SetActive(false);
    }
    public void Rate()
    {
#if UNITY_EDITOR || UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.tester.Illusions");
#elif UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/app/id1306547908");
#endif
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GamePlay");

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

      