using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public Rigidbody[] nemici = new Rigidbody[4];
    public Rigidbody prefabblue, prefabgreen, prefabyellow, prefabred;
    public Rigidbody slowball;
    public GameObject explosion;
    public GameObject CoinPref;

    private Vector3 pos = new Vector3();
    private Vector3 centro = new Vector3();
    private Enemy[] enemylist;
    private Enemy enemy = null;
    private Enemy[] enemynumber;
    private Light enemylight, oldenemy;

    public GameObject lose;
    public GameObject Menupausa;
    public GameObject indicatorred;
    
    public Text record;
    public Text losetext;
    public Text CoinText;

    private float vartimer;
    private float Timer = 1f;
    private float CoinTimer;
    private float tempspeed;
    private int i, j, posizione, ball;
    public int score;
    private int Enemies;
    private int spec;
    private int specnum;
    private bool wrong;
    private bool special = false;
    private float deltaspeed;

    public AudioClip[] sounds;
    AudioSource sound;
    #endregion

    void Start()
    {
        CoinTimer = 2f;

        sound = GetComponent<AudioSource>();

        Time.timeScale = 1f;
        score = 0;
        vartimer = 0;
        wrong = false;

        nemici[0] = prefabblue;
        nemici[1] = prefabgreen;
        nemici[2] = prefabyellow;
        nemici[3] = prefabred;

        centro = GameObject.FindGameObjectWithTag("Base").transform.position;
        
        AudioListener.pause = false;

        indicatorred.SetActive(false);
        Menupausa.SetActive(false);
        lose.SetActive(false);

        Instantiate(nemici[Random.Range(0, 4)], transform.position, transform.rotation);
        Enemy.speed = 20f;
        enemy = GameObject.FindObjectOfType<Enemy>();
        enemylight = enemy.GetComponent<Light>();
        enemylight.enabled = true;
        spec = score + Random.Range(5, 15);

        GetNewnemy();
    }

    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
            Generate();

        CoinTimer -= Time.deltaTime;

        if (CoinTimer <= 0)
            CoinGenerator();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && wrong == false && GameObject.FindGameObjectWithTag("Base") && Time.timeScale > 0f)
            Distruggi();

        /*if (Input.GetMouseButtonUp(0))
        {
            Distruggi2();
        }*/
    }

    public void Distruggi2()
    {
        Ray raggio = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit colpo;

        if (Physics.Raycast(raggio, out colpo, Mathf.Infinity))
        {
            if (GameObject.FindGameObjectWithTag(colpo.collider.name) && GameObject.FindGameObjectWithTag(colpo.collider.name).name == enemy.name)
            {
                Instantiate(explosion, enemy.transform.position, transform.rotation);
                enemy.GetComponent<Enemy>().Destroy();
                Enemy.speed += 0.05f;
                score += 1;
                Destroyed();
            }
            else
                StartCoroutine(WrongButton());
        }
    }

    public void Generate()
    {
        if (GameObject.FindGameObjectWithTag("Base"))
        {
            posizione = Random.Range(0, 4);
            switch (posizione)
            {
                case 0:
                   pos = transform.position = new Vector3(Random.Range(-85, 85), -150, -95);
                   break;
                case 1:
                   pos = transform.position = new Vector3(Random.Range(-85, 85), -150, 125);
                   break;
                case 2:
                   pos = transform.position = new Vector3(-85, -150, Random.Range(-95, 125));
                   break;
                case 3:
                   pos = transform.position = new Vector3(85, -150, Random.Range(-95, 125));
                   break;
            }

            ball = Random.Range(0, 4);
            Instantiate(nemici[ball], pos, transform.rotation);
            Timer = 1f - vartimer;
            Howmanyenemy();
        }
    }

    public void Distruggi()
    {
       Ray raggio = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
       RaycastHit colpo;
        
        if (Physics.Raycast(raggio, out colpo, Mathf.Infinity))
        {
            if (GameObject.FindGameObjectWithTag(colpo.collider.name) && GameObject.FindGameObjectWithTag(colpo.collider.name).name == enemy.name)
            {
                Instantiate(explosion, enemy.transform.position, transform.rotation);
                enemy.GetComponent<Enemy>().Destroy();
                Enemy.speed += deltaspeed;
                score += 1;
                Destroyed();
            }
             else
                StartCoroutine(WrongButton());
        }
    }

    public void Destroyed()
    {
        record.text = score.ToString();
        GetNewnemy();
        Aumentadifficolta();

        if (score == spec)
            StartCoroutine(Special());
    }

    private IEnumerator WrongButton()
    {
        wrong = true;
        indicatorred.SetActive(true);
        Handheld.Vibrate();
        yield return new WaitForSeconds(0.2f);
        wrong = false;
        indicatorred.SetActive(false);
    }

    public IEnumerator FindClosestEnemy()
    {
        yield return new WaitUntil(() => Enemies > 0);
        {
            enemylist = FindObjectsOfType(typeof(Enemy)) as Enemy[];
            float shortestDistance = Mathf.Infinity;
            foreach (Enemy nemico in enemylist)
            {
                oldenemy = nemico.GetComponent<Light>();
                oldenemy.enabled = false;
                Vector3 diff = nemico.transform.position - centro;
                float distanza = diff.sqrMagnitude;
                if (distanza < shortestDistance)
                {
                    enemy = nemico;
                    shortestDistance = distanza;
                }
            }
            enemylight = enemy.GetComponent<Light>();
            enemylight.enabled = true;
            enemy.transform.localScale = new Vector3(15, 15, 15);
        }
    }

    public void Howmanyenemy()
    {
        enemynumber = FindObjectsOfType(typeof(Enemy)) as Enemy[];
        Enemies = enemynumber.Length;
        if (Enemies <= 1)
            Generate();
    }

    void Aumentadifficolta()
    {
        switch (score)
        {
            case 0:
                deltaspeed = 0.05f;
                vartimer = 0.3f;
                break;
            case 20:
                vartimer = 0.35f;
                break;
            case 60:
                deltaspeed = 0.025f;
                vartimer = 0.45f;
                special = true;
                break;
            case 100:
                deltaspeed = 0.01f;
                break;
            case 120:
                deltaspeed = 0.002f;
                vartimer = 0.5f;
                break;
            case 160:
                vartimer = 0.55f;
                break;
            case 200:
                vartimer = 0.6f;
                break;
        }
    }

    public void Pause()
    {
        if (GameObject.Find("Base"))
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            Menupausa.SetActive(true);
        }
    }

    public void Resume()
    {
        sound.clip = sounds[0];
        sound.Play();
        Time.timeScale = 1f; 
        AudioListener.pause = false;
        Menupausa.SetActive(false);
    }

    public void Mainmenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        sound.clip = sounds[0];
        sound.Play();
        SceneManager.LoadScene(1);
    }

    public void Endgame()
    {
        sound.clip = sounds[1];
        sound.Play();

        lose.SetActive(true);
        CoinText.text = "Coins: "+ PlayerPrefs.GetInt("Coins");
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            losetext.text = score + "!";
        }
        else
            losetext.text = "" + score;
    }

    private IEnumerator Special()
    {
        if (special == true)
        {
            special = false;
            specnum = Random.Range(0, 3);
            switch (specnum)
            {
                case 0:
                Timer = 30f;
                yield return new WaitUntil(() => Enemies <= 1);
                    for (i = 0; i < 3; i++)
                    {
                        pos = transform.position = new Vector3(2, -50, -95);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                        pos = transform.position = new Vector3(-2, -50, -95);
                        Instantiate(nemici[Random.Range(0,4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                    }
                    yield return new WaitUntil(() => Enemies <= 1);
                    for (i = 0; i < 3; i++)
                    {
                        pos = transform.position = new Vector3(2, -50, 125);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                        pos = transform.position = new Vector3(-2, -50, 125);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                    }
                    yield return new WaitUntil(() => Enemies <= 1);
                    for (i = 0; i < 3; i++)
                    {
                        pos = transform.position = new Vector3(85, -50, 14);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                        pos = transform.position = new Vector3(85, -50, 16);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                    }
                    yield return new WaitUntil(() => Enemies <= 1);
                    for (i = 0; i < 3; i++)
                    {
                        pos = transform.position = new Vector3(-85, -50, 14);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                        pos = transform.position = new Vector3(-85, -50, 16);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.35f);
                    }
                    yield return new WaitUntil(() => Enemies <= 1);
                    Timer = 0.5f;
                break;

                case 1:
                    Timer = 30f;
                    yield return new WaitUntil(() => Enemies <= 1);
                    for (i = 0; i < 4; i++)
                    {
                        pos = transform.position = new Vector3(-85, -50, 125);
                        Instantiate(nemici[Random.Range(0,4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.5f);
                        pos = transform.position = new Vector3(85, -50, 125);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.5f);
                        pos = transform.position = new Vector3(85, -50, -95);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.5f);
                        pos = transform.position = new Vector3(-85, -50, -95);
                        Instantiate(nemici[Random.Range(0, 4)], pos, transform.rotation);
                        Howmanyenemy();
                        yield return new WaitForSeconds(0.5f);
                    }
                    yield return new WaitUntil(() => Enemies <= 1);
                    Timer = 0.5f;
                    break;

                case 2:
                    pos = transform.position = new Vector3(0, -50, -95);
                    Instantiate(slowball, pos, transform.rotation);
                    break;
            }
            special = true;
        }
        spec = score + Random.Range(15, 35);
    }

    private void CoinGenerator()
    {
        CoinTimer = Random.Range(5f, 15f);
        Instantiate(CoinPref, pos, transform.rotation);
    }

    public void GetNewnemy()
    {
        Howmanyenemy();
        StartCoroutine(FindClosestEnemy());
    }
}