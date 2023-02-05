using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scythe : MonoBehaviour
{
    private int _health = 60;
    private int _maxHP;
    private bool _canAttack = true;
    [SerializeField] private int dmgValue = 30;

    // Start is called before the first frame update
    void Awake() 
    {
       
    }
    void Start()
    {

        _maxHP = _health;

        InvokeRepeating("Decay", 1.0f, 1.0f);

        
    }



    // Update is called once per frame
    void Update()
    {

        StartCoroutine(Attack());

        if(_health <= 0) {
            Destroy(this);
        }

    }

    void Decay() 
    {

        _health--;

    }

    IEnumerator Attack() 
    {
        _canAttack = false;
        yield return new WaitForSeconds(4.0f);
        _canAttack = true;

    }

    public void Repair()
    {

        _health = _maxHP;

    }

}
