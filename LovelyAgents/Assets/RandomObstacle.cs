using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacle : MonoBehaviour {


	// Use this for initialization
	void Start () {

        //Random position on the area
        //transform.position = new Vector3(Random.Range(-35f, 25f), Random.Range(-22f, 22f), -1f);

        //Our obstacle should have between 4 and 16 vertices
        gameObject.GetComponent<Shaper2D>().sectorCount = Random.Range(4, 17);

        //Get a random simple polygon
        if (gameObject.GetComponent<Shaper2D>().sectorCount == 4 || gameObject.GetComponent<Shaper2D>().sectorCount ==5)
            gameObject.GetComponent<Shaper2D>().starrines = 0;
        else
            gameObject.GetComponent<Shaper2D>().starrines = Random.Range(0.03f, 0.14f);

        //Be sure that the size of the obstacles won't be less than 1/8 of the area (radius = 11.4 ~ 1/8 of the area) and won't exceed 1/2 of the area (radius = 27 ~ 1/2 of the area)
        int numberOfObstacles = GameObject.Find("Obstacles").GetComponent<GenerateObstacles>().n;

        gameObject.GetComponent<Shaper2D>().outterRadius = Random.Range((int)11.4/numberOfObstacles,(int)27/numberOfObstacles);
        gameObject.GetComponent<Shaper2D>().outterRadius = gameObject.GetComponent<Shaper2D>().outterRadius + 0.1f;

        //Put a circle collider around our polygon
        gameObject.GetComponent<CircleCollider2D>().radius = gameObject.GetComponent<Shaper2D>().outterRadius+ 1f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
