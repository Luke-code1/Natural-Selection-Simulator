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

    private bool GenerationStart()
    {
        foreach (GameObject runner in TypeList)
        {
            Runner runner_script = runner.GetComponent<Runner>();
            SimulationControl.AppendGenerationRunnerData(runner_script.Attributes()); //adds attributes of runner to an array
            runner_script.NewGeneration(); //'safe_this_generation' set to false
        }
        return true;
    }

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

        starting_energy = 70000f; //filler value

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
                SimulationControl.r_tag = GenerationStart();
                moving_allowed = true;
                //Debug.Log("moving_allowed: " + moving_allowed);
                Debug.Log("Direction: " + direction);
            }
        }
    }
}
