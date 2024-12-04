using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalData
{
    public float totalExperience;
    public int money;
    public float newContractTime;
    public int maxContracts;
    public List<ContractData> activeContracts = new List<ContractData>();
    public List<ContractData> possibleContracts = new List<ContractData>();
    public List<ResearchData> allResearches = new List<ResearchData>();

    public int activeResearchSlots;

    public ResearchData activeResearch;
    public float activeResearchTime;


    /*
    public int energyLimitResearchLevel;
    public int energyRegenResearchLevel;
    public int energySpendResearchLevel;
    public int forceProductionResearchLevel;
    public int forceAutogenResearchLevel;
    public int forceSpendResearchLevel;
    public int productionSpeedResearchLevel;
    public int productionCountResearchLevel;
    public int experienceMultResearchLevel;
    */

    public Dictionary<string, int> resources = new Dictionary<string, int>();
    public Dictionary<string, int> researchLevels = new Dictionary<string, int>();
    
    public GlobalData()
    {
        totalExperience = 0;
        money = 0;
        newContractTime = 600;
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
}
