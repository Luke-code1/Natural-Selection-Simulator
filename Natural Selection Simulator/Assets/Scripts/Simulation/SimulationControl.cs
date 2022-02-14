using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationControl : MonoBehaviour
{

    public class AttributeArray //structure for item in list containing simulation data
    {
        private float[] attribute_array; //data
        public AttributeArray(float[] attributes) //consctructor
        {
            attribute_array = attributes;
        }
        public float GetItem(int index) { return attribute_array[index]; }
        public void SetItem(int index, float value) { attribute_array[index] = value; }       
    }

    private AttributeArray MeanEntity(List<AttributeArray> array, int max_index) //returns AttributeArray describing the mean runner/ tagger that generation
    {
        AttributeArray mean_entity = new AttributeArray(new float[] { 0, 0, 0, 0 });
        int length = array.Count;
        for (int i = 0; i <= max_index; i++)
        {
            float total = 0;
            foreach (AttributeArray entity in array)
            {
                total += entity.GetItem(i); //sum of particular attribute from a generation
            }
            mean_entity.SetItem(i, total / length); //sets mean_entity[i] to the mean of that attribute
        }
        return mean_entity;
    }

    private List<AttributeArray> MeanRunnerData = new List<AttributeArray>(); //contains mean runner/ tagger of each generation
    private List<AttributeArray> MeanTaggerData = new List<AttributeArray>();

    private List<AttributeArray> GenerationRunnerData = new List<AttributeArray>(); //contains all runners/ taggers of a given generation
    private List<AttributeArray> GenerationTaggerData = new List<AttributeArray>(); //emptied each generation

    public void AppendGenerationRunnerData(float[] attributes) { GenerationRunnerData.Add(new AttributeArray(attributes)); }
    public void AppendGenerationTaggerData(float[] attributes) { GenerationTaggerData.Add(new AttributeArray(attributes)); }

    public bool t_tag { get; set; } //tag that tells script when to append MeanEntity(GenerationTaggerData) to MeanTaggerData
    public bool r_tag { get; set; } //tag that tells script when to append MeanEntity(GenerationRunnerData) to MeanRunnerData

    private RunnerControl RunnerControl;
    private TaggerControl TaggerControl;

    private int runner_count; //number of runners in first generation
    private float runner_speed; //first generation runner attributes
    private float runner_size;
    private float runner_efficiency;
    private float runner_fear_coefficient;
    private float[] runner_attribute_arr;

    public float[] RunnerAttributes() { return runner_attribute_arr; }
    public int GetRunnerCount() { return runner_count; }

    private int tagger_count; //number of taggers in first generation
    private float tagger_speed; //first generation tagger attributes
    private float tagger_size;
    private float tagger_efficiency;
    private float[] tagger_attribute_arr;

    public float[] TaggerAttributes() { return tagger_attribute_arr; }
    public int GetTaggerCount() { return tagger_count; }

    public RunnerControl GetRunnerControl() { return RunnerControl; }
    public TaggerControl GetTaggerControl() { return TaggerControl; }

    public bool SimulationActive() { return RunnerControl.MovingAllowed(); } //returns true when 'moving_allowed' is true

    void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("InputManager"));
        Parameters parameters = GameObject.Find("InputManager").GetComponent<Parameters>();

        RunnerControl = GameObject.Find("Control").GetComponent<RunnerControl>();
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();

        runner_count = (int)parameters.R_attributes()[0]; ; tagger_count = (int)parameters.T_attributes()[0];
        runner_speed = parameters.R_attributes()[2]; tagger_speed = parameters.T_attributes()[2];
        runner_size = parameters.R_attributes()[3]; tagger_size = parameters.T_attributes()[3];
        runner_efficiency = parameters.R_attributes()[4]; tagger_efficiency = parameters.T_attributes()[4];
        runner_fear_coefficient = parameters.R_attributes()[5];

        RunnerControl.variance = parameters.R_attributes()[1];
        TaggerControl.variance = parameters.T_attributes()[1];

        /*runner_count = 20; ; tagger_count = 20;
        runner_speed = 50; tagger_speed = 45;
        runner_size = 10; tagger_size = 10;
        runner_efficiency = 0.9f; tagger_efficiency = 0.9f;
        runner_fear_coefficient = 0.2f; */

        runner_attribute_arr = new float[] { runner_speed, runner_size, runner_efficiency, runner_fear_coefficient };
        tagger_attribute_arr = new float[] { tagger_speed, tagger_size, tagger_efficiency };

    }

    void Start()
    {

    }

    void Update()
    {
        if(r_tag)
        {
            MeanRunnerData.Add(MeanEntity(GenerationRunnerData, 3));
            GenerationRunnerData = new List<AttributeArray>();
            r_tag = false;
            //Debug.Log("Runner data count: " + MeanRunnerData.Count);           
            DisplayAttributeArrayList(MeanRunnerData, 3, true);
        }
        if (t_tag)
        {
            MeanTaggerData.Add(MeanEntity(GenerationTaggerData, 2));
            GenerationTaggerData = new List<AttributeArray>();
            t_tag = false;
            //Debug.Log("Tagger data count: " + MeanTaggerData.Count);
            DisplayAttributeArrayList(MeanTaggerData, 2, false);
        }
    }

    public void DisplayAttributeArrayList(List<AttributeArray> list, int max_index, bool runner)
    { //purely for debugging purposes
        if (runner) { Debug.Log("Runner:"); }
        else { Debug.Log("Tagger"); }
        foreach (AttributeArray arr in list)
        {
            for (int i = 0; i <= max_index; i++)
            {
                Debug.Log(i + ": " + arr.GetItem(i));
            }
        }
    }
}
