using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerControl : EntityControl
{

    [SerializeField] GameObject tagger_prefab;

    private int no_tag_limit;
    private int tag_to_reproduce;

    public int NoTagLimit() { return no_tag_limit; }
    public int TagToReproduce() { return tag_to_reproduce; }

    void Start()
    {
        SimulationControl = GameObject.Find("Control").GetComponent<SimulationControl>();

        variance = 0.15f;
        no_tag_limit = 2; //filler value
        tag_to_reproduce = 2; //filler value 
        starting_energy = 70000.0f; //filler value

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
