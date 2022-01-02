using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected Rigidbody body; //component required for movement and position
    protected GameObject self; //reference to own instance

    protected float speed;
    protected float size;
    protected float efficiency;
    protected float energy;

    public Vector3 Position() { return body.position; }

    protected void Reproduce()
    {
        Instantiate(self, body.position, Quaternion.identity);
        Debug.Log("Copy created.");
    }
    public void DestroySelf(List<GameObject> TypeList)
    {
        TypeList.Remove(gameObject);
        Destroy(gameObject);
    }

    protected Vector3 LocateClosestEnemy(List<GameObject> EnemyList) //returns position of closest enemy
    {

        Vector3 ClosestEnemyPosition = body.position; //sets closest enemy to itself for the case where no enemy is located
        float closest_sqr_distance = Mathf.Infinity; //the first enemy comparison will always be closer

        foreach(GameObject enemy in EnemyList)
        {
            Vector3 enemy_position = enemy.GetComponent<Rigidbody>().position; 
            float current_sqr_distance = (enemy_position - body.position).sqrMagnitude;
            //square of distance between self and enemy being tested
            if (current_sqr_distance < closest_sqr_distance)
            {
                closest_sqr_distance = current_sqr_distance;
                ClosestEnemyPosition = enemy_position;
            }
        }
        return ClosestEnemyPosition;
    }


    void Start()
    {

    }


    void Update()
    {
        
    }
}
