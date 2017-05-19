using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Generation/World Settings")]
public class WorldDefinition : ScriptableObject
{
    [Header("Tile Settings")]
    public int LayerHeight = 5;
    public float TerrainHeightVariation = 1;
    public Vector3 TileSize;

    [Header("Fall Rate Settings")]
    public int BlocksPerDrop = 5;
    public int DropRate = 5;
    public float InitialTimeBeforeSpawn = 1;
    public int FallSpeed = 1;

    [Header("Layer Tilesets")]
    public List<LayerDefinition> Layers;
}
