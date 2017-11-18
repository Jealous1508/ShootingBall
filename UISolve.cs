using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UISolve : MonoBehaviour
{
    public Image Option;
    private void Start()
    {
        Option.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Option.gameObject.SetActive(true);
    }
    public void SpeedBost()
    {
        if (Time.timeScale == 1f)
            Time.timeScale = 2.5f;
        else if (Time.timeScale == 2.5f)
            Time.timeScale = 4f;
        else return;
    }

    public void Replay()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Option.gameObject.SetActive(false);
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        //This is particularly good for creating loading screens. You could also load the scene by build //number.
        AsyncOperation asyncLoad2 = SceneManager.LoadSceneAsync("GamePlay");

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad2.isDone)
        {
            yield return null;
        }
    }
}
