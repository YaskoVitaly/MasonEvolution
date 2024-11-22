using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchSystem : MonoBehaviour
{
    private MetaUI metaUI;
    private GlobalData globalData;
    
    public List<BasicResearchData> basicResearches;

    //public Dictionary<string, BasicResearchData> basicResearches = new Dictionary<string, BasicResearchData>();
    public int availableResearchSlots = 1;
    private int activeResearches = 0;

    public void Init(MetaUI _metaUI, GlobalData _globalData)
    {
        metaUI = _metaUI;
        globalData = _globalData;
    }

    public void StartResearch(BasicResearchData currentResearch)
    {
        if(globalData.money >= currentResearch.costCurrency && globalData.totalExperience >= currentResearch.costExperience)
        {
            StartCoroutine(ResearchProcess(currentResearch));
        }
    }

    /*
    public void StartResearch(string researchName, int playerCurrency, int playerExperience)
    {
        if (activeResearches >= availableResearchSlots)
        {
            Debug.Log("No available slots");
            return;
        }
        if(basicResearches.TryGetValue(researchName, out BasicResearchData basicResearchData))
        {
            if(basicResearchData.level >= basicResearchData.maxLevel)
            {
                Debug.Log($"{basicResearchData.name} is alredy at max level!");
                return;
            }

            int costCurrency = CalculateCostMoney(basicResearchData.costCurrency, basicResearchData.level);
            int costExperience = CalculateCostExperience(basicResearchData.costExperience, basicResearchData.level);

            if(playerCurrency >= costCurrency && playerExperience >= costExperience)
            {
                activeResearches++;
                
            }
        }
    }
    */
    private int CalculateCostMoney(int baseCost, int level) => baseCost * level;
    private int CalculateCostExperience(int baseCost, int level) => baseCost * level;
    private float CalculateTime(float baseTime, int level) => baseTime * level;

    private IEnumerator ResearchProcess(BasicResearchData basicResearchData)
    {
        Debug.Log($"Research '{basicResearchData.name}' started...");
        float timeRequired = CalculateTime(basicResearchData.time, basicResearchData.level);
        yield return new WaitForSeconds(timeRequired);

        basicResearchData.level++;
        activeResearches--;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
