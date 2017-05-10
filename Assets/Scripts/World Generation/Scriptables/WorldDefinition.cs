using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Generation/World Definition")]
public class WorldDefinition : ScriptableObject
{
    public Constants constantValues;
    public List<LayerDefinition> layers;
}
