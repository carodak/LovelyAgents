using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class TravellerCollisionObstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Obstacle"){

            //Slightly decrease the current speed of the traveller during 2seconds before avoiding the obstacle (via using an opposite force) 
            transform.parent.GetComponent<Traveller>().timer2 = 2f;
            transform.parent.GetComponent<Traveller>().obstacleNearby = 2;

            //Turning
            transform.parent.GetComponent<Traveller>().obstacleNearby = 3;

        }

        //If we are close to the target door, let's cancel the forces and accelerate
        if(other.gameObject.name == "Cube1" || other.gameObject.name == "Cube2")
        {
            transform.parent.GetComponent<Traveller>().timer2 = 2f;
            transform.parent.GetComponent<Traveller>().obstacleNearby = 2;
            transform.parent.GetComponent<Traveller>().obstacleNearby = 1;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.parent.GetComponent<Traveller>().timer2 = 7f;

            //Behavior after turning
            transform.parent.GetComponent<Traveller>().obstacleNearby = 4;


        }

    }
}
