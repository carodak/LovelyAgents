using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Wanderer : MonoBehaviour
{

    public Transform targetPosition; //position of the target = random in the area

    private Seeker seeker; //to get full path

    public Path path;

    private float speed; //current agent speed

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath; //did we reach the end of the path?

    public Vector3 dir; //direction for the next point of the grid

    public Rigidbody2D rb; //current rb

    public int obstacleNearby = 1; //use to know if we are accelerating, braking, deccelerating..

    public float maxSpeed; //maximal speed of the agent

    public float acc = 0.01f; //agents acceleration

    bool findPath = false; //did we find the pathfinding

    public float timer2 = 2f; //timer that we would use for the braking duration...

    public float timeBefNewDest = 4f;

    GameObject emptyGO;

    public bool agentSpotted = false;


    // Use this for initialization
    void Start()
    {


        //transform.position = GameObject.Find("Cube3").transform.position;
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(7f, 11f);//Move at random but high speed

        SetRandomDestination();

    }


    // Update is called once per frame
    void Update()
    {
        timeBefNewDest -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        //Wait before the grid is generated for pathfinding
        if (GameObject.Find("Travellers").GetComponent<GenerateTravellers>().activeScan == true && !findPath)
        {

            // Get a reference to the Seeker component that would find our path
            seeker = GetComponent<Seeker>();

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
                        SetRandomDestination();
                        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

                        obstacleNearby = 1;
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

            if(speed<1){
                obstacleNearby = 6;
            }

            //State 1: we are accelerating
            if (obstacleNearby == 1 && speed < maxSpeed)
            {
                //Debug.Log("ICIIIIII");
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

                rb.AddRelativeForce(dir * maxSpeed / 6 * acc, ForceMode2D.Impulse);
            }

            //State 4: after turning, we delete the current forces and accelerate
            else if (obstacleNearby == 4)
            {

                //transform.position += dir.normalized * 3f * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x, transform.position.y, -2);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(dir * 10 * maxSpeed * acc * speedFactor);
                //rb.angularVelocity = 0f;
                obstacleNearby = 1;

            }

            //State 5: An agent is in front of us -> deccelerate
            else if (obstacleNearby == 5)
            {
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

            //Repath randomly if we didnt see a traveller

            if (timeBefNewDest <= 0 && !agentSpotted)
            {
                SetRandomDestination();

                float rand = Random.Range(2f, 10f);
                timeBefNewDest = rand;

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

                obstacleNearby = 1;
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

    public void SetDestination(Transform t)
    {

        targetPosition.position = new Vector3(t.position.x + 8f, t.position.y, t.position.z);

        lastRepath = Time.time;
        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

    }

    //Set RandomDestination if we have not find any agent to annoy
    public void SetRandomDestination()
    {
        GameObject empGO = new GameObject();
        Vector3 destination = new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);

        while (Vector3.Distance(transform.position,destination)<40f){
            destination = new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);
        }

        empGO.transform.position = destination;

        targetPosition = empGO.transform;
    }


}
