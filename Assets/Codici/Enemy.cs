using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float speed = 20f;
    private float step;
    private GameObject target;

    private void Start()
    {
        Time.timeScale = 1f;
        if (GameObject.FindGameObjectWithTag("Base"))
            target = GameObject.FindGameObjectWithTag("Base");
        step = speed * Time.unscaledDeltaTime;
    }

    private void Update()
    {
        if (target && Time.timeScale > 0f)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Base")
            Destroy(gameObject);
    }
}