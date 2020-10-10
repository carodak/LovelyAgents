using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialCollisionSocial : MonoBehaviour
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
        if (other.gameObject.name == "SocialAgentBody")
        {

            //Let's decide with the agent would enter in a conversation
            int rand = Random.Range(1, 3);

            //Our agent will join the other one and they won't move until one leaves
            if(rand==2){
                Rigidbody2D rb2 = other.transform.parent.GetComponent<Rigidbody2D>();

                other.transform.parent.GetComponent<SocialAgent>().conversation = true;
                other.transform.parent.GetComponent<SocialAgent>().obstacleNearby = 0;

                Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();

                transform.parent.GetComponent<SocialAgent>().obstacleNearby = 0;
                transform.parent.GetComponent<SocialAgent>().conversation = true;

                //transform.parent.GetComponent<SocialAgent>().SetDestination(other.transform);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                rb2.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                transform.parent.GetComponent<SocialAgent>().timeBefLeaveGroup = Random.Range(0.5f, 2f);
                other.transform.parent.GetComponent<SocialAgent>().timeBefLeaveGroup = Random.Range(0.5f, 2f);
            }


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.name == "SocialAgentBody")
        {


        }


    }
}
