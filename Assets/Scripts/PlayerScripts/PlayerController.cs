using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //physics stuff
    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 acceleration = new Vector3(0, 0, 0);
    public float mass = 100;
    public float thrust = 0.05f;
    public float drag = 0.01f;
    public float angularDrag = 0.01f;
    public float forwardThrust = 1f;
    private Transform tr;
    public bool dead = false;
    //power up  stuff
    public float boostThrust = 10;
    private Vector3 boostDir = new Vector3(0, 0, 0);
    private float coolDownI = 4;
    float coolDown = 4;//cooldown when fuel gets to zero
    float fuelCapacity = 1000;
    public float fuelBurn = 15;
    bool onCoolDown = false;
//    private bool dead = false;
    //collision stuff
    public float MovingForce;
    Vector3 StartPoint;
    Vector3 Origin;
    public int NoOfRays = 10;
    int i;
    RaycastHit HitInfo;
    float LengthOfRay, DistanceBetweenRays, DirectionFactor;
    float LengthOfRay2;
    float LengthOfRay3;
    float DirectionFactor2;
    float DirectionFactor3;
    float margin = 0.015f;
    private BoxCollider collider1;
	//animation
	public Animator animator;

    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();
        collider1 = GetComponent<BoxCollider>();
        //Length of the Ray is distance from center to edge
        LengthOfRay = collider1.bounds.extents.y;
        LengthOfRay2 = collider1.bounds.extents.x;
        LengthOfRay3 = collider1.bounds.extents.z;
        //Initialize DirectionFactor for upward direction
        DirectionFactor = Mathf.Sign(Vector3.up.y);
        DirectionFactor2 = Mathf.Sign(Vector3.right.x);
        DirectionFactor3 = Mathf.Sign(Vector3.forward.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        Vector3 netForce = new Vector3(0, 0, 0);//this is the net force that is added to the object every update
        //not the overall netforce
            //takes player input and adds the thrust to the netforce
			if (Input.GetKey("s"))//down
            {
				netForce.y = netForce.y - thrust;
        		animator.SetBool("Down",true);
			}  
			else{
        		animator.SetBool("Down",false);
			}
			
			if (Input.GetKey("w"))//Up 
			{
				netForce.y = netForce.y + thrust;
        		animator.SetBool("Up",true);		
			} 
			else{
        		animator.SetBool("Up",false);
			}

			if (Input.GetKey("a"))//left
			{
				netForce.x = netForce.x - thrust;
        		animator.SetBool("Left",true);
			}else {
        			animator.SetBool("Left",false);
			}
			
			if (Input.GetKey("d"))//right
			{
				netForce.x = netForce.x + thrust;
        		animator.SetBool("Right",true);
			}else {
        			animator.SetBool("Right",false);
			}
        	

            //just a simple way of adding forward movement
            netForce.z = netForce.z + forwardThrust;
            boostDir = netForce.normalized;
        //this just uses newtons second law of motion F = ma. it is rearranged so, a = f/m
        //this is then subtracted by velocity mulriplied by a drag constant, as friction is
        //proportional to velocity.
        float boostX = 0;
        float boostY = 0;
        float boostZ = 0;
        if (Input.GetKey("space") && !onCoolDown)
        {
            boostX = boostDir.x * boostThrust / mass;
            boostY = boostDir.y * boostThrust / mass;
            boostZ = boostDir.z * boostThrust / mass;
            fuelCapacity = fuelCapacity - fuelBurn;
        }
        if(fuelCapacity <= 0)
        {
            onCoolDown = true;
            coolDown -= Time.deltaTime;
        }
        if(coolDown <= 0)
        {
            fuelCapacity = 0;
            onCoolDown = false;
            coolDown = coolDownI;
        }
        if(!onCoolDown && fuelCapacity < 1000)
        {
            fuelCapacity = fuelCapacity + 2;
        }
            float accelerationX = (netForce.x / mass) + boostX - velocity.x * drag;
            float accelerationY = (netForce.y / mass) + boostY - velocity.y * drag;
            float accelerationZ = (netForce.z / mass) + boostZ - velocity.z * drag;
            acceleration = new Vector3(accelerationX, accelerationY, accelerationZ);
            //then just update the velocity vector by adding the acceleration vector
            velocity.x = velocity.x + accelerationX;
            velocity.y = velocity.y + accelerationY;
            velocity.z = velocity.z + accelerationZ;
            LengthOfRay = collider1.bounds.extents.y + Mathf.Abs(velocity.y);
            LengthOfRay2 = collider1.bounds.extents.x + Mathf.Abs(velocity.x);
            LengthOfRay3 = collider1.bounds.extents.z + Mathf.Abs(velocity.z);
            // First ray origin point for this frame
            StartPoint = new Vector3(collider1.bounds.min.x + margin, transform.position.y, transform.position.z);
            if (IsCollidingVertically())
            {
            tr.parent = HitInfo.collider.gameObject.transform;
            dead = true;
            }
            if (IsCollidingHorizontally())
            {
            tr.parent = HitInfo.collider.gameObject.transform;
            dead = true;
            }
            if (IsCollidingForward())
           {
            tr.parent = HitInfo.collider.gameObject.transform;
            dead = true;

           }
            //finally transform the player by the velocity vector(via world co-ordinates)
            Vector3 relative = transform.InverseTransformDirection(velocity);
          if(!dead)tr.Translate(relative);
    }

    bool IsCollidingVertically()
    {
        Origin = StartPoint;
        DistanceBetweenRays = (collider1.bounds.size.x - 2 * margin) / (NoOfRays - 1);
        for (i = 0; i < NoOfRays; i++)
        {
            // Ray to be casted.
            Ray ray = new Ray(Origin, Vector3.up * DirectionFactor);
            Ray ray2 = new Ray(Origin, Vector3.down * DirectionFactor);
            //Draw ray on screen to see visually. Remember visual length is not actual length.
            Debug.DrawRay(Origin, Vector3.up * DirectionFactor, Color.yellow);
            if (Physics.Raycast(ray, out HitInfo, LengthOfRay)|| Physics.Raycast(ray2, out HitInfo, LengthOfRay))
            {
                if (!(HitInfo.collider.gameObject.name.Equals("ExhaustLeft")) && !(HitInfo.collider.gameObject.name.Equals("ExhaustRight")) &&
                    !(HitInfo.collider.gameObject.name.Equals("ShipBody1")))
                {
                    //print("Collided With " + HitInfo.collider.gameObject.name + " vertically " + Time.realtimeSinceStartup);
                    // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
                    DirectionFactor = -DirectionFactor;
                    return true;
                }
            }
            Origin += new Vector3(DistanceBetweenRays, 0, 0);
        }
        return false;
    }

    bool IsCollidingHorizontally()
    {
        Origin = StartPoint;
        DistanceBetweenRays = (collider1.bounds.size.z - 2 * margin) / (NoOfRays - 1);
        for (i = 0; i < NoOfRays; i++)
        {
            // Ray to be casted.
            Ray ray = new Ray(Origin, Vector3.left * DirectionFactor2);
            Ray ray2 = new Ray(Origin, Vector3.right * DirectionFactor2);
            //Draw ray on screen to see visually. Remember visual length is not actual length.
            Debug.DrawRay(Origin, Vector3.right * DirectionFactor2, Color.yellow);
            if (Physics.Raycast(ray, out HitInfo, LengthOfRay2) || Physics.Raycast(ray2, out HitInfo, LengthOfRay2))
            {
                if (!(HitInfo.collider.gameObject.name.Equals("ExhaustLeft")) && !(HitInfo.collider.gameObject.name.Equals("ExhaustRight"))&&
                    !(HitInfo.collider.gameObject.name.Equals("ShipBody1")))
                {
                    print("Collided With " + HitInfo.collider.gameObject.name + " horizontal-like " + Time.realtimeSinceStartup);
                    // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
                    DirectionFactor2 = -DirectionFactor2;
                    return true;
                }
            }
            Origin += new Vector3(0, DistanceBetweenRays, 0);
        }
        return false;
    }

    bool IsCollidingForward()
    {
        Origin = StartPoint;
        DistanceBetweenRays = (collider1.bounds.size.z - 2 * margin) / (NoOfRays - 1);
        for (i = 0; i < NoOfRays; i++)
        {
            // Ray to be casted.
            Ray ray = new Ray(Origin, Vector3.back * DirectionFactor3);
            Ray ray2 = new Ray(Origin, Vector3.forward * DirectionFactor3);
            //Draw ray on screen to see visually. Remember visual length is not actual length.
            Debug.DrawRay(Origin, Vector3.forward * DirectionFactor2, Color.yellow);
            if (Physics.Raycast(ray, out HitInfo, LengthOfRay3) || Physics.Raycast(ray2, out HitInfo, LengthOfRay3))
            {
                if (!(HitInfo.collider.gameObject.name.Equals("ExhaustLeft")) && !(HitInfo.collider.gameObject.name.Equals("ExhaustRight"))&&
                    !(HitInfo.collider.gameObject.name.Equals("ShipBody1"))) { 
                //print("Collided With " + HitInfo.collider.gameObject.name + " forwardly " + Time.realtimeSinceStartup);
                // Negate the Directionfactor to reverse the moving direction of colliding cube(here cube2)
                DirectionFactor3 = -DirectionFactor3;
                return true;
                }
            }
            Origin += new Vector3(0, DistanceBetweenRays, 0);
        }
        return false;
    }

}

