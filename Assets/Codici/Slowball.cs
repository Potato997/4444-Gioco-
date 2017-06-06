using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowball : MonoBehaviour {

    public float velocity = 15f;
    private float step;
    private GameObject target;

    void Start () {
        target = GameObject.FindGameObjectWithTag("Base");
        step = velocity * Time.deltaTime;
    }
	
	void Update () {
        if (GameObject.FindGameObjectWithTag("Base") && Time.timeScale > 0)
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
}