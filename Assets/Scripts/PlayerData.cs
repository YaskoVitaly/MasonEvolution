using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float energyMaxBasic;
    public float energyRegBasic;
    public float energySpendBasic;
    public float forceProdBasic;
    public float forceRegBasic;
    public float forceTimeBasic;
    public float forceSpendBasic;
    public float workCostBasic;
    public float experienceMultBasic;
    public float productionTimeBasic;
    public int productionCountBasic;


    public float energyMax;
    public float energyCur;
    public float energyReg;
    public float energySpend;
    public float forceProd;
    public float forceCur;
    public float forceReg;
    public float forceTime;
    public float forceSpend;
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
        energyMaxBasic = 10;
        energyRegBasic = 1;
        energySpendBasic = 1;
        forceProdBasic = 1;
        forceRegBasic = 0;
        forceTimeBasic = 5;
        forceSpendBasic = 1;
        workCostBasic = 1;
        experienceMultBasic = 0.1f;
        productionTimeBasic = 10;
        productionCountBasic = 1;

        energyLimitUpgradeLevel = 0;
        energyRegenerationUpgradeLevel = 0;
        energySpendUpgradeLevel = 0;
        forceProductionUpgradeLevel = 0;
        forceGenerationUpgradeLevel = 0;
        forceSpendUpgradeLevel = 0;
        productionTimeUpgradeLevel = 0;
        productionCountUpgradeLevel = 0;
        expIncomeUpgradeLevel = 0;

        energyMax = energyMaxBasic * (energyLimitUpgradeLevel + 1);
        energyCur = energyMax;
        energyReg = energyRegBasic += energyRegenerationUpgradeLevel;
        energySpend = energySpendBasic - energySpendUpgradeLevel / 10; //ѕроверить надо
        forceProd = forceProdBasic += forceProductionUpgradeLevel;
        forceCur = 0;
        forceReg = forceGenerationUpgradeLevel;
        forceTime = forceTimeBasic * (1 - energyRegenerationUpgradeLevel / 10); //ѕроверить надо
        forceSpend = forceSpendBasic - forceSpendUpgradeLevel / 10; //ѕроверить надо
        workCost = workCostBasic;
        expCur = 0;
        experienceMult = experienceMultBasic; //нужно доработать
        productionTime = productionTimeBasic; //нужно доработать
        productionCount = productionCountBasic + productionCountUpgradeLevel;
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
