using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "World Generation/Constants")]
public class Constants : ScriptableObject
{
    public int FALLING_SPEED = 1;
    public int LAYER_HEIGHT = 5;
    public int FALLING_BLOCKS_PER_SECOND = 5;
    public int DROP_NUMBER = 5;

    public Vector3 TILE_SIZE;
}
