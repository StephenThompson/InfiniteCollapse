using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Generation/Layer Definition")]
public class LayerDefinition : ScriptableObject
{
    public List<GameObject> BlockPrefabs;

    public float scale()
    {
        Renderer[] s = BlockPrefabs[0].GetComponentsInChildren<Renderer>();//.bounds.size;
        float xMin = float.MaxValue;
        float xMax = float.MinValue;
        foreach (Renderer r in s) {
            xMin = Mathf.Min(xMin, r.bounds.size.x);
            xMax = Mathf.Max(xMax, r.bounds.size.x);
        }
        return 1f / (xMax - xMin);
    }
}
