using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWanderers : MonoBehaviour
{

    [SerializeField]
    public int n = 4; //number of wanderers

    // Use this for initialization
    void Start()
    {
        CreateSocialAgents();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Create the agents. Spawn the agent randomly in the area but avoid overlapping

    void CreateSocialAgents()
    {
        GameObject obs = gameObject.transform.GetChild(0).gameObject;
        int i = 0;
        while (i < n)
        {
            Vector3 spawnPos = FindRandomSpawn();

            var checkResult = Physics2D.OverlapCircleAll(spawnPos, 1f);

            //If nothings overlap
            if (checkResult.Length == 0)
            {
                InstanciateGO(obs, spawnPos);
                i++;
            }
        }

        obs.SetActive(false);

    }

    //Instanciate the wanderer
    void InstanciateGO(GameObject obs, Vector3 spawnPos)
    {
        GameObject go = Instantiate(obs, spawnPos, Quaternion.identity);
        go.transform.parent = GameObject.Find("Wanderers").transform;
    }

    //Find a random spawn for the wanderer
    static Vector3 FindRandomSpawn()
    {
        return new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);
    }
}
