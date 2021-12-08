using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerControl : EntityControl
{

    private float boundary = 80.0f; //magnitude of x-oordinate that a runner object needs to reach to become safe

    private int direction;
    private bool moving_allowed;

    private int safe; //number of runner objects which have safely made it accross the map in a given generation

    public void IncrementSafe() { safe++; } //increments the integer 'safe'
    public void DecrementSafe() { safe--; } //decrements the integer 'safe'

    public float GetBoundary() { return boundary; } //getter method for boundary
    public int GetDirection() { return direction; } //getter method for direction
    public bool MovingAllowed() { return moving_allowed; }


    void Start()
    {
        direction = 1; //starting value of direction in the simulation
        moving_allowed = true;
    }


    void Update()
    {
        if (safe == TypeList.Count)
        {
            moving_allowed = false;
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("SPACE");
                direction *= -1;
                safe = 0;
            }
        }
        else
        {
            moving_allowed = true;
        }
    }
}
