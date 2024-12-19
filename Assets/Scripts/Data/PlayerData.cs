using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ResearchData;

[Serializable]
public class PlayerData
{
    [Serializable]
    public class KeyValue
    {
        public string key;
        public int value;
    }
    public List<KeyValue> upgradeDataList = new List<KeyValue>();

    public float energyMaxBasic;
    public float energyRegBasic;
    public float energySpendBasic;
    public float forceProdBasic;
    public float forceRegBasic;
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
    public float forceSpend;
    public float workCost;
    public float expCur;
    public float experienceMult;
    public float productionTime;
    public int productionCount;

    public float expTotal;
    public int completedObjects;
    public ContractData currentContract;
    
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();
    

    public PlayerData()
    {
        energyMaxBasic = 10;
        energyRegBasic = 1;
        energySpendBasic = 1;
        forceProdBasic = 1;
        forceRegBasic = 2;
        forceSpendBasic = 1;
        workCostBasic = 1;
        experienceMultBasic = 0.1f;
        productionTimeBasic = 10;
        productionCountBasic = 1;

        upgradeLevels["EnergyLimit"] = 0;
        upgradeLevels["EnergyRegeneration"] = 0;
        upgradeLevels["EnergySpend"] = 0;
        upgradeLevels["ForceProduction"] = 0;
        upgradeLevels["ForceGeneration"] = 0;
        upgradeLevels["ForceSpend"] = 0;
        upgradeLevels["ProductionSpeed"] = 0;
        upgradeLevels["ProductionCount"] = 0;
        upgradeLevels["ExperienceMult"] = 0;
        

        energyMax = energyMaxBasic * (upgradeLevels["EnergyLimit"] + 1);
        energyCur = energyMax;
        energyReg = energyRegBasic += upgradeLevels["EnergyRegeneration"];
        energySpend = energySpendBasic - upgradeLevels["EnergySpend"] / 10; //Проверить надо
        forceProd = forceProdBasic += upgradeLevels["ForceProduction"];
        forceCur = 0;
        forceReg = upgradeLevels["ForceGeneration"];
        forceSpend = forceSpendBasic - upgradeLevels["ForceSpend"] / 10; //Проверить надо
        workCost = workCostBasic;
        expCur = 0;
        experienceMult = experienceMultBasic; //нужно доработать
        productionTime = productionTimeBasic; //нужно доработать
        productionCount = productionCountBasic + upgradeLevels["ProductionCount"];
        expTotal = 0;
        completedObjects = 0;
    }
    public void ConvertDictionary()
    {
        upgradeDataList.Clear();
        foreach (var pair in upgradeLevels)
        {
            upgradeDataList.Add(new KeyValue { key = pair.Key, value = pair.Value });
        }
    }

    public void GetDictionary()
    {
        foreach (var kvp in upgradeDataList)
        {
            upgradeLevels[kvp.key] = kvp.value;
        }
    }
}
