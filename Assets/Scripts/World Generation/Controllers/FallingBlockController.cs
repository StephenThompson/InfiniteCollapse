using UnityEngine;

public class FallingBlockController : MonoBehaviour {
    public WorldDefinition wd;

    private Vector3 startPosition;
    private bool hasDropped;
    private bool hasCollision;
    private Vector3 size;

    public bool HasDropped
    {
        get { return hasDropped; }
        set { hasDropped = value; }
    }

	void Start () {
        size = GetComponent<Renderer>().bounds.size;
        startPosition = transform.localPosition;
    }

	void FixedUpdate () {

        if (hasDropped && !hasCollision) {
            RaycastHit rc;
            float fallstep = wd.constantValues.FALLING_SPEED * Time.deltaTime;
            float minStep = fallstep;
            for (int x = -1; x <= 1; ++x)
            {
                for (int z = -1; z <= 1; ++z)
                {
                    if (Physics.Raycast(transform.position + new Vector3(size.x * 0.4f * x, -size.y * 0.5f, size.z * 0.4f * x), Vector3.down, out rc))
                    {
                       if (rc.distance < minStep)
                        {
                            minStep = rc.distance;
                        }
                    }
                }
            }

            transform.position += Vector3.down * minStep;
            hasCollision = minStep + 0.001f < fallstep;
        }
	}

   public void resetBlock()
    {
        transform.localPosition = startPosition;
        hasCollision = hasDropped = false;
    }

    /*void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("ASDA");
        hasCollision = true;
    }*/
}
