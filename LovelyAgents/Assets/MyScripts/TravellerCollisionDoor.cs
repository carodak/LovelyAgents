using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerCollisionDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy the traveller if they reached their goal
        if (other.gameObject.name == "Cube1" || other.gameObject.name == "Cube2")
        {
            Destroy(transform.parent.gameObject);
            transform.parent.parent.GetComponent<GenerateTravellers>().nbCreated -= 1;
        }
    }
}
