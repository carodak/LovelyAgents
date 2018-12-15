using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTravellers : MonoBehaviour
{

    [SerializeField]
    public int n = 4; //number of travellers
    public bool activeScan = false;
    float timeLeft = 0.5f;

    float creationTime = 3f;//every each 3 secondes a traveller will spawn

    public int nbCreated = 1;


    // Use this for initialization
    void Start()
    {
       GameObject.Find("Agents").GetComponent<AstarPath>().showNavGraphs = false;
        GameObject.Find("Agents").GetComponent<AstarPath>().showUnwalkableNodes = false;
    }

    // Update is called once per frame
    void Update()
    {
        creationTime -= Time.deltaTime;

        //Check if we need to create travellers
        createTravellers();

        //Wait before the obstacles are generated to get the path
        timeLeft -= Time.deltaTime;

        //Scan the terrain and get a path (pathfinding)
        if (timeLeft < 0 && !activeScan)
        {
            AstarPath.active.Scan();
            activeScan = true;
        }

    }

    //Create the travellers
    void createTravellers()
    {

        if (nbCreated < n && creationTime < 0f)
        {
            GameObject obs = gameObject.transform.GetChild(0).gameObject;
            GameObject go = Instantiate(obs, GameObject.Find("Cube3").transform.position, Quaternion.identity);
            go.transform.parent = GameObject.Find("Travellers").transform;
            creationTime = 3f;
            nbCreated++;
        }

    }
}
