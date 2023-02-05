using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesticideCapsule : MonoBehaviour
{
    private int _health = 10;
    private int _maxHP;
    private GameObject enemy;
    private bool _canDamage = false;
    [SerializeField] public int dmgValue = 2;
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

        Debug.Log("colldiing");


    }

    void DPS() { 

        if(_canDamage && enemy.GetComponent<EnemyMovement>() != null) {
            Destroy(enemy.gameObject);
        }
        //enemy.GetComponent<EnemyMovement>().health = enemy.GetComponent<EnemyMovement>().health - dmgValue;
        
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
