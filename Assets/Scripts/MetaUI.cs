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
    public ContractManager contractManager;

    public GameObject researchPanel;

    public Image researchFrame;

    public Button researchOpenButton;

    /*
    public Button energyLimitResearchButton;
    public Button energyRegenResearchButton;
    public Button energySpendResearchButton;
    public Button forceProductionResearchButton;
    public Button forceGenerationResearchButton;
    public Button forceSpendResearchButton;
    public Button productionTimeResearchButton;
    public Button productionCountResearchButton;
    public Button experienceIncomeResearchButton;
    */

    public GameObject contractButtonPrefab;
    public Transform contractPanel;
    public TextMeshProUGUI newContractTimeText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI experienceText;

    public List<GameObject> contractButtons;
    public List<Button> researchButtons;


    public void Init(GlobalData _globalData, ResearchSystem _researchSystem, ContractManager _contractManager)
    {
        globalData = _globalData;
        researchSystem = _researchSystem;
        contractManager = _contractManager;
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0");
        researchSystem.OnResearchUpdated += UpdateBasicResearchButtons;
        researchSystem.OnResearchStarted += UpdateResources;
        researchSystem.OnResearchProcessed += UpdateResearchFrame;
        contractManager.OnContractTimerUpdated += UpdateContractTimer;
    }

    public void Unsubscribe()
    {
        researchSystem.OnResearchUpdated -= UpdateBasicResearchButtons;
        researchSystem.OnResearchStarted -= UpdateResources;
        researchSystem.OnResearchProcessed -= UpdateResearchFrame;
    }

    private void UpdateResources()//Добавить проверку на активность кнопок в соответствии с свободными слотами и наличием ресурсов
    {
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0");
    }

    private void UpdateResearchButton(Button button, ResearchData research)
    {
        ResearchButton researchButton = button.GetComponent<ResearchButton>();
        researchButton.researchData = research;
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

    private void UpdateContractTimer(float time)
    {
        if (time >= 0 && time < globalData.contractCooldown)
        {
            newContractTimeText.text = "New contract in: " + (globalData.contractCooldown - time).ToString("0") + " sec.";
        }
        else if (time > globalData.contractCooldown)
        {
            newContractTimeText.text = "New contract in: " + 0 + " sec.";
        }
        else
        {
            newContractTimeText.text = "The order board is full!";
        }

    }

    private void UpdateResearchFrame(ResearchData rd, float currentTime)
    {
        researchFrame.fillAmount = currentTime / rd.levels[rd.currentLevel].timeRequired;
        if(currentTime >= rd.levels[rd.currentLevel].timeRequired)
        {
            researchFrame.fillAmount = 0;
        }
    }

    private void UpdateBasicResearchButtons(ResearchData research)
    {
        foreach(Button button in researchButtons)
        {
            if(button.GetComponent<ResearchButton>().researchData.researchName == research.researchName)
            {
                UpdateResearchButton(button, research);
            }
            
        }
        /*
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
        */
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
