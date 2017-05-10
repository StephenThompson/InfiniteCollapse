using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwards : MonoBehaviour {
    Vector3 t;
    public float speed = 1;
    public float movespeed = 1;
    // Use this for initialization
    void Start () {
        t = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
    
        t.z += Time.deltaTime * speed;
        t.x += Mathf.Round(Input.GetAxis("Horizontal") * 10000) * 0.0001f * Time.deltaTime * movespeed;
        t.y += Mathf.Round(Input.GetAxis("Vertical") * 10000) * 0.0001f * Time.deltaTime * movespeed;
        transform.position = t;
    }
}
