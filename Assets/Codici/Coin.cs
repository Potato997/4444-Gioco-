using UnityEngine;

public class Coin : MonoBehaviour {

    private GameObject target;

    void Start () {
        if (GameObject.FindGameObjectWithTag("Base"))
            target = GameObject.FindGameObjectWithTag("Base");
        transform.Rotate(90, 0, 0);
    }

    void Update()
    {
        if (target && Time.timeScale > 0f)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 20f * Time.deltaTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}