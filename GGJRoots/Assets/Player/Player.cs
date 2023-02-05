using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    UIManager _UI;

    private int worms = 2;
    private int ants = 2;
    private int moles = 0;

    private float _health = 30f;
    private float _maxHealth = 30f;
    private bool _dead = false;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerMovement>().OnPlayerDigEvent += OnPlayerDig;
        _UI = UIManager.instance;
        _UI.SetWormAmount(worms);
        _UI.SetAntAmount(ants);
        _UI.SetMoleAmount(moles);
        audioSource = GetComponent<AudioSource>();
    }


    void OnPlayerDig(GridMap grid, Vector2Int currentCell, Vector2Int destinationCell) {
            grid.SetCell(destinationCell.x, destinationCell.y, new GridSpot(SpotType.NoDirt));
            
            if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        }

    void CheckDeath()
    {
        if (_health <= 0 && !_dead)
        {
            _dead = true;
            StartCoroutine(GetComponent<PlayerMovement>().Respawn());
        }
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        _dead = false;
        GetComponent<PlayerMovement>().SetDead(_dead);
    }

    // debugging
    public void Kill()
    {
        _health = 0;
        CheckDeath();
    }


    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Worm"))
        {
            _health -= 1;
        }
        else if (c.CompareTag("Ant"))
        {
            _health -= 2;
        }
        else if (c.CompareTag("Mole"))
        {
            _health -= 3;
        }
        _UI.SetPlayerSlider(_health / _maxHealth);
        CheckDeath();
    }
}
