using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {
    ArrayList activePowerups;            //List of all powerups that are interactable
    Queue inactivePowerups;              //List of all the inactive powerups

    int MAX_POWERUPS;                    //Maximum active powerups

    Vector2 SPAWN_TIMER_RANGE;           //Determines range of time for how frequently powerups can spawn

    public GameObject powerupMesh;       //Select powerup model in the editor
    public GameObject playerCharacter;   //Select player character in the editor

    float timeBuffer;                    //Used for timing calculations
    float spawnTimer;

    Vector3 posBuffer;                   //Used for speed calculations

    // Use this for initialization
    void Start () {
        activePowerups = new ArrayList();
        inactivePowerups = new Queue();

        MAX_POWERUPS = 3;

        SPAWN_TIMER_RANGE = new Vector2(30, 5);

        for (int i = 0; i < MAX_POWERUPS; ++i)
        {
            inactivePowerups.Enqueue(new PowerUp(powerupMesh));
        }

        timeBuffer = Time.time;
        spawnTimer = Random.Range(SPAWN_TIMER_RANGE.x, SPAWN_TIMER_RANGE.y);

        posBuffer = playerCharacter.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //Get player speed to use later
        Vector3 vel = playerCharacter.transform.position - posBuffer;
        posBuffer = playerCharacter.transform.position;

        //Check if we need to spawn a new powerup
        float timeSinceLastSpawn = Time.time - timeBuffer;
        if (SpawnPowerup(timeSinceLastSpawn))
        {
            //If we do, update time buffer and set new spawntime
            timeBuffer = Time.time;
            spawnTimer = Random.Range(SPAWN_TIMER_RANGE.x, SPAWN_TIMER_RANGE.y);
        }

        
        //If we have active powerups...
        if(activePowerups.Count > 0)
        {
            UpdateActivePowerups(vel);
        }
	}

    //Handle powerups that need to be despawned or updated
    void UpdateActivePowerups(Vector3 vel)
    {
        //Keep temp buffer of objects to remove
        ArrayList toRemove = new ArrayList();
        foreach (PowerUp p in activePowerups)
        {
            //If powerup is not connected
            if (!p.connected)
            {
                //Check to see player is in range
                p.CheckCollect(playerCharacter.transform.position);
                //Check to see if player missed this one
                if (p.Missed(playerCharacter.transform.position))
                {
                    toRemove.Add(p);
                }
            }
            else
            {
                //Otherwise, chase player
                p.Chase(playerCharacter.transform.position, vel);
                //Check to see if powerup is absorbed
                if (p.Collect(playerCharacter.transform.position))
                {
                    toRemove.Add(p);
                }
            }
        }

        //Swap powerups around
        foreach (PowerUp p in toRemove)
        {
            activePowerups.Remove(p);
            inactivePowerups.Enqueue(p);
            p.Deactivate();
        }
    }

    //Spawns new powerups by swapping from inactive to active states
    bool SpawnPowerup(float delta) {
        //Check if it is time to spawn
        if(delta > spawnTimer)
        {
            //Take one off inactive queue...
            PowerUp p = (PowerUp)inactivePowerups.Dequeue();

            //And place into active queue and activate
            activePowerups.Add(p);
            p.Activate(playerCharacter.transform.position);
            return true;
        }
        return false;
    }

    /*=======================================================================*/
    /*                               CLASSES                                 */
    /*=======================================================================*/

    public class PowerUp
    {
        public Vector3 position;
        public bool connected;
        public Vector3 velocity;

        int leashRange;             //Distance player needs to be to collect powerup
        int minDist;                //Distance player needs to be to collect powerup

        //Speed measured in scalars from 0 to 1
        float maxspeed;             //Maximum rate at whcih powerup chases player
        float speed;                //Rate at whcih powerup chases player
        float acceleration;         //Rate at which powerup accelerates

        //Powerups always spawn in front of the player.
        int spawnRange;             //Distance in front
        Vector2 xDeviation;         //Random distance from side to side
        Vector2 yDeviation;         //Random distance up and down

        GameObject mesh;
        public PowerUp(GameObject m)
        {
            mesh = Instantiate(m);

            position = new Vector3(0, 0, 0);
            connected = false;

            leashRange = 30;
            minDist = 5;

            maxspeed = 0.6f;
            speed = 0;
            acceleration = 0.01f;

            spawnRange = 500;
            xDeviation = new Vector2(-50, 50);
            yDeviation = new Vector2(-30, 30);
        }

        //Check if player is in range of this powerup to collect it
        public void CheckCollect(Vector3 pos)
        {
            if(Vector3.Distance(position, pos) <= leashRange)
            {
                connected = true;
            }
        }

        //Activate by turning on mesh renderer and moving in front of player
        public void Activate(Vector3 playerPos)
        {
            MeshRenderer mr = mesh.GetComponent<MeshRenderer>();
            mr.enabled = true;

            float x = playerPos.x + Random.Range(xDeviation.x, xDeviation.y);
            float y = playerPos.y + Random.Range(yDeviation.x, yDeviation.y);
            float z = playerPos.z + spawnRange;

            UpdatePosition(new Vector3(x, y, z));
        }

        public void Deactivate()
        {
            MeshRenderer mr = mesh.GetComponent<MeshRenderer>();
            mr.enabled = false;
        }

        public void Chase(Vector3 dest, Vector3 v)
        {
            Vector3 alignment = dest - position;

            alignment = alignment * (v.magnitude*0.8f / alignment.magnitude);

            speed += acceleration;

            if(speed > maxspeed)
            {
                speed = maxspeed;
            }

            velocity = alignment * (1 + speed);

            UpdatePosition(position += velocity);
        }

        public bool Collect(Vector3 pos)
        {
            if(Vector3.Distance(pos, position) < minDist)
            {
                return true;
            }
            return false;
        }

        public bool Missed(Vector3 pos)
        {
            if(position.z + 50 < pos.z)
            {
                return true;
            }
            return false;
        }

        public void UpdatePosition(Vector3 pos)
        {
            position = pos;
            mesh.transform.position = position;
        }
    }
}
