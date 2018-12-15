using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSocialAgents : MonoBehaviour
{

    [SerializeField]
    public int n = 4; //number of wanderers

    // Use this for initialization
    void Start()
    {
        createTravellers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Create the travellers. Spawn the agent randomly in the area but avoid overlapping

    void createTravellers()
    {
        GameObject obs = gameObject.transform.GetChild(0).gameObject;
        int i = 0;
        while(i<n){
            Vector3 spawnPos = new Vector3(Random.Range(-38f, 38f), Random.Range(-19f, 19f), -2f);

            var checkResult = Physics2D.OverlapCircleAll(spawnPos, 1f);
            if (checkResult.Length == 0)
            {

                GameObject go = Instantiate(obs, spawnPos, Quaternion.identity);
                go.transform.parent = GameObject.Find("Wanderers").transform;
                i++;
            }
        }

        obs.SetActive(false);

    }
}
