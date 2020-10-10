using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GameAgent : MonoBehaviour
{
	public Vector3 targetPosition; //position of the target

	protected Seeker seeker; //to get full path

	public Path path;

	protected float speed; //current agent speed
	protected Vector3 velocity;

	public float nextWaypointDistance = 3;

	// The distance to the next waypoint in the path
	public float distanceToWaypoint;

	protected int currentWaypoint = 0;

	public float repathRate = 0.5f;
	protected float lastRepath = float.NegativeInfinity;

	public bool reachedEndOfPath; //did we reach the end of the path?

	public Vector3 dir; //direction for the next point of the grid

	public Rigidbody2D rb; //current rb

	public int obstacleNearby = 1; //use to know if we are accelerating, braking, deccelerating..

	public float maxSpeed; //maximal speed of the agent

	public float acc = 0.01f; //agents acceleration

	public float speedFactor;

	protected bool findPath = false; //did we find the pathfinding

	public float timer2 = 2f; //timer that we would use for the braking duration...

	public float timeBefNewDest; //time to reach next destination

	public GameAgent()
	{

	}

	//Update timers
	public void UpdateTimers()
	{
		timeBefNewDest -= Time.deltaTime;
		timer2 -= Time.deltaTime;
	}

	//Check if we have a new path to follow
	public void UpdateFindPath()
	{
		if (Time.time > lastRepath + repathRate && seeker!=null && seeker.IsDone())
		{
			lastRepath = Time.time;
			// Start a new path to the targetPosition, call the the OnPathComplete function
			// when the path has been calculated (which may take a few frames depending on the complexity)
			seeker.StartPath(transform.position, targetPosition, OnPathComplete);
		}
		if (path == null)
		{
			// We have no path to follow yet, so don't do anything
			return;
		}

		findPath = true;
	}

	//Go to next destination
	public void EndPath()
	{
		// Set a status variable to indicate that the agent has reached the end of the path.
		// You can use this to trigger some special code if your game requires that.
		reachedEndOfPath = true;
		SetRandomDestination();
		seeker.StartPath(transform.position, targetPosition, OnPathComplete);

		obstacleNearby = 1;
	}

	public void IncrCurrentWayPoint()
	{
		currentWaypoint++;
	}


	// Slow down smoothly upon approaching the end of the path
	// This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
	public void UpdateSpeedFactor()
	{
		speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
	}

	// Direction to the next waypoint
	public void UpdateDir()
	{
		dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		// Debug.Log("Dir: " + dir);
	}

	public void Way()
	{
		while (true)
		{
			distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

            // Check if there is another waypoint or if we have reached the end of the path
            if (distanceToWaypoint < nextWaypointDistance)
			{
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    IncrCurrentWayPoint();
                }
                else
                {
                    EndPath();
                    break;
                }
            }
			else
			{
				break;
			}
		}
	}

	public void UpdateSpeed()
	{
		// Multiply the direction by our desired speed to get a velocity
		velocity = dir * acc * speedFactor;
		speed = Vector3.Magnitude(rb.velocity);  // get the current object speed
												 //Debug.Log("speed: " + speed);
	}

	public void AgentAcceleration()
	{
		//Debug.Log("ICIIIIII");
		rb.AddForce(velocity);
	}

	public void CancelForceAndAcc()
	{
		//rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(velocity * 90f);
	}

	public void Turning()
	{
		//transform.Translate(dir/1.2f, Space.World);
		//transform.position = new Vector3(transform.position.x,transform.position.y,-2f);
		//rb.AddRelativeForce(dir * acc);
		// Recalculate only the first grid graph
		//rb.velocity = Vector3.zero;
		//rb.AddForce(dir * 70 * acc * speedFactor);

		rb.AddRelativeForce(dir * maxSpeed / 6 * acc, ForceMode2D.Impulse);
	}

	public void CancelForceAccAfterTurning()
	{
		//transform.position += dir.normalized * 3f * Time.deltaTime;
		//transform.position = new Vector3(transform.position.x, transform.position.y, -2);

		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(dir * 10 * maxSpeed * acc * speedFactor);
		//rb.angularVelocity = 0f;
		obstacleNearby = 1;
	}

	public void Deccelerate()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(dir * 10 * acc);
		//rb.AddForce(-GetComponent<Rigidbody2D>().velocity.normalized / 2 * Time.deltaTime);
	}

	public void AccelerateAfterDeccelerating()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(dir * maxSpeed / 20 * acc, ForceMode2D.Impulse);
		obstacleNearby = 1;
	}

	public void DeccelerateIfMaxSpeed()
	{
		//rb.velocity = new Vector3(-maxSpeed / 1.2f, 0f);
		//rb.velocity = Vector3.zero;
		//rb.AddForce(dir * 25 * acc * speedFactor);
		//rb.velocity *= 0.70f;

		rb.AddForce(-GetComponent<Rigidbody2D>().velocity.normalized / 4 * Time.deltaTime);
	}

	/*
    * MOVEMENTS
    * Move the agent with steering force: acc, braking and turning
    * Normalize it so that it has a length of 1 world unit
    * Direction to the next waypoint
    * Normalize it so that it has a length of 1 world unit
    */
	public void MoveAgentSteeringForces()
	{
		if (speed < 1)
		{
			obstacleNearby = 6;
		}

		//State 1: we are accelerating
		if (obstacleNearby == 1 && speed < maxSpeed)
		{
			AgentAcceleration();
		}

		//State 2: before avoiding the obstacle, we cancel the current forces and accelerate
		else if (obstacleNearby == 2 && timer2 > 0f)
		{
			CancelForceAndAcc();
		}

		//State 3: Turning
		else if (obstacleNearby == 3 && speed < maxSpeed)
		{
			Turning();
		}

		//State 4: after turning, we delete the current forces and accelerate
		else if (obstacleNearby == 4)
		{
			CancelForceAccAfterTurning();
		}

		//State 5: An agent is in front of us -> deccelerate
		else if (obstacleNearby == 5)
		{
			Deccelerate();
		}

		//State 6: After deccelerate: accelerate
		else if (obstacleNearby == 6)
		{
			AccelerateAfterDeccelerating();
		}

		//Limit of the acceleration: decelerate if we reached the maximum speed
		else if (speed > maxSpeed)
		{
			DeccelerateIfMaxSpeed();
		}
	}

    public void CheckNewPath()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            StartNewPath();
        }
    }

    // Start a new path to the targetPosition, call the the OnPathComplete function
    // when the path has been calculated (which may take a few frames depending on the complexity)
    public void StartNewPath()
    {
        lastRepath = Time.time;

        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }

    public void OnPathComplete(Path p)
	{
		//Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

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

    public void SetDestination(Vector3 t)
    {

        targetPosition = new Vector3(t.x + 8f, t.y, t.z);

        lastRepath = Time.time;
        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);

    }

    //Set RandomDestination if we have not find any agent to annoy
    public void SetRandomDestination()
    {
        Vector3 destination = new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);

        while (Vector3.Distance(transform.position, destination) < 40f)
        {
            destination = new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);
        }

        targetPosition = destination;
    }

}
