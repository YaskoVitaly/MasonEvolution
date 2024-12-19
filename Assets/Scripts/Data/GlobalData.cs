using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalData
{
    [Serializable]
    public class KeyValue
    {
        public string key;
        public int value;
    }
    public List<KeyValue> researchDataList = new List<KeyValue>();

    public string globalTime;
    public float timePeriod;

    public float totalExperience;
    public int money;
    public float contractCooldown;
    public float nextContractTime;
    public int maxContracts;
    public ContractData currentContract;

    public PlayerData playerData;
    public List<ContractData> activeContracts = new List<ContractData>();
    public List<ContractData> possibleContracts = new List<ContractData>();
    public List<ResearchData> allResearches = new List<ResearchData>();

    public int activeResearchSlots;

    public ResearchData activeResearch;
    public float activeResearchTime;

    public Dictionary<string, int> researchLevels = new Dictionary<string, int>();//Переработать в список исследований. Словарь не сохраняется.
    
    public GlobalData()
    {
        totalExperience = 0;
        money = 0;
        contractCooldown = 600;
        nextContractTime = 0;
        maxContracts = 3;
        activeResearchSlots = 1;

        researchLevels["EnergyLimit"] = 0;
        researchLevels["EnergyRegeneration"] = 0;
        researchLevels["EnergySpend"] = 0;
        researchLevels["ForceProduction"] = 0;
        researchLevels["ForceGeneration"] = 0;
        researchLevels["ForceSpend"] = 0;
        researchLevels["ProductionSpeed"] = 0;
        researchLevels["ProductionCount"] = 0;
        researchLevels["ExperienceMult"] = 0;
    }

    public void ConvertDictionary()
    {
        researchDataList.Clear();
        foreach (var pair in researchLevels)
        {
            researchDataList.Add(new KeyValue { key = pair.Key, value = pair.Value });
        }
    }

    public void GetDictionary()
    {
        foreach (var kvp in researchDataList)
        {
            researchLevels[kvp.key] = kvp.value;
        }
    }
}
