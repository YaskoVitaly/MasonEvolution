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
    public int contractsQueue;
    public List<ContractData> activeContracts = new List<ContractData>();
    public List<ContractData> possibleContracts = new List<ContractData>();

    public int energyLimitResearchLevel = 2;
    public int energyRegenResearchLevel;
    public int energySpendResearchLevel;
    public int forceProductionResearchLevel;
    public int forceAutogenResearchLevel;
    public int forceSpendResearchLevel;
    public int productionSpeedResearchLevel;
    public int productionCountResearchLevel;
    public int experienceMultResearchLevel;


    public Dictionary<string, int> resources = new Dictionary<string, int>(); // Материалы и их количество
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Уровни улучшений
    
}
