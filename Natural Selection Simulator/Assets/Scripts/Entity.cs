using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public Rigidbody body; //component required for movement and position
    protected GameObject self; //reference to own instance

    protected float speed;
    protected float size;
    protected float efficiency;
    protected float energy;


    public void GiveAttributes(float parent_speed, float parent_size, float parent_efficiency, Vector3 parent_position) 
    {
        self = GameObject.Find(name);

        body.position = parent_position;
        speed = parent_speed;
        efficiency = parent_efficiency;
        size = parent_size;

    } //called in the reproduce method by parent on newly instantaited object to share attributes

    protected void Reproduce()
    {

    }


    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }


    void Update()
    {
        
    }
}
