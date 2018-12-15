using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Traveller : MonoBehaviour
{

    public Transform targetPosition; //position of the target = door

    private Seeker seeker; //to get full path

    public Path path;

    private float speed; //current traveller speed

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath; //did we reach the end of the path?

    public Vector3 dir; //direction for the next point of the grid

    public Rigidbody2D rb; //current rb

    public int obstacleNearby = 1; //use to know if we are accelerating, braking, deccelerating..

    public float maxSpeed; //maximal speed of a traveller

    public float acc = 0.01f; //travellers acceleration

    bool findPath = false; //did we find the pathfinding

    public float timer2 = 2f; //timer that we would use for the braking duration...

    public float timeLeft = 5f; //time to reach the door or change the targeted door

    int door; //target = door number 1 or door number 2?

    // Use this for initialization
    void Start()
    {
        //Spawn the traveller at the right door
        transform.position = GameObject.Find("Cube3").transform.position;
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(7f, 11f);//Move at random but high speed
    }


    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        //Wait before the grid is generated for pathfinding
        if (transform.parent.GetComponent<GenerateTravellers>().activeScan == true && !findPath)
        {

            // Get a reference to the Seeker component that would find our path
            seeker = GetComponent<Seeker>();


            //Choose randomly the door destination
            door = Random.Range(1, 3);
            if (door == 1)
            {
                targetPosition = GameObject.Find("Cube1").transform;
            }
            else
            {
                targetPosition = GameObject.Find("Cube2").transform;
            }

            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }
            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                return;
            }

            findPath = true;
        }

        if (findPath)
        {
            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.

            reachedEndOfPath = false;

            // The distance to the next waypoint in the path

            float distanceToWaypoint;

            while (true)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.

            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            /*
             * MOVEMENTS
             * 
             * Move the agent with steering force: acc, braking and turning
             */

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit

            dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            // Multiply the direction by our desired speed to get a velocity

            Vector3 velocity = dir * acc * speedFactor;
            // Debug.Log("Dir: " + dir);

            speed = Vector3.Magnitude(rb.velocity);  // get the current object speed
            //Debug.Log("speed: " + speed);

            //State 1: we are accelerating
            if (obstacleNearby == 1 && speed < maxSpeed)
            {
                rb.AddForce(velocity);

            }

            //State 2: before avoiding the obstacle, we cancel the current forces and accelerate
            else if (obstacleNearby == 2 && timer2 > 0f)
            {
                //rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(velocity * 90f);
            }



            //State 3: Turning
            else if (obstacleNearby == 3 && speed < maxSpeed)
            {
                //transform.Translate(dir/1.2f, Space.World);
                //transform.position = new Vector3(transform.position.x,transform.position.y,-2f);
                //rb.AddRelativeForce(dir * acc);
                // Recalculate only the first grid graph
                //rb.velocity = Vector3.zero;
                //rb.AddForce(dir * 70 * acc * speedFactor);

                rb.AddRelativeForce(dir * maxSpeed/6*acc, ForceMode2D.Impulse);
            }

            //State 4: after turning, we delete the current forces and accelerate
            else if (obstacleNearby == 4)
            {

                //transform.position += dir.normalized * 3f * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x, transform.position.y, -2);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(dir * 10 * maxSpeed * acc  * speedFactor);
                //rb.angularVelocity = 0f;
                obstacleNearby = 1;

            }

            //State 5: An agent is in front of us -> deccelerate
            else if(obstacleNearby ==5){
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(dir * 10 * acc);
                //rb.AddForce(-GetComponent<Rigidbody2D>().velocity.normalized / 2 * Time.deltaTime);
            }

            //State 6: After deccelerate: accelerate
            else if (obstacleNearby == 6)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(dir * maxSpeed / 20 * acc, ForceMode2D.Impulse);
                obstacleNearby = 1;

            }

            //Limit of the acceleration: decelerate if we reached the maximum speed
            else if (speed > maxSpeed)
            {
                //rb.velocity = new Vector3(-maxSpeed / 1.2f, 0f);
                //rb.velocity = Vector3.zero;
                //rb.AddForce(dir * 25 * acc * speedFactor);
                //rb.velocity *= 0.70f;

                rb.AddForce(-GetComponent<Rigidbody2D>().velocity.normalized / 4 * Time.deltaTime);
            }

            //Change the aimed door 
            if (timeLeft<0 && door==1){

                targetPosition = GameObject.Find("Cube2").transform;


                if (Time.time > lastRepath + repathRate && seeker.IsDone())
                {
                    lastRepath = Time.time;
                    // Start a new path to the targetPosition, call the the OnPathComplete function
                    // when the path has been calculated (which may take a few frames depending on the complexity)
                    seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
                }
     

                door = 2;
                timeLeft = 5f;
            }

            if (timeLeft < 0 && door == 2)
            {
                targetPosition = GameObject.Find("Cube1").transform;
                door = 1;
                timeLeft = 10f;

                if (Time.time > lastRepath + repathRate && seeker.IsDone())
                {
                    lastRepath = Time.time;
                    // Start a new path to the targetPosition, call the the OnPathComplete function
                    // when the path has been calculated (which may take a few frames depending on the complexity)
                    seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
                }
            }
        }
    }



    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }


}
