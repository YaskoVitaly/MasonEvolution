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

        researchLevels["EnergyLimit"] = 2;
        researchLevels["EnergyRegeneration"] = 2;
        researchLevels["EnergySpend"] = 2;
        researchLevels["ForceProduction"] = 2;
        researchLevels["ForceGeneration"] = 2;
        researchLevels["ForceSpend"] = 2;
        researchLevels["ProductionSpeed"] = 2;
        researchLevels["ProductionCount"] = 2;
        researchLevels["ExperienceMult"] = 2;
    }
}
