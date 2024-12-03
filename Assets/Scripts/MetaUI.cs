using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    public Action<int> OnContractSelected;
    public Action<string> OnResearchSelected;

    public GlobalData globalData;
    public ResearchSystem researchSystem;

    public GameObject researchPanel;

    public Image researchFrame;

    public Button researchOpenButton;

    public Button energyLimitResearchButton;
    public Button energyRegenResearchButton;
    public Button energySpendResearchButton;
    public Button forceProductionResearchButton;
    public Button forceGenerationResearchButton;
    public Button forceSpendResearchButton;
    public Button productionTimeResearchButton;
    public Button productionCountResearchButton;
    public Button experienceIncomeResearchButton;

    public GameObject contractButtonPrefab;
    public Transform contractPanel;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI experienceText;

    public List<GameObject> contractButtons;
    
    public void Init(GlobalData _globalData, ResearchSystem _researchSystem)
    {
        globalData = _globalData;
        researchSystem = _researchSystem;
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0,0");
        researchSystem.OnResearchUpdated += UpdateBasicResearchButton;
        researchSystem.OnResearchStarted += UpdateResearchFrame;
        researchSystem.OnResearchProcessed += UpdateResearchFrame;
    }

    public void Unsubscribe()
    {
        researchSystem.OnResearchUpdated -= UpdateBasicResearchButton;
        researchSystem.OnResearchStarted -= UpdateResearchFrame;
        researchSystem.OnResearchProcessed -= UpdateResearchFrame;
    }

    private void CheckResearchButtonPrice(Button button, float expCost, float moneyCost)
    {
        if(globalData.money >= moneyCost && globalData.totalExperience >= expCost)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    private void UpdateResearchFrame(ResearchData rd, float currentTime)
    {
        researchFrame.fillAmount = currentTime / rd.levels[rd.currentLevel].timeRequired;
    }

    private void UpdateBasicResearchButton(ResearchData research)
    {
        switch (research.researchName)
        {
            case "EnergyLimit":
                CheckResearchButtonPrice(energyLimitResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                Debug.Log(energyLimitResearchButton);
                Debug.Log(research.levels[research.currentLevel].costExperience + " =? " + globalData.totalExperience);
                Debug.Log(research.levels[research.currentLevel].costCurrency + " =? " + globalData.money);

                break;
            case "EnergyRegeneration":
                CheckResearchButtonPrice(energyRegenResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "EnergySpend":
                CheckResearchButtonPrice(energySpendResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ForceProduction":
                CheckResearchButtonPrice(forceProductionResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ForceGeneration":
                CheckResearchButtonPrice(forceGenerationResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ForceSpend":
                CheckResearchButtonPrice(forceSpendResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ProductionSpeed":
                CheckResearchButtonPrice(productionTimeResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ProductionCount":
                CheckResearchButtonPrice(productionCountResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
            case "ExperienceMult":
                CheckResearchButtonPrice(experienceIncomeResearchButton, research.levels[research.currentLevel].costExperience, research.levels[research.currentLevel].costCurrency);
                break;
        }
    }

    public void SelectContract(int index)
    {
        OnContractSelected(index);
    }

    public void SelectResearch(string researchName)
    {
        if(researchSystem.availableResearchSlots > 0)
        {
            researchSystem.StartResearch(researchName);
        }
    }
    
    public void OpenResearchPanel()
    {
        if (!researchPanel.activeSelf)
        {
            researchPanel.SetActive(true);
        }
        else
        {
            researchPanel.SetActive(false);
        }
    }

}
