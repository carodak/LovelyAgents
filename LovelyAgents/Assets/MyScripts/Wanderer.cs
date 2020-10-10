using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Wanderer : GameAgent
{

    public bool agentSpotted = false;

    public Wanderer()
    {
    }

    // Use this for initialization
    void Start()
    {

        timeBefNewDest = 4f;
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(7f, 11f);//Move at random but high speed

        SetRandomDestination();

    }


    // Update is called once per frame
    void Update()
    {
        PerformActions();
    }

    private void PerformActions()
    {
        //Wait before the grid is generated for pathfinding
        //if (GameObject.Find("Travellers").GetComponent<GenerateTravellers>().activeScan == true && !findPath)
        if (!findPath)
        {
            // Get a reference to the Seeker component that would find our path
            seeker = GetComponent<Seeker>();

            CheckNewPath();

            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                return;
            }

            findPath = true;
        }

        UpdateTimers();
        UpdateFindPath();


        if (findPath)
        {
            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.

            reachedEndOfPath = false;

            Way();

            UpdateSpeedFactor();

            UpdateDir();

            UpdateSpeed();

            MoveAgentSteeringForces();

            //Repath randomly if we didnt see a traveller

            CheckRepath();
        }
    }

    void CheckRepath()
    {
        if (timeBefNewDest <= 0 && !agentSpotted)
        {
            RepathRandomly();
        }
    }

    //Repath and find a new destination randomly
    void RepathRandomly()
    {
        SetRandomDestination();

        float rand = Random.Range(2f, 10f);
        timeBefNewDest = rand;

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);

        obstacleNearby = 1;
    }
}
