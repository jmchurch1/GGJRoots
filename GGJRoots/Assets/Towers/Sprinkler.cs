using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Sprinkler : MonoBehaviour
{
    private int _health = 60;

    private int _maxHP;

    private float _time;

    private BoxCollider2D _AOE;

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
        StartCoroutine(Decay());
    }

    IEnumerator Decay() 
    {

        _health--;

        yield return new WaitForSeconds(1.0f);

    }

    public void Repair() 
    {

        _health = _maxHP;

    }

    void OnCollisionStay(Collision collision) {

        //collision.collider.gameObject.speed = 0; *Assumes enemies will have a speed parameter


    }

}
