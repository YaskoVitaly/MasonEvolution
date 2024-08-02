using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float energyMax;
    public float energyCur;
    public float energyReg;
    public float energySpend;
    public float forceProd;
    public float forceCur;
    public float forceReg;
    public float forceTime;
    public float workCost;
    public float expCur;
    public float experienceMult;
    public float productionTime;
    public int productionCount;
    public float expTotal;

    public int completedObjects;

    public int energyLimitUpgradeLevel;
    public int energyRegenerationUpgradeLevel;
    public int energySpendUpgradeLevel;
    public int forceProductionUpgradeLevel;
    public int forceGenerationUpgradeLevel;
    public int forceSpendUpgradeLevel;
    public int productionTimeUpgradeLevel;
    public int productionCountUpgradeLevel;
    public int expIncomeUpgradeLevel;

    public PlayerData()
    {
        energyMax = 10;
        energyCur = energyMax;
        energyReg = 1;
        energySpend = 1;
        forceProd = 1;
        forceCur = 0;
        forceReg = 0;
        forceTime = 5;
        workCost = 1;
        expCur = 0;
        experienceMult = 1;
        productionTime = 10;
        productionCount = 1;
        expTotal = 0;
        completedObjects = 0;
    }

    /*//ƒоработать при необходимости!
    public float EnergyMax { get { return energyMax + (energyMax * energyLimitUpgradeLevel); } } //Ќужен баланс
    public float EnergyCur { get { return energyCur; } } //возможно нет необходимости в хранении. 
    public float EnergyReg { get { return energyReg + energyRegenerationUpgradeLevel;} }
    public float EnergySpend { get { return energySpend * (1 - energySpendUpgradeLevel/10);} } //доработать. необходимо снижение текущего показател€ на 10% при каждом повышении уровн€
    public float ForceProd {  get { return forceProd + forceProductionUpgradeLevel; } }
    public float ForceCur { get { return forceCur; } }
    public float ForceReg { get { return forceReg + forceGenerationUpgradeLevel; } } //Check
    public float ForceTime { get { return forceTime * (1 - forceGenerationUpgradeLevel/10); } }//доработать. необходимо снижение текущего показател€ на 10% при каждом повышении уровн€
    public float WorkCost { get { return workCost; } }
    */


}
