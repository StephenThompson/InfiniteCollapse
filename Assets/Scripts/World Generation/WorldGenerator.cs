using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    public Material tileMaterial;
    private List<WorldSections> activeTiles = new List<WorldSections>();
    private List<WorldSections> pooledTiles = new List<WorldSections>();

    public float dropTimer = 5.0f;
    public float timer = 0;

    public int pregeneratedNextSection = -1;
    public bool generatedNextSection = false;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 50; ++i)
        {
            WorldSections g = WorldSections.generateSection(tileMaterial, 60, 20);
            g.gameObject.transform.parent = transform;
            g.positionSection(0, i, 0);
            g.deactivate();
            pooledTiles.Add(g);
        }
    }
    // Step 1: layers
    // Step 2: infinite terrain
    // Step 3: Falling tiles

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int variathingy = (int)Camera.main.transform.position.z / 60;
        if (variathingy > pregeneratedNextSection)
        {
            pregeneratedNextSection = variathingy;
            for (int i = 0; i < 2; ++i) {
                WorldSections ws = pooledTiles[i];
                ws.positionSection(0, i, ((int)Camera.main.transform.position.z / 60 + 1) * 60);
                pooledTiles.Remove(ws);
                activeTiles.Add(ws);
                ws.resetTilePositions();
                ws.activate();
            }
        } /*else
        {
            if (generatedNextSection)
            {
                generatedNextSection = false;
            }
        }
         */

        for (int i = 0; i < activeTiles.Count; ++i)
        {
            if (timer > dropTimer)
            {
                activeTiles[i].dropRandomTile();
            }
            if (activeTiles[i].isPastCamera())
            {
                activeTiles[i].deactivate();
                pooledTiles.Add(activeTiles[i]);
                activeTiles.Remove(activeTiles[i]);
            }
        }

        if (timer > dropTimer)
        {
            timer = 0;
        }
    }
}
