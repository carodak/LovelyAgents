using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Traveller : GameAgent
{

    int door; //target = door number 1 or door number 2?

    public Traveller()
    {
    }

    // Use this for initialization
    void Start()
    {
        //Spawn the traveller at the right door
        timeBefNewDest = 5f;
        transform.position = GameObject.Find("Cube3").transform.position;
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(7f, 11f);//Move at random but high speed
    }


    // Update is called once per frame
    void Update()
    {
        FindPath();
    }

    void FindPath()
    {
        //Wait before the grid is generated for pathfinding
        if (!findPath)
        {

            // Get a reference to the Seeker component that would find our path
            seeker = GetComponent<Seeker>();

            ChooseDoor();

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

            UpdateAimedDoor();

        }

    }

    void UpdateAimedDoor()
    {
        //Change the aimed door 
        if (timeBefNewDest < 0 && door == 1)
        {
            AimFirstDoor();
        }

        if (timeBefNewDest < 0 && door == 2)
        {
            AimSecondDoor();
        }

    }

    void AimSecondDoor()
    {
        targetPosition = GameObject.Find("Cube1").transform.position;
        door = 1;
        timeBefNewDest = 10f;

        CheckNewPath();
    }

    void AimFirstDoor()
    {
        targetPosition = GameObject.Find("Cube2").transform.position;
        CheckNewPath();

        door = 2;
        timeBefNewDest = 5f;
    }

    void ChooseDoor()
    {
        //Choose randomly the door destination
        door = Random.Range(1, 3);
        if (door == 1)
        {
            targetPosition = GameObject.Find("Cube1").transform.position;
        }
        else
        {
            targetPosition = GameObject.Find("Cube2").transform.position;
        }
    }
}
