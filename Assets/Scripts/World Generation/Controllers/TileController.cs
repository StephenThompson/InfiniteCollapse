using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    private Queue<FallingBlockController> staticTiles;
    private List<FallingBlockController> droppedTiles;

    public void setupTiles(WorldDefinition worldSetting, Collider[] c)
    {
        Transform[] subBlocks = gameObject.GetComponentsInChildren<Transform>();
        staticTiles = new Queue<FallingBlockController>();
        droppedTiles = new List<FallingBlockController>();

        for (int i = 0; i < subBlocks.Length; ++i)
        {
            if (subBlocks[i] == transform) continue;
            FallingBlockController fbc = subBlocks[i].gameObject.AddComponent<FallingBlockController>();
            staticTiles.Enqueue(fbc);
            fbc.worldSettings = worldSetting;
            fbc.BelowTileColliders = c;
        }
    }

    public bool dropTile()
    {
        if (staticTiles.Count == 0) return false;
        FallingBlockController fbc = staticTiles.Dequeue();
        fbc.HasDropped = true;
        droppedTiles.Add(fbc);
        return true;
    }
    
    public void resetPositions()
    {
        for (int i = droppedTiles.Count - 1; i >= 0; --i)
        {
            droppedTiles[i].resetBlock();
            staticTiles.Enqueue(droppedTiles[i]);
            droppedTiles.RemoveAt(i);
        }
    }
}
