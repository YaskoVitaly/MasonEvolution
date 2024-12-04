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
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0");
        researchSystem.OnResearchUpdated += UpdateBasicResearchButtons;
        researchSystem.OnResearchStarted += UpdateResources;
        researchSystem.OnResearchProcessed += UpdateResearchFrame;
    }

    public void Unsubscribe()
    {
        researchSystem.OnResearchUpdated -= UpdateBasicResearchButtons;
        researchSystem.OnResearchStarted -= UpdateResources;
        researchSystem.OnResearchProcessed -= UpdateResearchFrame;
    }

    private void UpdateResources()
    {
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0");
    }

    private void UpdateResearchButton(Button button, ResearchData research)
    {
        ResearchButton researchButton = button.GetComponent<ResearchButton>();
        researchButton.title.text = research.researchName + " \nresearch";
        researchButton.description.text = "Level: " + research.currentLevel.ToString();
        researchButton.priceMoney.text = "Money: " +  research.levels[research.currentLevel].costCurrency.ToString();
        researchButton.priceExp.text = "Exp: " + research.levels[research.currentLevel].costExperience.ToString();
        researchButton.time.text = "Time: " + research.levels[research.currentLevel].timeRequired / 60;


        if (globalData.money >= research.levels[research.currentLevel].costCurrency && globalData.totalExperience >= research.levels[research.currentLevel].costExperience)
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

    private void UpdateBasicResearchButtons(ResearchData research)
    {
        switch (research.researchName)
        {
            case "EnergyLimit":
                UpdateResearchButton(energyLimitResearchButton, research);
                break;
            case "EnergyRegeneration":
                UpdateResearchButton(energyRegenResearchButton, research);
                break;
            case "EnergySpend":
                UpdateResearchButton(energySpendResearchButton, research);
                break;
            case "ForceProduction":
                UpdateResearchButton(forceProductionResearchButton, research);
                break;
            case "ForceGeneration":
                UpdateResearchButton(forceGenerationResearchButton, research);
                break;
            case "ForceSpend":
                UpdateResearchButton(forceSpendResearchButton, research);
                break;
            case "ProductionSpeed":
                UpdateResearchButton(productionTimeResearchButton, research);
                break;
            case "ProductionCount":
                UpdateResearchButton(productionCountResearchButton, research);
                break;
            case "ExperienceMult":
                UpdateResearchButton(experienceIncomeResearchButton, research);
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
