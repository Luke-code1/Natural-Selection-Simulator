using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Entity
{

    private RunnerControl RunnerControl;
    private float boundary;

    private bool safe;
    private bool moving;
    private int times_crossed;
    private float fear_coefficient;


    public void GiveAttributes(float parent_speed, float parent_size, float parent_efficiency, float parent_fear_coefficient, Vector3 parent_position) 
    {
        base.GiveAttributes(parent_speed, parent_size, parent_efficiency, parent_position); 
        fear_coefficient = parent_fear_coefficient;
    } //overload of parent class method to include 'fear_coefficient'


    private bool Safe()  
    {
        if ((body.position.x)*RunnerControl.GetDirection() >= boundary) 
        {
            Debug.Log("Safe position reached at position " + body.position);
            body.position = new Vector3(boundary * RunnerControl.GetDirection() - 10 * RunnerControl.GetDirection(), body.position.y, body.position.z);
            times_crossed++;
            Debug.Log("Object repositioned to " + body.position);
            Debug.Log("times_crossed = " + times_crossed);
            return true;
        } 
        //if the position of a runner object is past the safe boundary the method will return true
        //if (body.position.x < -boundary) and (direction = -1) then body.position.x*direction > boundary
        //similar reasoning for if (body.position.x > boundary) and (direction = 1), therefore condition is true when runner object is past the safe line

        return false;
    } 


    private Vector3 CalculateVelocityVector()
    {
        return new Vector3(RunnerControl.GetDirection(), 0, 0) * speed; //filler for version one
    }


    void Start()
    {
        RunnerControl = GameObject.Find("EntityControls").GetComponent<RunnerControl>();
        boundary = RunnerControl.GetBoundary(); 

        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        speed = 45; //filler value for version one

        RunnerControl.TypeList.Add(self); //adds object to the list containing all runner instances
        Reproduce();

    }


    void Update()
    {
        if (RunnerControl.MovingAllowed()) //only checks if object is safe when 'moving_allowed' is true because otherwise all alive objects will be safe anyway
        {
            safe = Safe();
            if (!safe) 
            {
                moving = true;       
            }
            else //if object has reached the safe boundary
            {
                moving = false; 
                times_crossed++;
                RunnerControl.IncrementSafe(); //tells 'RunnerControl' that a runner object is safe 
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
