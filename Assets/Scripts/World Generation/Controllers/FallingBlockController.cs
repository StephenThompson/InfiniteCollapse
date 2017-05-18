using UnityEngine;

public class FallingBlockController : MonoBehaviour {
    public WorldDefinition worldSettings;

    private Vector3 startPosition;
    private bool hasDropped;
    private bool hasCollision;
    private Vector3 size;

    private Collider[] selfColliders;
    private Collider[] belowTileColliders;
    public Collider[] BelowTileColliders
    {
        set { belowTileColliders = value; }
    }

    public bool HasDropped
    {
        get { return hasDropped; }
        set { hasDropped = value; }
    }

	void Start () {
        Renderer r = GetComponent<Renderer>();
        size = r.bounds.size;
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        startPosition = transform.localPosition;
        selfColliders = GetComponents<Collider>();
    }

	void FixedUpdate () {
        // Drop tile until collision occurs
        if (hasDropped && !hasCollision) {
            float fallstep = worldSettings.FallSpeed * Time.deltaTime;
            transform.position += Vector3.down * fallstep;
            float height = startPosition.y - worldSettings.LayerHeight + worldSettings.TileSize.y * 0.44f;

            if (transform.localPosition.y < height)
            {
                transform.localPosition = new Vector3(startPosition.x, height, startPosition.z);
                hasCollision = true;
                return;
            }

            /* Accurate collision code is disabled until optimizations are applied
             * for (int i = 0; i < selfColliders.Length; ++i)
            {
                for (int j = 0; j < belowTileColliders.Length; ++j)
                {
                    // If colliding, stop checking for collisions
                    if (selfColliders[i].bounds.Intersects(belowTileColliders[j].bounds))
                    {
                        hasCollision = true;
                        return;
                    }
                }
            }*/
        }
    }

   public void resetBlock()
    {
        Vector3 perlinPos = transform.localPosition * 0.1f;
        transform.localPosition = startPosition + Vector3.up * Mathf.PerlinNoise(perlinPos.x, perlinPos.z + perlinPos.y) * worldSettings.TerrainHeightVariation;
        hasCollision = hasDropped = false;
    }
}
