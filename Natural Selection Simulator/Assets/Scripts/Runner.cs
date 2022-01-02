using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Entity
{

    private RunnerControl RunnerControl;
    private float boundary;

    private bool safe_this_generation;
    private bool moving;
    private int times_crossed;
    private float fear_coefficient;

    private bool Safe()  
    {
        if ((body.position.x)*RunnerControl.GetDirection() >= boundary) 
        {
            times_crossed++;
            //Debug.Log("Safe position reached at position " + body.position);
            body.position = new Vector3(boundary * RunnerControl.GetDirection() - 10 * RunnerControl.GetDirection(), body.position.y, body.position.z);
            //Debug.Log("Object repositioned to " + body.position);
            return true;
        } 
        //if the position of a runner object is past the safe boundary the method will return true
        //if (body.position.x < -boundary) and (direction = -1) then body.position.x*direction > boundary
        //similar reasoning for if (body.position.x > boundary) and (direction = 1), therefore condition is true when runner object is past the safe line

        return false;
    } 

    public void NewGeneration() { safe_this_generation = false; }

    private Vector3 CalculateVelocityVector()
    {
        Vector3 ClosestEnemy = LocateClosestEnemy(RunnerControl.EnemyList);
        Debug.Log("Closest tagger: " + ClosestEnemy);
        return new Vector3(RunnerControl.GetDirection(), 0, 0) * speed; //filler for version one
    }

    void Start()
    {
        self = this.gameObject;
        RunnerControl = GameObject.Find("Control").GetComponent<RunnerControl>();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        safe_this_generation = false;
        times_crossed = 0;
        boundary = RunnerControl.GetBoundary();

        speed = 70; //filler value //speed + Random.Range(-speed * RunnerControl.variance, speed * RunnerControl.variance);
        size = size + Random.Range(-size * RunnerControl.variance, size * RunnerControl.variance);
        efficiency = efficiency + Random.Range(-efficiency * RunnerControl.variance, efficiency * RunnerControl.variance);
        fear_coefficient = fear_coefficient + Random.Range(-fear_coefficient * RunnerControl.variance, fear_coefficient * RunnerControl.variance);

        RunnerControl.TypeList.Add(self); //adds object to the list containing all runner instances
    }


    void Update()
    {
        if (RunnerControl.MovingAllowed() && !safe_this_generation)
        { //only checks if object is safe in the cases where it is possible for an object to be not safe
            if (!Safe()) 
            {
                moving = true;
                //Debug.Log(name + " moving: " + moving);
            }
            else //if object has reached the safe boundary
            {
                safe_this_generation = true;
                moving = false;
                Debug.Log(name + " moving: " + moving);

                RunnerControl.IncrementSafe(); //tells 'RunnerControl' that a runner object is safe 
            }
        }
        else if (!RunnerControl.MovingAllowed())
        { //if 'moving allowed' is false then 'safe_this_generation' will be true
            if (times_crossed == 3)
            {
                Reproduce();
                times_crossed = 0;
            }
        }
    }


    private void FixedUpdate()
    {
        if (moving)
        {
            body.velocity = CalculateVelocityVector();
        }
        else
        {
            body.velocity = Vector3.zero;
        }
    }

}
