using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    private float energyMax;
    public float energyCur;
    public float energyReg;
    public float workCost;
    public float forceProduction;
    public float forceCur;
    public float forceReg;
    public float experience;
    public float productionTime;

    public float energySpend;
    public float forceTime;
    public int productionCount;
    public float experienceMult;
    public int completedObjects;

    public float EnergyMax
    {
        set { energyMax = value; }
        get { return energyMax; }

    }
}
