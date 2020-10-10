using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererCollisionAgent : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Agent" && other.gameObject.name == "TravellerBody")
        {
            Interference(other);

        }

        
        else if (other.gameObject.tag == "Agent")
        {
            MoveOtherDirection();

        }

    }

    //If the agent detects a non traveller agent, they move in another direction
    void MoveOtherDirection()
    {
        transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        transform.parent.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        int rand = Random.Range(1, 5);

        float rand2 = Random.Range(0.5f, 1.4f);
        MoveAgent(rand, rand2);

        transform.parent.GetComponent<Wanderer>().SetRandomDestination();
    }

    //AddForces to the agent and make it moves
    void MoveAgent(int rand, float rand2)
    {
        if (rand == 1)
            transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * rand2 * Time.deltaTime);

        else if (rand == 2)
            transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.down * rand2 * Time.deltaTime);

        else if (rand == 3)
            transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.left * rand2 * Time.deltaTime);

        else if (rand == 4)
            transform.parent.GetComponent<Rigidbody2D>().AddForce(-Vector2.down * rand2 * Time.deltaTime);
    }

    //If the agent detects a traveller they interfer
    void Interference(Collider2D other)
    {
        transform.parent.GetComponent<Wanderer>().agentSpotted = true;

        transform.parent.GetComponent<Wanderer>().rb.velocity = Vector3.zero;
        transform.parent.GetComponent<Wanderer>().rb.angularVelocity = 0f;

        transform.parent.GetComponent<Wanderer>().SetDestination(other.gameObject.transform.parent.GetComponent<Traveller>().targetPosition);


        transform.parent.GetComponent<Wanderer>().obstacleNearby = 5;
    }

    void OnTriggerExit2D(Collider2D other)
    {

        //We don't have any agent in front of us
        if (other.gameObject.tag == "Agent" && other.gameObject.name == "TravellerBody"){
    
            transform.parent.GetComponent<Wanderer>().obstacleNearby = 6;
            transform.parent.GetComponent<Wanderer>().agentSpotted = false;
            transform.parent.rotation = Quaternion.identity;
        }

        else if (other.gameObject.tag == "Agent")
        {

            transform.parent.GetComponent<Wanderer>().obstacleNearby = 6;

        }

    }
}