using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MetaUI : MonoBehaviour
{
    public Action<int> OnContractSelected;
    public Action<string> OnResearchSelected;
    public Action<ContractData> OnContractContinued;

    public GameObject ContractInfoWindow;
    public GlobalData globalData;
    public ResearchSystem researchSystem;
    public ContractManager contractManager;

    public GameObject researchPanel;

    public Image researchFrame;

    public Button researchOpenButton;
    

    public RectTransform bottomPanel;
    public RectTransform contractPanel;
    public RectTransform activeContractPanel;

    public GameObject contractInfoPanel;
    public ContractInfo contractInfo;


    public GameObject contractButtonPrefab;
    public GameObject activeContractButtonPrefab;

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
        contractManager.OnContractGenerated += CreateContractButton;
        contractInfo = contractInfoPanel.GetComponent<ContractInfo>();
        contractInfo.OnNextContractSelected += UpdateContractInfo;
        contractInfo.OnPreviousContractSelected += UpdateContractInfo;
        contractInfo.OnContractDeleted += DeleteContract;

        if(globalData.currentContract != null)
        {
            CreateActiveContractButton();
        }
    }

    public void Unsubscribe()
    {
        researchSystem.OnResearchUpdated -= UpdateBasicResearchButtons;
        researchSystem.OnResearchStarted -= UpdateResources;
        researchSystem.OnResearchProcessed -= UpdateResearchFrame;
        contractManager.OnContractTimerUpdated -= UpdateContractTimer;
        contractManager.OnContractGenerated -= CreateContractButton;
        contractInfo.OnNextContractSelected -= UpdateContractInfo;
        contractInfo.OnPreviousContractSelected -= UpdateContractInfo;
        contractInfo.OnContractDeleted -= DeleteContract;
    }

    private void UpdateResources(ResearchData rd)//Добавить проверку на активность кнопок в соответствии с свободными слотами и наличием ресурсов
    {
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0");
        foreach (Button button in researchButtons)
        {
            if (button.GetComponent<ResearchButton>().researchData.researchName == rd.researchName)
            {
                ResearchButton researchButton = button.GetComponent<ResearchButton>();
                researchButton.researchData = rd;
                researchButton.title.text = rd.researchName + " \nresearch";
                researchButton.description.text = "Level: " + (rd.currentLevel + 1).ToString();
                researchButton.priceMoney.text = "Money: " + rd.levels[rd.currentLevel + 1].costCurrency.ToString();
                researchButton.priceExp.text = "Exp: " + rd.levels[rd.currentLevel + 1].costExperience.ToString();
                researchButton.time.text = "Time: " + rd.levels[rd.currentLevel + 1].timeRequired / 60;
                button.interactable = false;
            }
            if(researchSystem.availableResearchSlots == 0)
            {
                button.interactable = false;
            }
        }
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
        if(researchSystem.availableResearchSlots > 0)
        {
            if (globalData.money >= research.levels[research.currentLevel].costCurrency && globalData.totalExperience >= research.levels[research.currentLevel].costExperience)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
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
    public void OpenContractInfo(ContractData cd)
    {
        if(globalData.currentContract == null)
        {
            contractInfoPanel.SetActive(true);
            ContractInfo contractInfo = contractInfoPanel.GetComponent<ContractInfo>();
            contractInfo.Init(cd, this);
        }
        else
        {
            Debug.LogWarning("Finish your current contract first");
        }

    }
    public void UpdateContractInfo(ContractData cd, bool flag)
    {
        int curCdIndex = globalData.activeContracts.IndexOf(cd);
        ContractData nextCD = null;
        if (flag && globalData.activeContracts.Count > curCdIndex+1)
        {
            nextCD = globalData.activeContracts[curCdIndex+1];
        }
        else if(globalData.activeContracts.Count == 1)
        {
            nextCD = cd;
        }
        else if(!flag && curCdIndex > 0)
        {
            nextCD = globalData.activeContracts[curCdIndex - 1];
        }
        else
        {
            nextCD = globalData.activeContracts[0];
        }
        contractInfo.UpdateData(nextCD);
    }
    public void DeleteContract(ContractData cd)//Поправить метод!
    {
        GameObject tempButton = null;
        foreach (GameObject b in contractButtons)
        {
            if (b.GetComponent<ContractButton>().contractData == cd)
            {
                tempButton = b;
            }
        }
        Destroy(tempButton);
        contractButtons.Remove(tempButton);
        globalData.activeContracts.Remove(cd);

        if (globalData.activeContracts.Count == 0)
        {
            contractInfoPanel.SetActive(false);
        }
        else
        {
            UpdateContractInfo(cd, true);
        }
    }
    private void CreateContractButton(ContractData contractData)
    {
        GameObject buttonObject = Instantiate(contractButtonPrefab, contractPanel);
        //Debug.Log(contractPanel.transform.position);
        contractButtons.Add(buttonObject);
        ContractButton contractButton = buttonObject.GetComponent<ContractButton>();
        Button button = buttonObject.GetComponent<Button>();
        contractButton.Init(contractData);
        button.onClick.AddListener(() => OpenContractInfo(contractData));
    }
    private void CreateActiveContractButton()
    {
        Debug.LogWarning("Create active contract button");

        GameObject buttonObject = Instantiate(activeContractButtonPrefab, activeContractPanel);
        //Debug.Log(contractPanel.transform.position);
        //contractButtons.Add(buttonObject);
        ContractButton contractButton = buttonObject.GetComponent<ContractButton>();
        Button button = buttonObject.GetComponent<Button>();
        contractButton.Init(globalData.currentContract);
        button.onClick.AddListener(() => ContractContinue());
    }
    private void ContractContinue()
    {
        Debug.Log(globalData.currentContract.title);
        OnContractContinued(globalData.currentContract);
    }
}
