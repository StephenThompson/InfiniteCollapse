using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    public WorldDefinition worldSettings;

    // Player info
    public Transform player;
    private int currentLayer;
    public int CurrentLayer { get { return currentLayer; } }

    // Memory Management
    private TileController[][] tiles;

    // World generation parameters. It works for now but could do with some cleaning up
    private float TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH;
    private const int zoffset = 1;
    private const int yoffset = 0;
    private const int sizeX = 5, sizeY = 4, sizeZ = 5, MinX = -2, MaxX = 2, MinZ = 4, MaxZ = -1;
    private float fallrate, timer;

    void Start()
    {
        // Set time settings
        timer = -worldSettings.InitialTimeBeforeSpawn;
        fallrate = 1f / worldSettings.BlocksPerDrop;

        // Set world scale
        float scaleModifier = worldSettings.Layers[0].scale();
        TILE_WIDTH = worldSettings.TileSize.x;
        TILE_HEIGHT = worldSettings.TileSize.y;
        TILE_LENGTH = worldSettings.TileSize.z;

        // Generate tiles and place in array
        tiles = new TileController[sizeY][];
        for (int i = 0; i < sizeY; ++i)
        {
            tiles[i] = new TileController[sizeX * sizeZ];
            for (int j = 0; j < tiles[i].Length; ++j)
            {
                GameObject tile = Instantiate(worldSettings.Layers[i % worldSettings.Layers.Count].BlockPrefabs[Random.Range(0, worldSettings.Layers[0].BlockPrefabs.Count)]);
                tiles[i][j] = tile.AddComponent<TileController>();
                tiles[i][j].transform.parent = transform;
                tiles[i][j].transform.localScale = new Vector3(TILE_WIDTH, TILE_HEIGHT, TILE_LENGTH) * scaleModifier;
            }
        }

        // Set colliders for 
        for (int i = 0; i < sizeY; ++i)
            for (int j = 0; j < tiles[i].Length; ++j)
                tiles[i][j].setupTiles(worldSettings, tiles[i == 0 ? sizeY - 1 : i - 1][j].GetComponentsInChildren<Collider>());
    }
	
	void Update () {
        // Set current layer
        currentLayer = Mathf.FloorToInt(player.position.y / worldSettings.LayerHeight);

        // Get current tile positon of player
        int x1 = Mathf.FloorToInt((player.position.x + TILE_WIDTH * 0.5f) / TILE_WIDTH);
        int y1 = currentLayer;
        int z1 = Mathf.FloorToInt((player.position.z + TILE_LENGTH * 0.5f) / TILE_LENGTH);

        // Move tiles with player
        for (int i = 0; i < sizeY; ++i)
        {
            for (int j = 0; j < tiles[i].Length; ++j)
            {
                int xo = j % sizeX;
                int yo = i;
                int zo = j / sizeX;
               
                float px = ((x1 / sizeX) * sizeX + Mathf.FloorToInt(1f * (x1 % sizeX + (sizeX / 2 - xo)) / sizeX) * sizeX + xo) * TILE_WIDTH + TILE_WIDTH * 0.5f * zo % 2;
                float py = ((y1 / sizeY) * sizeY + Mathf.FloorToInt(1f * (y1 % sizeY + (sizeY / 2 - yo - yoffset)) / sizeY) * sizeY + yo) * worldSettings.LayerHeight;
                float pz = ((z1 / sizeZ) * sizeZ + Mathf.FloorToInt(1f * (z1 % sizeZ + (sizeZ / 2 - zo + zoffset)) / sizeZ) * sizeZ + zo) * TILE_LENGTH;

                // Reset tiles when looping to front of the player
                if (tiles[i][j].transform.position.z - pz < -TILE_LENGTH)
                    tiles[i][j].resetPositions();

                tiles[i][j].transform.position = new Vector3(px, py, pz);
            }
        }

        // Drops tiles
        timer += Time.deltaTime;
        if (timer > fallrate)
        {
            timer = 0;
            for (int i = 0; i < worldSettings.DropRate; ++i)
            {
                int index1 = Random.Range(0, tiles.Length);
                int index2 = Random.Range(0, tiles[index1].Length);
                tiles[index1][index2].dropTile();
            }
        }
    }
}
