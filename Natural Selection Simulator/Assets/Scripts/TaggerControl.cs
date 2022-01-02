using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerControl : EntityControl
{



    void Start()
    {
        SimulationControl = GameObject.Find("Control").GetComponent<SimulationControl>();

        variance = 0.06f;
    }


    void Update()
    {
        EnemyList = SimulationControl.GetRunnerControl().TypeList;
    }
}
