using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerCollisionAgent : MonoBehaviour
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

        if (other.gameObject.tag == "Agent")
        {
            //transform.parent.GetComponent<Traveller>().obstacleNearby = 5;
            // Debug.Log("Collinding with an agent");
            //var magnitude = 0.03f;
            // calculate force vector
           // var force = transform.parent.position - other.transform.parent.position;
           // normalize force vector to get direction only and trim magnitude
            //force.Normalize();
            //transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce(force * magnitude);
            transform.parent.GetComponent<Traveller>().obstacleNearby = 5;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        //We don't have any agent in front of us
        if (other.gameObject.tag == "Agent"){
            transform.parent.GetComponent<Traveller>().obstacleNearby = 6;
            transform.parent.rotation = Quaternion.identity;
        }

    }
}
