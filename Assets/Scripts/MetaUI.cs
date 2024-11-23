using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    public Action<int> OnContractSelected;
    public Action<int> OnResearchSelected;

    public GlobalData globalData;

    public GameObject researchPanel;
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
    public List<Button> researchButtons;
    
    public void Init(GlobalData _globalData)
    {
        globalData = _globalData;
        moneyText.text = "Money: " + globalData.money;
        experienceText.text = "Exp: " + globalData.totalExperience.ToString("0,0");

        /*ƒоработать активацию/деактивацию кнопок в зависимости от ресурсов и стоимости исследовани€
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, energyLimitResearchButton, upgradeSystem.energyMaxUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, energyRegenResearchButton, upgradeSystem.energyRegUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, energySpendResearchButton, upgradeSystem.energySpendUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, forceProductionResearchButton, upgradeSystem.forceProductionUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, forceGenerationResearchButton, upgradeSystem.forceGenerationUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, forceSpendResearchButton, upgradeSystem.forceSpendUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, productionTimeResearchButton, upgradeSystem.productionTimeUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, productionCountResearchButton, upgradeSystem.productionCountUpgradeCost);
        CheckResearchButtonPrice(globalData.money, globalData.totalExperience, experienceIncomeResearchButton, upgradeSystem.expIncomeUpgradeCost);
        */
    }

    public void SelectContract(int index)
    {
        OnContractSelected(index);
    }

    public void SelectResearch(int index)
    {
        OnResearchSelected(index);
    }
    private void CheckResearchButtonPrice(float valueMoney, float valueExp, Button button, int upgradePriceMoney, float upgradePriceExp)
    {
        if (valueMoney < upgradePriceMoney || valueExp < upgradePriceExp)
            button.interactable = false;
        else
            button.interactable = true;

    }
    public void OpenResearchPanel()
    {
        if (!researchPanel.activeSelf)
        {
            researchPanel.SetActive(true);
            researchOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 N Icon");

        }
        else
        {
            researchPanel.SetActive(false);
            researchOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 S Icon");
        }
    }

}
