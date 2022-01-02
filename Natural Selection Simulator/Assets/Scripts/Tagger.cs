using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : Entity
{

    private TaggerControl TaggerControl;

    private Vector3 CalculateVelocityVector()
    {
        Vector3 ClosestEnemy = LocateClosestEnemy(TaggerControl.EnemyList);
        Debug.Log("Closest runner: " + ClosestEnemy);
        return Vector3.zero;
    }

    void Start()
    {
        self = this.gameObject;
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        speed = 70.0f; //filler value //speed + Random.Range(-speed * RunnerControl.variance, speed * RunnerControl.variance);
        size = size + Random.Range(-size * TaggerControl.variance, size * TaggerControl.variance);
        efficiency = efficiency + Random.Range(-efficiency * TaggerControl.variance, efficiency * TaggerControl.variance);

        TaggerControl.TypeList.Add(self);
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
        }
        else
        {
            body.velocity = Vector3.zero;
        }
    }

}
