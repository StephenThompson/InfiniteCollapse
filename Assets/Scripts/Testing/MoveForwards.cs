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

        if (Input.GetKey(KeyCode.LeftArrow)) t.x -= Time.deltaTime * movespeed;
        else if (Input.GetKey(KeyCode.RightArrow)) t.x += Time.deltaTime * movespeed;
        if (Input.GetKey(KeyCode.UpArrow)) t.y += Time.deltaTime * movespeed;
        else if (Input.GetKey(KeyCode.DownArrow)) t.y -= Time.deltaTime * movespeed;
        transform.position = t;
    }
}
