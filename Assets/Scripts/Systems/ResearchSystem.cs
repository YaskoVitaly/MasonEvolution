using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchSystem : MonoBehaviour
{
    [Serializable]
    public class ActiveResearch
    {
        public ResearchData research;
        public int currentLevel;
        public bool isCompleted;
    }

    private MetaUI metaUI;
    private GlobalData globalData;


    public List<ActiveResearch> activeResearches = new List<ActiveResearch>();

    public int availableResearchSlots = 1;

    public void Init(MetaUI _metaUI, GlobalData _globalData)
    {
        metaUI = _metaUI;
        globalData = _globalData;


        if (activeResearches.Count == 0)
        {
            foreach (var researchData in globalData.allResearches)
            {
                Debug.Log("research" + researchData.name);
                activeResearches.Add(new ActiveResearch
                {
                    research = researchData,
                    currentLevel = 0,
                    isCompleted = false
                });
            }
        }
        metaUI.OnResearchSelected += StartResearch;
    }
    public void StartResearch(int researchIndex)
    {
        if (availableResearchSlots <= 0)
        {
            Debug.Log("No available slots for research!");
        }
        else
        {
            ActiveResearch research = activeResearches[researchIndex];

            if (research.isCompleted || research.currentLevel >= research.research.levels.Count)
            {
                Debug.Log("Research is already at max level or completed!");
            }
            else
            {
                ResearchData.ResearchLevel levelData = research.research.levels[research.currentLevel];
                if (globalData.money >= levelData.costCurrency && globalData.totalExperience >= levelData.costExperience)
                {
                    globalData.money -= levelData.costCurrency;
                    globalData.totalExperience -= levelData.costExperience;

                    availableResearchSlots--;
                    StartCoroutine(ResearchProcess(research));
                }
                else
                {
                    Debug.Log("Not enough resources!");
                }
            }

            
        }
    }
    private IEnumerator ResearchProcess(ActiveResearch research)
    {
        ResearchData.ResearchLevel levelData = research.research.levels[research.currentLevel];
        Debug.Log($"Research '{research.research.researchName}' started...");
        yield return new WaitForSeconds(levelData.timeRequired);

        research.currentLevel++;
        globalData.researchLevels[research.research.researchName] = research.currentLevel;

        if (research.currentLevel >= research.research.levels.Count)
        {
            research.isCompleted = true;
        }

        availableResearchSlots++;

        Debug.Log($"Research '{research.research.researchName}' completed! New Level: {research.currentLevel}");
    }
}
