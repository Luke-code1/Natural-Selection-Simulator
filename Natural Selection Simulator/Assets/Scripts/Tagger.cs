using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : Entity
{

    private TaggerControl TaggerControl;

    private int runners_tagged;

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

    new private Vector3 LocateClosestEnemy(List<GameObject> EnemyList)
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
        Vector3 ClosestEnemy = LocateClosestEnemy(TaggerControl.EnemyList);
        Vector3 NormalizedRelativePosition = (ClosestEnemy - body.position).normalized;
        //Debug.Log("Closest runner: " + ClosestEnemy);
        return speed*(NormalizedRelativePosition);
    }

    void Start()
    {
        self = this.gameObject;
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        runners_tagged = 0;

        speed = speed + Random.Range(-speed * TaggerControl.variance, speed * TaggerControl.variance);
        size = size + Random.Range(-size * TaggerControl.variance, size * TaggerControl.variance);
        efficiency = efficiency + Random.Range(-efficiency * TaggerControl.variance, efficiency * TaggerControl.variance);

        TaggerControl.TypeList.Add(gameObject);
    }


    void Update()
    {
        //Debug.Log(TaggerControl.SimulationControl.SimulationActive());
    }

    private void FixedUpdate()
    {
        if (TaggerControl.SimulationControl.SimulationActive())
        {
            body.velocity = CalculateVelocityVector();
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
        if (collision.gameObject.tag == "Runner")
        {
            //Debug.Log("runner-tagger collision");
            if (!collision.gameObject.GetComponent<Runner>().SafeThisGeneration())
            {
                runners_tagged++;
                collision.gameObject.GetComponent<Runner>().DestroySelf(TaggerControl.SimulationControl.GetRunnerControl().TypeList);
            }
        }
    }

}
