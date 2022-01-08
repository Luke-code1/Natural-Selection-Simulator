using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerControl : EntityControl
{

    [SerializeField] GameObject tagger_prefab;

    void Start()
    {
        SimulationControl = GameObject.Find("Control").GetComponent<SimulationControl>();

        variance = 0.15f;

        for (int i = 0; i < SimulationControl.GetTaggerCount(); i++)
        {
            GameObject tagger = Instantiate(tagger_prefab, new Vector3(0, 1.5f, Random.Range(-48, 48)), Quaternion.identity);
            Tagger tagger_script = tagger.GetComponent<Tagger>();
            tagger_script.RecieveAttributes(SimulationControl.TaggerAttributes());
        }

    }

    void Update()
    {
        EnemyList = SimulationControl.GetRunnerControl().TypeList;
    }
}
