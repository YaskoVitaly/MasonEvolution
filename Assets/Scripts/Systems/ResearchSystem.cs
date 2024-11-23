using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ResearchSystem;

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

    public List<ResearchData> allResearches;
    public List<ActiveResearch> activeResearches = new List<ActiveResearch>();
    public List<Button> researchButtons;
    public int availableResearchSlots = 1;

    public void Init(MetaUI _metaUI, GlobalData _globalData)
    {
        metaUI = _metaUI;
        globalData = _globalData;

        foreach (ResearchData researchData in allResearches)
        {
            activeResearches.Add(new ActiveResearch
            {
                research = researchData,
                currentLevel = 0,
                isCompleted = false,
            });
        }
        UpdateResearchButtons();

    }
    public void StartResearch(int researchIndex, int playerCurrency, int playerExperience)
    {
        if (availableResearchSlots <= 0)
        {
            Debug.Log("No available slots for research!");
            return;
        }

        ActiveResearch research = activeResearches[researchIndex];
        if (research.isCompleted || research.currentLevel >= research.research.levels.Count)
        {
            Debug.Log("Research is already at max level or completed!");
            return;
        }

        var levelData = research.research.levels[research.currentLevel];
        if (playerCurrency >= levelData.costCurrency && playerExperience >= levelData.costExperience)
        {
            availableResearchSlots--;
            StartCoroutine(ResearchProcess(research));
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }
    private IEnumerator ResearchProcess(ActiveResearch research)
    {
        var levelData = research.research.levels[research.currentLevel];
        Debug.Log($"Research '{research.research.researchName}' started...");
        yield return new WaitForSeconds(levelData.timeRequired);

        research.currentLevel++;
        if (research.currentLevel >= research.research.levels.Count)
        {
            research.isCompleted = true;
        }

        availableResearchSlots++;
        //UpdateResearchButtons();

        Debug.Log($"Research '{research.research.researchName}' completed! New Level: {research.currentLevel}");
    }
    private void UpdateResearchButtons()
    {
        for (int i = 0; i < researchButtons.Count; i++)
        {
            if (i < activeResearches.Count)
            {
                ActiveResearch research = activeResearches[i];
                var levelData = research.currentLevel < research.research.levels.Count
                    ? research.research.levels[research.currentLevel]
                    : null;

                Button button = researchButtons[i];
                if (levelData != null)
                {
                    button.GetComponentInChildren<Text>().text = $"{research.research.researchName} - Level {research.currentLevel}\nCost: {levelData.costCurrency}\nTime: {levelData.timeRequired}s";
                    button.interactable = !research.isCompleted;
                    int index = i; // Локальная копия индекса
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => StartResearch(index, /*playerCurrency=*/1000, /*playerExperience=*/500));
                }
                else
                {
                    button.GetComponentInChildren<Text>().text = $"{research.research.researchName} - Max Level";
                    button.interactable = false;
                }
            }
        }
    }

}
