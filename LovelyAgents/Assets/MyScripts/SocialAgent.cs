using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SocialAgent : GameAgent
{


    public bool conversation;

    public float timeBefLeaveGroup = 2f;

    public float timeWanderer = 10f;//during 10secs, the agent that left the conversation would become a wanderer

    public bool wanderer;

    // Use this for initialization
    void Start()
    {
        timeBefNewDest = 4f;

        //transform.position = GameObject.Find("Cube3").transform.position;
        rb = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(7f, 11f);//Move at random but high speed

        SetRandomDestination();

        conversation = false;

        wanderer = false;

    }

    new void UpdateTimers()
    {
        timeBefLeaveGroup -= Time.deltaTime;
        timeBefNewDest -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        timeWanderer -= Time.deltaTime;
    }


    // Update is called once per frame
    void Update()
    {
        PerformActions();
    }

    void PerformActions()
    {
        //Wait before the grid is generated for pathfinding
        if (GameObject.Find("Travellers").GetComponent<GenerateTravellers>().activeScan == true && !findPath)
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

            PerformSocialActions();

        }
    }

    //move randomly, enter a group or become a wanderer
    void PerformSocialActions()
    {
        //Repath randomly if we are not in a conversation

        if (timeBefNewDest <= 0 && !conversation)
        {
            RepathRandomly();
        }

        if (timeBefLeaveGroup <= 0 && conversation)
        {
            BecomeWanderer();
        }

        if (wanderer && timeWanderer <= 0f)
        {
            GetBackToSocialAgent();
        }
    }

    //Stop being a wanderer agent
    void GetBackToSocialAgent()
    {
        wanderer = false;

        Renderer rend = GetComponent<Renderer>();

        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.yellow);

        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        SetRandomDestination();
    }

    //Change the aspect and become a wanderer agent
    void BecomeWanderer()
    {
        conversation = false;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        Renderer rend = GetComponent<Renderer>();

        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.green);

        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        SetRandomDestination();
        wanderer = true;
        timeWanderer = 10f;
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
