using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static Root instance;
    public static UIManager _UI;

    float _health = 50;
    float _maxHealth = 50;

    private void Start()
    {
        instance = this;
        _UI = UIManager.instance;
    }

    public void DecrementHealth(float health)
    {
        _health -= health;
        _UI.SetPlantSlider(_health / _maxHealth);


        if (_health <= 0)
        {
            Application.Quit();
        }
    }




}
