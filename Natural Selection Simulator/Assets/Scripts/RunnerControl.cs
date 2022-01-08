using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerControl : EntityControl
{

    [SerializeField] GameObject runner_prefab;

    private float boundary = 80.0f; //magnitude of x-oordinate that a runner object needs to reach to become safe
    private int direction;
    private bool moving_allowed;

    private int safe; //number of runner objects which have safely made it accross the map in a given generation

    public void IncrementSafe() { safe++; } 
    public void DecrementSafe() { safe--; } 

    public float GetBoundary() { return boundary; } 
    public int GetDirection() { return direction; } 
    public bool MovingAllowed() { return moving_allowed; }

    private bool AllSafe()
    {
        if (safe == TypeList.Count)
        {
            Debug.Log("All runner objects are safe. (" + safe + ")");
            return true;
        }
        return false;
    }

    void Start()
    {
        SimulationControl = GameObject.Find("Control").GetComponent<SimulationControl>();

        direction = -1; //starting value of direction in the simulation
        moving_allowed = false;
        variance = 0.2f;


        for (int i = 0; i < SimulationControl.GetRunnerCount(); i++)
        {
            GameObject runner = Instantiate(runner_prefab, new Vector3(-70, 1.5f, Random.Range(-48, 48)), Quaternion.identity);
            Runner runner_script = runner.GetComponent<Runner>();
            runner_script.RecieveAttributes(SimulationControl.RunnerAttributes());
        }
    }

    void Update()
    {
        EnemyList = SimulationControl.GetTaggerControl().TypeList;
        if (moving_allowed)
        {
            if (AllSafe())
            {
                moving_allowed = false;
                //Debug.Log("moving_allowed: " + moving_allowed);
            }
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
                direction *= -1;
                safe = 0;
                foreach (GameObject runner in TypeList)
                {
                    runner.GetComponent<Runner>().NewGeneration();
                }
                moving_allowed = true;
                //Debug.Log("moving_allowed: " + moving_allowed);
                Debug.Log("Direction: " + direction);
            }
        }
    }
}
