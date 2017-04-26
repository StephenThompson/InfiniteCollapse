/**Use this script to create a flock of birds that flies towards a goal.
 * This early version of the script does not use flow fields. 
 * Change the destination vector to control where the birds move to.
 * Change the shape, maximum speed, and minimum separation distance of the birds
 * in the bird class.
 * 
 * Author Lawrence Buck 4/20/2017
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFlock : MonoBehaviour {
    //Globals
    public GameObject destination;                    
    Bird leader;                                        //Root of the tree structure
    ArrayList flock = new ArrayList();                  //List of all the birds for easy access

	//Initialization
    void Start () {
        leader = new Bird(0, 0, 0);

        //Put birds into flock
		for(int i = 0; i < 200; ++i)
        {
            Bird bird = new Bird(Random.Range(1.0f, -1.0f), Random.Range(1.0f, -1.0f), Random.Range(1.0f, -1.0f));
            flock.Add(bird);
            //Insert each bird into tree structure
            arrange(bird, leader);
        }
	}
	// Update is called once per frame
	void Update () {
        //Update leader destination and calacualte and apply steering vector...
        leader.goal = destination.transform.position;
        Steer(leader);

        //Do the same for each bird
        foreach(Bird b in flock)
        {
            b.goal = b.parent.position;
            Steer(b);
        }
	}

    /**
     * Method to calculate steering vector
     */
    void Steer(Bird b)
    {
        //Calculate and add alignment vector
        Vector3 alignment = Align(b);
        b.velocity += alignment;

        //Calculate and add separation vector
        Vector3 sep = Separate(b);
        b.velocity += sep * 0.5f;
        
        //Limit overall speed
        if(b.velocity.magnitude > b.maxSpeed)
        {
            b.velocity = b.velocity * (b.maxSpeed / b.velocity.magnitude);
        }

        //Finally, set position
        b.position += b.velocity;
        b.UpdatePosition();
    }

    /**
     * Method to calculate alignment vector
     */
    Vector3 Align(Bird b)
    {
        //Subtract bird's position from goal to get direction vector, then normalize and
        //mulitply by any scalar to get final velocity
        Vector3 dir = (b.goal - b.position).normalized;
        return dir * 0.02f;

    }

    /**
     * Method to calculate separation force
     */
    Vector3 Separate(Bird b)
    {   
        //Count number of neighbours
        int neighbours = 0;
        Vector3 velocity = new Vector3(0, 0, 0);

        foreach(Bird bird in flock)
        {
            //Only count other birds...
            if (bird != b)
            {
                //If the distance between other bird and this bird is below minimum separation distance...
                float distance = Vector3.Distance(b.position, bird.position);
                if (distance < b.minDist)
                {
                    //Add (normalized) direction vector between this bird and other bird to temporary velocity
                    velocity += (b.position - bird.position).normalized;
                    ++neighbours;
                }
            }
        }

        //We don't want the force to be too strong so divide by number of neighbours
        if(neighbours > 0)
        {
            return velocity / neighbours;
        }
        return velocity;
    }

    /**
     * Method to arrange birds into psuedo - binary tree structure
     */
    void arrange(Bird b, Bird n)
    {
        //If left child is null, set child
        if(n.left == null)
        {
            n.left = b;
            b.parent = n;
        }
        //Otherwise use right child
        else if(n.right == null)
        {
            n.right = b;
            b.parent = n;
        }
        //If both are full, recurse randomly down either branch
        else
        {
            if(Random.Range(0, 2) == 0)
            {
                arrange(b, n.left);
            }
            else
            {
                arrange(b, n.right);
            }
        }
    }


    /*=======================================================================*/
    /*                               CLASSES                                 */
    /*=======================================================================*/

    /*The bird class holds all the information to track the position of each bird, and its children*/
    public class Bird
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 goal;

        public float maxSpeed = 0.18f;
        public float minDist = 0.2f;

        public Bird parent;
        public Bird left;
        public Bird right;

        GameObject boid;

        /**
         * Constructor
         */
        public Bird(float x, float y, float z)
        {
            position.x = x;
            position.y = y;
            position.z = z;

            boid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            boid.transform.localScale = new Vector3(1, 0.7f, 0.7f);
        }

        /**
         * Method to update position and rotation
         */
        public void UpdatePosition()
        {
            boid.transform.position = position;
            boid.transform.right = velocity;
        }
    }
}
