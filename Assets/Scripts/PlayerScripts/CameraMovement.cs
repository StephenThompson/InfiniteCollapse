using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public GameObject Cube;
    public float forwardM = 5;
    public float xRot = 110;
    public float yRot = 80;
    public float zRot = 70;
    PlayerController movement;
	Transform tr;

    private Vector3 offset;
	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();
        offset = new Vector3(transform.position.x - Cube.transform.position.x, transform.position.y - Cube.transform.position.y, -10);
        movement = Cube.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        transform.TransformPoint(new Vector3(offset.x, 0, 0));
        transform.position = Cube.transform.position + offset;
        transform.Translate(new Vector3(0, 0, -20));// -movement.velocity.magnitude/ forwardM));
		Camera c = GetComponent<Camera> ();
        c.fieldOfView = Mathf.Clamp(movement.velocity.z * 20, 60, 120);
        float x = movement.velocity.x;
        float y = movement.velocity.y;
		//transform.localRotation = new Quaternion(-y / xRot, x / yRot, x / zRot, Cube.transform.rotation.w);
		//transform.Rotate (-y / xRot, x / yRot, x / zRot);
		tr.rotation = new Quaternion(-y / xRot,  x / yRot, x / zRot, tr.rotation.w);
		print (-x / xRot);
	}
}
