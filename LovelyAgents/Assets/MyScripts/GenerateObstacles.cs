using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour {

    [SerializeField]
    public int n = 4; //number of obstacles

	// Use this for initialization
	void Start () {

        CreateObstacles();
        GetObstaclesSize();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateObstacles(){
        //Use the created obstacle to make the others
        GameObject obs = gameObject.transform.GetChild(0).gameObject;


        int i = 0; Vector3 spawnPos;
        while (i < n)
        {
            spawnPos = GetPositionObstacle();

            var checkResult = Physics2D.OverlapCircleAll(spawnPos, 3f);

            if (checkResult.Length == 0)
            {
                CreateObstacle(obs, spawnPos);
                i++;
            }
        }

        obs.SetActive(false);


    }

    //Instanciate obs 
    void CreateObstacle(GameObject obs, Vector3 spawnPos)
    {
        GameObject go = Instantiate(obs, spawnPos, Quaternion.identity);
        go.transform.parent = GameObject.Find("Obstacles").transform;
    }

    //Generate random position for the obstacles
    Vector3 GetPositionObstacle()
    {
        Vector3 spawnPos;
        if (n <= 3)
        {
            spawnPos = new Vector3(Random.Range(-20f, 20f), Random.Range(-19f, 19f), -2f);
        }

        else
            spawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(-19f, 19f), -2f);
        return spawnPos;
    }

    float GetObstaclesSize(){
        float obstaclesSize = 0f;

        //Check the size of obstacles to be sure that they are capable of blocking at the maximum 1/2 of the area

        for (int i = 0; i < n - 1; i++)
        {
            obstaclesSize += gameObject.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Shaper2D>().outterRadius;
        }

        return obstaclesSize;
    }
}
