using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {
    
    public Text highscore;
    public GameObject LoadingPanel;
    public Text LoadText;
    public Transform LoadingBar;

    void Start()
    {
        highscore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene(1));
    }

    private void Update()
    {
        LoadingBar.GetComponent<Image>().gameObject.transform.Rotate(0, 0, -10);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Shop()
    {
        StartCoroutine(LoadScene(2));
    }

    public void MyBalls()
    {
        StartCoroutine(LoadScene(3));
    }

    IEnumerator LoadScene(int scene)
    {
        LoadingPanel.SetActive(true);
        AsyncOperation asy = SceneManager.LoadSceneAsync(scene);
        while (!asy.isDone)
        {
            LoadText.text = asy.progress *110 + "%";
            LoadingBar.GetComponent<Image>().fillAmount = asy.progress * (float) 1.1;
            yield return 0;
        }
    }
}