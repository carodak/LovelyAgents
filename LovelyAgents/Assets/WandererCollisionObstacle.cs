using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class WandererCollisionObstacle : MonoBehaviour
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
        if (other.gameObject.tag == "Obstacle")
        {

            //Slightly decrease the current speed of the traveller during 2seconds before avoiding the obstacle (via using an opposite force) 
            transform.parent.GetComponent<Wanderer>().timer2 = 2f;
            transform.parent.GetComponent<Wanderer>().obstacleNearby = 2;

            //Turning
            transform.parent.GetComponent<Wanderer>().obstacleNearby = 3;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.parent.GetComponent<Wanderer>().timer2 = 7f;

            //Behavior after turning
            transform.parent.GetComponent<Wanderer>().obstacleNearby = 4;


        }

    }
}
