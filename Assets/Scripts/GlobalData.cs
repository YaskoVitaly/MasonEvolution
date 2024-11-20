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

    public Dictionary<string, int> resources = new Dictionary<string, int>(); // Материалы и их количество
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // Уровни улучшений

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
