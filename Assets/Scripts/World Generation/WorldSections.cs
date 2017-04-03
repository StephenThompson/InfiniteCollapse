using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSections : MonoBehaviour {
    private GameObject[] tiles;
    private const float LAYER_HEIGHT = 15;
    private float sectionwidth;
    private int subdiv;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void positionSection(float x, int y, float z)
    {
        transform.position = new Vector3(x, y * LAYER_HEIGHT, z);
    }

    public bool isPastCamera()
    {
        return transform.position.z + sectionwidth < Camera.main.transform.position.z;
    }

    public void resetTilePositions()
    {
        int subdivisions = subdiv;
        int size = subdivisions * subdivisions;
        float width = sectionwidth;
        float sub = 1.0f / subdivisions;

        for (int i = 0; i < size; ++i)
        {

            GameObject tile = tiles[i];

            float ran = Random.value;
            float x = (float)(i % subdivisions) * sub * width;
            float y = 0;
            float z = (float)(i / subdivisions) * sub * width;



            tile.transform.position = gameObject.transform.position + 
                new Vector3(x, (y - 1.5f) * 3f + Mathf.PerlinNoise(x * 0.1f + y * 0.5f, z * 0.1f) * 4f + (ran > 0.95 ? ran * 0.7f : 0), z);
            tile.transform.localScale = new Vector3(1f, 0.5f + Random.value * 0.1f, 1f) * sub * width;
        }
    }

    public void activate()
    {
        gameObject.SetActive(true);
    }

    public void deactivate()
    {
        gameObject.SetActive(false);
    }

    public void dropRandomTile()
    {
        GameObject t = tiles[Random.Range(0, tiles.Length)];
        if (t.transform.position.z < Camera.main.transform.position.z + 10) return;
        Rigidbody r = t.GetComponent<Rigidbody>();
        r.isKinematic = false;
    }

    public static WorldSections generateSection(Material mat, float width, int subdivisions)
    {
        GameObject section = new GameObject();
        WorldSections ws = section.AddComponent<WorldSections>();
        int size = subdivisions * subdivisions;
        ws.tiles = new GameObject[size];
        ws.sectionwidth = width;
        ws.subdiv = subdivisions;
        float sub = 1.0f / subdivisions;
        for (int i = 0; i < size; ++i)
        {

            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.transform.parent = ws.transform;
            float ran = Random.value;
            float x = (float)(i % subdivisions) * sub * width;
            float y = 0;
            float z = (float)(i / subdivisions) * sub * width;



            tile.transform.position = new Vector3(x, (y - 1.5f) * 3f + Mathf.PerlinNoise(x * 0.1f + y * 0.5f, z * 0.1f) * 4f + (ran > 0.95 ? ran * 0.7f : 0), z);
            tile.transform.localScale = new Vector3(1f, 0.5f + Random.value * 0.1f, 1f) * sub * width;
            tile.GetComponent<Renderer>().material = mat;
            Rigidbody r = tile.AddComponent<Rigidbody>();
            r.isKinematic = true;
            //r.detectCollisions = false;
            ws.tiles[i] = tile;
        }

        return ws;
    }
}
