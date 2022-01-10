using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : Entity
{

    private TaggerControl TaggerControl;

    private int runners_tagged;
    private int generations_not_tagged;

    public void RecieveAttributes(float[] attributes)
    {
        //Debug.Log("speed: " + attributes[0] + "size: " + attributes[1] + "efficiency" + attributes[2]);
        speed = attributes[0];
        size = attributes[1];
        efficiency = attributes[2];
    }

    private void Reproduce()
    {
        GameObject child = Instantiate(self, body.position, Quaternion.identity);
        Tagger child_script = child.GetComponent<Tagger>();
        child_script.RecieveAttributes(new float[] { speed, size, efficiency });

        Debug.Log("Copy created.");
    }

    private void Reposition() { body.position = new Vector3(0, 1.5f, Random.Range(-48, 48)); }

    new private Vector3 LocateClosestEnemy(ref List<GameObject> EnemyList)
    {
        Vector3 ClosestEnemyPosition = body.position; //sets closest enemy to itself for the case where no enemy is located
        float closest_sqr_distance = Mathf.Infinity; //the first enemy comparison will always be closer

        foreach (GameObject enemy in EnemyList) 
        {
            if (!enemy.GetComponent<Runner>().SafeThisGeneration()) //only checks runner position is it is not safe
            {
                Vector3 enemy_position = enemy.GetComponent<Rigidbody>().position;
                float current_sqr_distance = (enemy_position - body.position).sqrMagnitude;
                //square of distance between self and enemy being tested
                if (current_sqr_distance < closest_sqr_distance)
                {
                    closest_sqr_distance = current_sqr_distance;
                    ClosestEnemyPosition = enemy_position;
                }
            }

        }
        return ClosestEnemyPosition;
    }

    private Vector3 CalculateVelocityVector()
    {
        Vector3 ClosestEnemy = LocateClosestEnemy(ref TaggerControl.EnemyList); 
        Vector3 NormalizedRelativePosition = (ClosestEnemy - body.position).normalized; //relative position vector: tagger to runner
        //Debug.Log("Closest runner: " + ClosestEnemy);
        return speed*(NormalizedRelativePosition);
    }

    void Start()
    {
        self = this.gameObject;
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        energy = TaggerControl.StartingEnergy();
        runners_tagged = 0;
        generations_not_tagged = 0;

        speed = speed + Random.Range(-speed * TaggerControl.variance, speed * TaggerControl.variance);
        size = size + Random.Range(-size * TaggerControl.variance, size * TaggerControl.variance);
        efficiency = efficiency + Random.Range(-efficiency * TaggerControl.variance, efficiency * TaggerControl.variance);

        TaggerControl.TypeList.Add(gameObject);
    }


    void Update()
    {
        if (TaggerControl.SimulationControl.SimulationActive())
        {
            energy -= (Time.deltaTime * size * speed * speed) / efficiency;
        } //energy depletes at constant rate while the simulation is active
        else
        {
            energy = TaggerControl.StartingEnergy(); //energy restored to starting value when generation is over
            if (!safe_this_generation) //'safe_this_generation' refers to whether a tagger has tagged a runner this generation
            { 
                generations_not_tagged++; 
            }
            else
            {
                if (runners_tagged >= TaggerControl.TagToReproduce()) //if tagger tags enough runners then it will produce a copy
                { 
                    Reproduce();
                    runners_tagged = 0;
                }
            }
            if (generations_not_tagged > TaggerControl.NoTagLimit()) //if tagger goes too many generations without tagging a runner iot is destroyed
            {
                DestroySelf(TaggerControl.TypeList); 
            }
            safe_this_generation = true; //set to true so 'generations_not_tagged' is only incremented once if false
            /*if (TaggerControl.SimulationControl.SimulationActive())
            {
                safe_this_generation = false;
            }*/
        }
    }

    private void FixedUpdate()
    {
        if (TaggerControl.SimulationControl.SimulationActive())
        {
            if (energy > 0) { body.velocity = CalculateVelocityVector(); } //cannot move unless energy is positive
            else { body.velocity = Vector3.zero; }
            //Debug.Log(name + " absolute speed: " + body.velocity.magnitude);
        }
        else
        {
            Reposition();
            body.velocity = Vector3.zero;            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Runner") //all runner objects have the tag "Runner"
        {
            //Debug.Log("runner-tagger collision");

            if (!collision.gameObject.GetComponent<Runner>().SafeThisGeneration()) //prevent accidental tagging of safe runner objects
            {
                safe_this_generation = true;
                runners_tagged++;
                collision.gameObject.GetComponent<Runner>().DestroySelf(TaggerControl.SimulationControl.GetRunnerControl().TypeList);
            }
        }
    }

}
