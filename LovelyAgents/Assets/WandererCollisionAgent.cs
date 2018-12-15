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
        //If the agent detects a traveller they interfer
        if(other.gameObject.tag == "Agent" && other.gameObject.name == "TravellerBody")
        {
            transform.parent.GetComponent<Wanderer>().agentSpotted = true;

            transform.parent.GetComponent<Wanderer>().rb.velocity = Vector3.zero;
            transform.parent.GetComponent<Wanderer>().rb.angularVelocity = 0f;

            transform.parent.GetComponent<Wanderer>().SetDestination(other.gameObject.transform.parent.GetComponent<Traveller>().targetPosition);


            transform.parent.GetComponent<Wanderer>().obstacleNearby = 5;

        }

        //If the agent detects a non traveller agent, they move in another direction
        else if (other.gameObject.tag == "Agent")
        {

            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.parent.GetComponent<Rigidbody2D>().angularVelocity = 0f;

            int rand = Random.Range(1, 5);

            float rand2 = Random.Range(0.5f, 1.4f);

            if (rand == 1)
                transform.parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * rand2 *Time.deltaTime);

            else if (rand == 2)
                transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.down * rand2 * Time.deltaTime);

            else if (rand == 3)
                transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.left * rand2 * Time.deltaTime);

            else if (rand == 4)
                transform.parent.GetComponent<Rigidbody2D>().AddForce(-Vector2.down * rand2 * Time.deltaTime);

           transform.parent.GetComponent<Wanderer>().SetRandomDestination();


        }

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