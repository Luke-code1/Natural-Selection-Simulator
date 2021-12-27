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


    protected void Reproduce()
    {
        GameObject child = Instantiate(self, body.position, Quaternion.identity);
        Debug.Log("Copy created.");
    }


    void Start()
    {

    }


    void Update()
    {
        
    }
}
