using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpotType
{
    Dirt,
    NoDirt,
    Grass,
    Sky,
    Goal
}
public class GridSpot
{
    // 0: dirt, 1: no dirt, 2: grass, 3: sky, 4: Goal
    private SpotType _spotType;
    private float _health;
    private bool _hasTower = false;
    
    public GridSpot(SpotType spotType)
    {
        _spotType = spotType;
    }

    public SpotType GetSpotType()
    {
        return _spotType;
    }

    public void SetSpotType(SpotType spotType)
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

    public bool GetTowerStatus() 
    { 
        return _hasTower; 
    }

    public void SetTowerStatus(bool t) 
    { 
        _hasTower = t; 
        
    }

    public bool IsUnblocked() {
        // is this GridSpot able to be passed through? (used for pathfinding)
        return _spotType == SpotType.NoDirt;
    }
}
