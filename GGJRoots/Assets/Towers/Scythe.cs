using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scythe : MonoBehaviour
{
    private int _health = 10;
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

        InvokeRepeating("Attack", 1.0f, 4.0f);
    }



    // Update is called once per frame
    void Update()
    {

        if(_health <= 0) {
            Destroy(this.gameObject);
        }

    }

    void Decay() 
    {

        _health--;

        //Debug.Log("decay");

    }

    void Attack() 
    {
        _canAttack = true;
        StartCoroutine(attackDuration());

    }

    IEnumerator attackDuration() {

        yield return new WaitForSeconds(1.0f);
        _canAttack = false;

    }

    public void Repair()
    {

        _health = _maxHP;

    }

    void OnTriggerStay2D(Collider2D collision) {

        if(_canAttack && collision.gameObject.GetComponent<EnemyMovement>() != null)
            Destroy(collision.gameObject);
            //collision.gameObject.GetComponent<EnemyMovement>().health = collision.gameObject.GetComponent<EnemyMovement>().health;

    }

}
