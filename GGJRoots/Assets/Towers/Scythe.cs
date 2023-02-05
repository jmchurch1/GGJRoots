using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scythe : MonoBehaviour
{
    private int _health = 60;
    private int _maxHP;
    private bool _canAttack = true;
    private BoxCollider2D _AOE;
    [SerializeField] private int dmgValue = 30;

    // Start is called before the first frame update
    void Awake() 
    {
       
    }
    void Start()
    {

        _maxHP = _health;

        _AOE = GetComponent<BoxCollider2D>();

        
    }

    // Update is called once per frame
    void Update()
    {

        Attack();

        if(_health <= 0) {
            Destroy(this);
        }

        StartCoroutine(Decay());
    }

    void Attack()
    {
        if(_canAttack) {

        _AOE.enabled = true;
        StartCoroutine(attackDelay());

        //Debug.Log("Attacked");

        }

    }

    IEnumerator Decay() 
    {

        _health--;

        yield return new WaitForSeconds(1.0f);

        Debug.Log("decay");

    }

    IEnumerator attackDelay() 
    {
        _canAttack = false;
        yield return new WaitForSeconds(4.0f);
        _AOE.enabled = false;
        _canAttack = true;

    }

    public void repair() 
    {

        _health = _maxHP;

    }

    void OnCollisionStay(Collision collision) {

        collision.collider.gameObject.GetComponent<PlayerMovement>().health = collision.collider.gameObject.GetComponent<PlayerMovement>().health - dmgValue; //*Assumes enemies will have a speed parameter

        Debug.Log("colldiing");

    }

}
