using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchSystem : MonoBehaviour
{
    public Action OnResearchStarted;
    public Action<ResearchData> OnResearchUpdated;
    public Action<ResearchData, float> OnResearchProcessed;

    private GlobalData globalData;
    private MetaUI metaUI;

    public List<ResearchData> activeResearches = new List<ResearchData>();

    private float researchTimer;
    public int availableResearchSlots;

    public void Init(GlobalData _globalData, MetaUI _metaUI)
    {
        globalData = _globalData;
        metaUI = _metaUI;

        availableResearchSlots = globalData.activeResearchSlots;

        if (activeResearches.Count == 0)
        {
            foreach (ResearchData researchData in globalData.allResearches)
            {
                Debug.Log("research" + researchData.name);
                if(globalData.researchLevels.ContainsKey(researchData.researchName))
                {
                    researchData.currentLevel = globalData.researchLevels[researchData.researchName];
                }
                activeResearches.Add(researchData);
            }
        }
        ResearchesUpdate();
    }

    public void ResearchesUpdate()
    {
        if(metaUI != null)
        {
            foreach (ResearchData researchData in activeResearches)
            {
                OnResearchUpdated(researchData);
            }
            if (globalData.activeResearch != null)
            {
                availableResearchSlots--;
                researchTimer = globalData.activeResearchTime;
                StartCoroutine(ResearchProcess(globalData.activeResearch));
            }
        }
    }

    public void StartResearch(string researchName)
    {
        
        if (availableResearchSlots <= 0)
        {
            Debug.Log("No available slots for research!");
        }
        else
        {
            foreach(ResearchData rd in activeResearches)
            {
                if(rd.researchName == researchName)
                {
                    if (rd.isCompleted || rd.currentLevel >= rd.levels.Count)
                    {
                        Debug.Log("Research is already at max level or completed!");
                    }
                    else
                    {
                        ResearchData.ResearchLevel levelData = rd.levels[rd.currentLevel];
                        if (globalData.money >= levelData.costCurrency && globalData.totalExperience >= levelData.costExperience)
                        {
                            globalData.money -= levelData.costCurrency;
                            globalData.totalExperience -= levelData.costExperience;

                            availableResearchSlots--;
                            researchTimer = 0;
                            if(metaUI != null)
                            {
                                OnResearchStarted();
                                OnResearchUpdated(rd);
                            }
                            globalData.activeResearch = rd;
                            StartCoroutine(ResearchProcess(rd));
                        }
                        else
                        {
                            Debug.Log("Not enough resources!");
                        }
                    }
                }
            }
        }
    }
    private IEnumerator ResearchProcess(ResearchData research)
    {
        while (researchTimer < research.levels[research.currentLevel].timeRequired)
        {
            ResearchData.ResearchLevel levelData = research.levels[research.currentLevel];
            //Debug.Log($"Research '{research.researchName}' started...");
            researchTimer++;
            globalData.activeResearchTime = researchTimer;
            if(metaUI != null)
            {
                OnResearchProcessed(research, researchTimer);
            }
            yield return new WaitForSeconds(1);
        }
        research.currentLevel++;
        globalData.researchLevels[research.researchName] = research.currentLevel;
        globalData.activeResearch = null;

        if (research.currentLevel >= research.levels.Count)
        {
            research.isCompleted = true;
        }
        if(metaUI != null)
        {
            OnResearchUpdated(research);
        }
        
        availableResearchSlots++;
        Debug.Log($"Research '{research.researchName}' completed! New Level: {research.currentLevel}");
    }
}
