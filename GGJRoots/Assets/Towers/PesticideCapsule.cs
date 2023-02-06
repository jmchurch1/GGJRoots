using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesticideCapsule : MonoBehaviour
{
    private int _health = 20;
    private int _maxHP;
    private GameObject enemy;
    private bool _canDamage = false;
    [SerializeField] public int dmgValue = 5;
    // Start is called before the first frame update
    void Start()
    {
        _maxHP = _health;


        InvokeRepeating("Decay", 1.0f, 1.0f);

        InvokeRepeating("DPS", 0.0f, 1.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        if(_health <= 0) {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerStay2D(Collider2D collision) { 

        _canDamage = true;

        enemy = collision.gameObject;

        //Debug.Log("colldiing");


    }

    void DPS() { 

        if(_canDamage && enemy.GetComponent<EnemyMovement>() != null) {

            if(enemy.GetComponent<Ant>() != null) {

                enemy.GetComponent<Ant>().health = enemy.GetComponent<Ant>().health - dmgValue;

            } else if(enemy.GetComponent<Worm>() != null) {

                enemy.GetComponent<Worm>().health = enemy.GetComponent<Worm>().health - dmgValue;

            } else if(enemy.GetComponent<Mole>() != null) {

                enemy.GetComponent<Mole>().health = enemy.GetComponent<Mole>().health - dmgValue;
                
            }
        }
        
    }

    public void Repair() 
    {

        _health = _maxHP;

    }

    void Decay() 
    {

        _health--;

    }
}
