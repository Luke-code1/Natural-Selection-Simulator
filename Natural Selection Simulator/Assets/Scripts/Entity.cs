using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected Rigidbody body; //component required for movement and position
    protected GameObject self; //reference to own instance

    [SerializeField] protected float speed;
    [SerializeField] protected float size;
    [SerializeField] protected float efficiency;
    [SerializeField] protected float energy;

    protected bool safe_this_generation;
    public void NewGeneration() { safe_this_generation = false; }

    public void DestroySelf(List<GameObject> TypeList)
    {
        TypeList.Remove(gameObject);
        Destroy(gameObject);
        //Debug.Log(name + "destroyed.");
    }

    protected Vector3 LocateClosestEnemy(ref List<GameObject> EnemyList) //returns position of closest enemy
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
