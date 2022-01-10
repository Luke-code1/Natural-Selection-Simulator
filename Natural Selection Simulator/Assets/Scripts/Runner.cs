using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Entity
{

    private RunnerControl RunnerControl;
    private float boundary;

    private bool moving;
    private int times_crossed;
    [SerializeField] private float fear_coefficient;

    public void RecieveAttributes(float[] attributes)
    {
        //Debug.Log("speed: " + attributes[0] + "size: " + attributes[1] + "efficiency" + attributes[2] + "fear_coefficienct: " + attributes[3]);
        speed = attributes[0];
        size = attributes[1]; 
        efficiency = attributes[2];
        fear_coefficient = attributes[3];
    }

    private void Reproduce()
    {
        GameObject child = Instantiate(self, body.position, Quaternion.identity);
        Runner child_script = child.GetComponent<Runner>();
        child_script.RecieveAttributes(new float[] {speed, size, efficiency, fear_coefficient});

        Debug.Log("Copy created.");
    }

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

    public bool SafeThisGeneration() { return safe_this_generation; }

    private Vector3 CalculateVelocityVector()
    {
        Vector3 ClosestEnemy = LocateClosestEnemy(ref RunnerControl.EnemyList);
        //Debug.Log(name + " Closest tagger: " + ClosestEnemy);
        Vector3 NormalizedRelativePosition = (body.position - ClosestEnemy).normalized;  //relative position vecotr: tagger to runner
        Vector3 NormalizedVelocityVector = (fear_coefficient * NormalizedRelativePosition + new Vector3(RunnerControl.GetDirection()*0.7f, 0, 0)).normalized;
        return speed * NormalizedVelocityVector;
    }

    void Start()
    {
        self = this.gameObject;
        RunnerControl = GameObject.Find("Control").GetComponent<RunnerControl>();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        safe_this_generation = false;
        energy = RunnerControl.StartingEnergy();
        times_crossed = 0;
        boundary = RunnerControl.GetBoundary();

        speed = speed + Random.Range(-speed * RunnerControl.variance, speed * RunnerControl.variance);
        size = size + Random.Range(-size * RunnerControl.variance, size * RunnerControl.variance);
        efficiency = efficiency + Random.Range(-efficiency * RunnerControl.variance, efficiency * RunnerControl.variance);
        fear_coefficient = fear_coefficient + Random.Range(-fear_coefficient * RunnerControl.variance, fear_coefficient * RunnerControl.variance);

        RunnerControl.TypeList.Add(gameObject); //adds object to the list containing all runner instances
    }

    void Update()
    {
        //Debug.Log("speed: " + speed + "size: " + size);
        if (RunnerControl.MovingAllowed() && !safe_this_generation)
        { //only checks if object is safe in the cases where it is possible for an object to be not safe
            if (!Safe()) 
            {
                energy -= (Time.deltaTime * size * speed * speed) / efficiency; //depletion of energy while moving
                moving = true;
                //Debug.Log(name + " moving: " + moving);
            }
            else //if object has reached the safe boundary
            {
                safe_this_generation = true;
                moving = false;
                //Debug.Log(name + " moving: " + moving);

                RunnerControl.IncrementSafe(); //tells 'RunnerControl' that a runner object is safe 
            }
        }
        else if (!RunnerControl.MovingAllowed())
        { //if 'moving allowed' is false then 'safe_this_generation' will be true
            energy = RunnerControl.StartingEnergy(); //reset to the starting amount of energy each generation
            if (times_crossed == 2)
            {
                Reproduce();
                times_crossed = 0;
            }
        }
    }


    private void FixedUpdate()
    {
        if (moving && energy > 0)
        {
            body.velocity = CalculateVelocityVector();
            //Debug.Log("energy: " + energy);
        }
        else
        {
            body.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "SideWall1" || collision.collider.name == "SideWall2")
        {
            body.position = new Vector3(body.position.x, body.position.y, 0);
        }    
    }

}
