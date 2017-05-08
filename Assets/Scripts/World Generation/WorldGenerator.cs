using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    public WorldDefinition wd;
    public Material lowPolyMaterial;

    // Player info
    public Transform player;
    private int currentLayer;
    public int CurrentLayer { get { return currentLayer; } }

    // Memory Management
    private TileController[][] tiles;

    // World generation parameters
    //private float viewingDistance = 100;
    //private float viewingDistanceWidth = 1;
    //private float dropDistance;

    //private Transform[][] farPlanes;
    private Transform[] topPlanes;


    private float TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH;
    private const int zoffset = 1;
    private const int yoffset = 0;
    private const int sizeX = 5, sizeY = 3, sizeZ = 5;
    private float fallrate, timer;

    void Start()
    {
        timer = 0;
        fallrate = 1f / wd.constantValues.FALLING_BLOCKS_PER_SECOND;

        float scaleModifier = 1 / 12f;// wd.layers[0].scale();
        TILE_WIDTH = wd.constantValues.TILE_SIZE.x;
        TILE_HEIGHT = wd.constantValues.TILE_SIZE.y;
        TILE_LENGTH = wd.constantValues.TILE_SIZE.z;

        //viewingDistance = Camera.main.farClipPlane;
        //viewingDistanceWidth = viewingDistance;

        //dropDistance = (sizeZ - zoffset - 0.5f) * TILE_LENGTH;

        //farPlanes = new Transform[sizeY][];
        tiles = new TileController[sizeY][];
        for (int i = 0; i < sizeY; ++i)
        {
            /*farPlanes[i] = new Transform[3];
            // Generate low poly tiles
            for (int j = 0; j < farPlanes[i].Length; ++j)
            {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
                farPlanes[i][j] = plane.transform;
                farPlanes[i][j].parent = transform;
                farPlanes[i][j].localScale = new Vector3(viewingDistanceWidth, TILE_HEIGHT * scaleModifier, (viewingDistance - dropDistance));
                farPlanes[i][j].GetComponent<Renderer>().material = lowPolyMaterial;
            }*/

            // Generate High poly tiles
            tiles[i] = new TileController[sizeX * sizeZ];
            for (int j = 0; j < tiles[i].Length; ++j)
            {
                GameObject tile = Instantiate(wd.layers[0].BlockPrefabs[Random.Range(0, wd.layers[0].BlockPrefabs.Count)]);
                tiles[i][j] = tile.AddComponent<TileController>();
                tiles[i][j].wd = wd;
                tiles[i][j].transform.parent = transform;
                tiles[i][j].transform.localScale = new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH) * scaleModifier;
            }
        }

        /*topPlanes = new Transform[2];
        for (int i = 0; i < sizeY; ++i)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topPlanes[i] = plane.transform;
            topPlanes[i].parent = transform;
            topPlanes[i].localScale = new Vector3(viewingDistance * 3, TILE_HEIGHT * scaleModifier, viewingDistance);
            topPlanes[i].GetComponent<Renderer>().material = lowPolyMaterial;
        }*/
    }
	
	void Update () {
        int x1 = Mathf.FloorToInt((player.position.x + TILE_WIDTH * 0.5f) / TILE_WIDTH);
        currentLayer = Mathf.FloorToInt(player.position.y / wd.constantValues.LAYER_HEIGHT);
        int y1 = currentLayer;
        int z1 = Mathf.FloorToInt((player.position.z + TILE_LENGTH * 0.5f) / TILE_LENGTH);

        for (int i = 0; i < sizeY; ++i)
        {
            // Move far away planes
            /*for (int j = 0; j < farPlanes[i].Length; ++j)
            {
                float x = (Mathf.FloorToInt(player.position.x / viewingDistanceWidth) + (j - 0.5f)) * viewingDistanceWidth;
                float y = Mathf.FloorToInt(player.position.y / wd.constantValues.LAYER_HEIGHT + i) * wd.constantValues.LAYER_HEIGHT;
                float z = Mathf.FloorToInt((player.position.z + TILE_LENGTH * 0.5f) / TILE_LENGTH) * TILE_LENGTH + dropDistance + (viewingDistance - dropDistance) * 0.5f;
                farPlanes[i][j].position = new Vector3(x, y, z);
            }*/

            // Move near stuff
            for (int j = 0; j < tiles[i].Length; ++j)
            {
                int xo = j % sizeX;
                int yo = i;
                int zo = j / sizeX;

                float px = ((x1 / sizeX) * sizeX + Mathf.FloorToInt(1f * (x1 % sizeX + (sizeX / 2 - xo)) / sizeX) * sizeX + xo) * TILE_WIDTH;
                float py = ((y1 / sizeY) * sizeY + Mathf.FloorToInt(1f * (y1 % sizeY + (sizeY / 2 - yo - yoffset)) / sizeY) * sizeY + yo) * wd.constantValues.LAYER_HEIGHT;
                float pz = ((z1 / sizeZ) * sizeZ + Mathf.FloorToInt(1f * (z1 % sizeZ + (sizeZ / 2 - zo + zoffset)) / sizeZ) * sizeZ + zo) * TILE_LENGTH;

                if (tiles[i][j].transform.position.z - pz < -TILE_LENGTH)
                    tiles[i][j].resetPositions();

                tiles[i][j].transform.position = new Vector3(px, py, pz);
            }
        }

        timer += Time.deltaTime;
        if (timer > fallrate)
        {
            timer = 0;
            for (int i = 0; i < wd.constantValues.DROP_NUMBER; ++i)
            {
                int index1 = Random.Range(0, tiles.Length);
                int index2 = Random.Range(0, tiles[index1].Length);
                tiles[index1][index2].dropTile();
            }
        }

       /* for (int i = 0; i < topPlanes.Length; ++i)
        {
            float x = Mathf.FloorToInt(player.position.x / TILE_WIDTH) * TILE_WIDTH;
            float y = Mathf.FloorToInt(player.position.y / wd.constantValues.LAYER_HEIGHT + (i == 0? -1 : 2)) * wd.constantValues.LAYER_HEIGHT;
            float z = Mathf.FloorToInt(player.position.z / TILE_LENGTH) * TILE_LENGTH + viewingDistance * 0.5f;
            topPlanes[i].position = new Vector3(x, y, z);
        }*/
    }
}
