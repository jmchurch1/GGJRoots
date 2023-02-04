using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpot
{
    // 0: dirt, 1: no dirt, 2: grass, 3: sky
    private int _spotType;
    private float _health;
    
    public GridSpot(int spotType)
    {
        _spotType = spotType;
    }

    public int GetSpotType()
    {
        return _spotType;
    }

    public void SetSpotType(int spotType)
    {
        _spotType = spotType;
    }

    public float GetSpotHealth()
    {
        return _health;
    }

    public void DecrementSpotHealth(float health)
    {
        _health -= health;
    }

    public void SetSpotHealth(float health)
    {
        _health = health;   
    }
}
