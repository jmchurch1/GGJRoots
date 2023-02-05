using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesticideCapsule : MonoBehaviour
{
    private int _health = 60;
    private int _maxHP;
    private BoxCollider2D _AOE;
    [SerializeField] public int dmgValue = 2;
    // Start is called before the first frame update
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

    void OnCollisionStay(Collision collision) { 

        StartCoroutine(DPS(collision.gameObject));

    }

    IEnumerator DPS(GameObject enemy) { 

        yield return new WaitForSeconds(1.0f);
        //enemy.health = enemy.health - dmgValue;
        
    }

    IEnumerator Decay() 
    {

        _health--;

        yield return new WaitForSeconds(1.0f);

    }
}
