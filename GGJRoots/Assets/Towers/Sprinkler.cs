using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sprinkler : MonoBehaviour
{
    private int _health = 60;

    private int _maxHP;

    // Start is called before the first frame update
    void Start()
    {

        _maxHP = _health;

        InvokeRepeating("Decay", 1.0f, 1.0f);

    }
        

    // Update is called once per frame
    void Update()
    {
        
        if(_health <= 0) {
            Destroy(this);
        }

    }

    void Decay() 
    {

        _health--;

    }

    public void Repair() 
    {

        _health = _maxHP;

    }

    void OnCollisionEnter(Collision collision) {

        collision.collider.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell = 0.9f; //*Assumes enemies will have a speed parameter

        Debug.Log("colldiing");

    }

    void OnCollisionExit(Collision collision) {

        collision.collider.gameObject.GetComponent<EnemyMovement>().waitBeforeMoveToNextCell = 0.6f;

    }

}
