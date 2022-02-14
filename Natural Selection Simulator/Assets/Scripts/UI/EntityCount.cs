using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EntityCount : MonoBehaviour
{
    private RunnerControl RunnerControl;
    private TaggerControl TaggerControl;

    private Text runner_no;
    private Text tagger_no;

    // Start is called before the first frame update
    void Start()
    {
        RunnerControl = GameObject.Find("Control").GetComponent<RunnerControl>();
        TaggerControl = GameObject.Find("Control").GetComponent<TaggerControl>();
        runner_no = GameObject.Find("Runner_no").GetComponent<Text>();
        tagger_no = GameObject.Find("Tagger_no").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        runner_no.text = "Runner: " + RunnerControl.TypeList.Count;
        tagger_no.text = "Tagger: " + TaggerControl.TypeList.Count;
    }
}
