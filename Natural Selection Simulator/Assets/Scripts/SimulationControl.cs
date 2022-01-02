using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationControl : MonoBehaviour
{

    private RunnerControl RunnerControl;
    private TaggerControl TaggerControl;

    private int runner_count;
    private float runner_speed;
    private float runner_size;
    private float runner_efficiency;
    private float runner_fear_coefficient;
    private float[] runner_attribute_arr;

    public float[] RunnerAttributes() { return runner_attribute_arr; }
    public int GetRunnerCount() { return runner_count; }

    private int tagger_count;
    private float tagger_speed;
    private float tagger_size;
    private float tagger_efficiency;
    private float[] tagger_attribute_arr;

    public float[] TaggerAttributes() { return tagger_attribute_arr; }
    public int GetTaggerCount() { return tagger_count; }

    public RunnerControl GetRunnerControl() { return RunnerControl; }
    public TaggerControl GetTaggerControl() { return TaggerControl; }

    public bool SimulationActive() { return RunnerControl.MovingAllowed(); } //returns true when 'moving_allowed' is true

    void Start()
    {
        RunnerControl = GameObject.Find("Control").GetComponent<RunnerControl>();
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();

        runner_count = 1; tagger_count = 1; //lines 40 to 48 to be implemented in Version4
        runner_speed = 50.0f; tagger_speed = 50.0f;
        runner_size = 5.0f; tagger_size = 5.0f;
        runner_efficiency = 0.7f; tagger_efficiency = 0.7f;
        runner_fear_coefficient = 0.3f;

        runner_attribute_arr = new float[]{ runner_speed, runner_size, runner_efficiency, runner_fear_coefficient };
        tagger_attribute_arr = new float[] { tagger_speed, tagger_size, tagger_efficiency };

    }


    void Update()
    {
        
    }
}
