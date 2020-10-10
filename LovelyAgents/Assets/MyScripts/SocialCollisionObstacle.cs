using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SocialCollisionObstacle : MonoBehaviour
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
            transform.parent.GetComponent<SocialAgent>().timer2 = 2f;
            transform.parent.GetComponent<SocialAgent>().obstacleNearby = 2;

            //Turning
            transform.parent.GetComponent<SocialAgent>().obstacleNearby = 3;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.parent.GetComponent<SocialAgent>().timer2 = 7f;

            //Behavior after turning
            transform.parent.GetComponent<SocialAgent>().obstacleNearby = 4;


        }

    }
}
