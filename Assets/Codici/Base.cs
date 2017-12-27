using UnityEngine;
using System.Collections;

public class Base : MonoBehaviour {

    public GameObject endexplosion;
    public GameObject exp;
    public GameManager GameManager;
    new AudioSource audio;
    public AudioClip coinclip;
    private float cost;
    private float _cost;
    private float x;
    private int Coins;

    void Start()
	{
        GameManager = FindObjectOfType<GameManager>();
        StartCoroutine(Scaling());
        if (GameObject.Find("GameManager"))
        {
            x = 0.75f;
            cost = 15;
            gameObject.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + PlayerPrefs.GetString("Skin")) as Texture;
            Coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            x = 3f;
            cost = 200;
        }

        audio = GetComponent<AudioSource>();
	}
    void Update()
    {
        if(Time.timeScale > 0)
        {
            Time.timeScale += 0.05f * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            gameObject.transform.localScale = new Vector3(cost, cost, cost);
            cost += _cost;
            gameObject.transform.Rotate(Vector3.down * Time.unscaledDeltaTime * 10);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Blu" || col.gameObject.tag == "Verde" || col.gameObject.tag == "Giallo" || col.gameObject.tag == "Rosso")
            StartCoroutine(Collisione());

        if (col.gameObject.tag == "Slow")
            Slowdown();

        if(col.gameObject.tag == "Coin")
        {
            Coins += 1;
            col.GetComponent<Coin>().Destroy();
            audio.clip = coinclip;
            audio.Play();
        }
    }
    
    private IEnumerator Scaling()
    {
        do
        {
            _cost = 0.25f;
            yield return new WaitForSeconds(x);
            _cost = -0.25f;
            yield return new WaitForSeconds(x);
        } while (gameObject);
        
    }

    private IEnumerator Collisione()
    {
        Destroy(GameObject.FindGameObjectWithTag("Cuore"));
        yield return new WaitForSeconds(0.01f);
        GameManager.GetNewnemy();
        if (!GameObject.FindGameObjectWithTag("Cuore"))
        {
            PlayerPrefs.SetInt("Coins", Coins);
            Instantiate(endexplosion, transform.position, transform.rotation);
            GameManager.Endgame();
            Destroy(gameObject);
        }
    }

    private void Slowdown()
    {
        Instantiate(exp, transform.position, transform.rotation);
        Destroy(GameObject.FindGameObjectWithTag("Slow"));
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}