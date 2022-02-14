using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControl : MonoBehaviour
{

    public List<GameObject> TypeList = new List<GameObject>();
    public List<GameObject> EnemyList = new List<GameObject>();

    public SimulationControl SimulationControl; //defined in 'RunnerControl' and 'TaggerControl'
    protected float starting_energy;
    public float StartingEnergy() { return starting_energy; }

    public float variance { get; set; }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
