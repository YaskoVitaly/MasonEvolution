using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public Action<int, float, float, float> OnEnergyLimitUpgraded;
    public Action<int, float, float, float> OnEnergyRegenUpgraded;
    public Action<int, float, float, float> OnEnergySpendUpgraded;
    public Action<int, float, float, float, float, float> OnForceProductionUpgraded;
    public Action<int, float, float, float> OnForceGenerationUpgraded;
    public Action<int, float, float, float> OnForceSpendUpgraded;
    public Action<int, float, float, float> OnProductionTimeUpgraded;
    public Action<int, float, float, float> OnProductionCountUpgraded;
    public Action<int, float, float, float> OnExperienceIncomeUpgraded;

    public int basicEnergyMaxUpgradeCost = 2;
    public int basicEnergyRegUpgradeCost = 1;
    public int basicEnergySpendUpgradeCost = 5;
    public int basicForceProductionUpgradeCost = 2;
    public int basicForceGenerationUpgradeCost = 2;
    public int basicForceSpendUpgradeCost = 5;
    public int basicProductionTimeUpgradeCost = 3;
    public int basicProductionCountUpgradeCost = 3;
    public int basicExpIncomeUpgradeCost = 4;

    public int currentEnergyMaxUpgradeCost;
    public int currentEnergyRegUpgradeCost;
    public int currentEnergySpendUpgradeCost;
    public int currentForceProductionUpgradeCost;
    public int currentForceGenerationUpgradeCost;
    public int currentForceSpendUpgradeCost;
    public int currentProductionTimeUpgradeCost;
    public int currentProductionCountUpgradeCost;
    public int currentExpIncomeUpgradeCost;
    

    private PlayerData playerData;
    private GlobalData globalData;
    private ObjectCreator objectCreator;
    private CoreUI coreUI;

    public void Init(GlobalData _globalData, ObjectCreator _objectCreator, CoreUI _coreUI)
    {
        globalData = _globalData;
        playerData = globalData.playerData;
        objectCreator = _objectCreator;
        coreUI = _coreUI;

        currentEnergyMaxUpgradeCost = basicEnergyMaxUpgradeCost;
        currentEnergyRegUpgradeCost = basicEnergyRegUpgradeCost;
        currentEnergySpendUpgradeCost = basicEnergySpendUpgradeCost;
        currentForceProductionUpgradeCost = basicForceProductionUpgradeCost;
        currentForceGenerationUpgradeCost = basicForceGenerationUpgradeCost;
        currentForceSpendUpgradeCost = basicForceSpendUpgradeCost;
        currentProductionTimeUpgradeCost = basicProductionTimeUpgradeCost;
        currentProductionCountUpgradeCost = basicProductionCountUpgradeCost;
        currentExpIncomeUpgradeCost = basicExpIncomeUpgradeCost;

        OnEnergyLimitUpgraded(currentEnergyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.upgradeLevels["EnergyLimit"] + 1) * 2));
        OnEnergyRegenUpgraded(currentEnergyRegUpgradeCost, playerData.expCur, playerData.energyReg, playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"] + 1);
        OnEnergySpendUpgraded(currentEnergySpendUpgradeCost, playerData.expCur, playerData.energySpend, playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"] + 1));

        OnForceProductionUpgraded(currentForceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCost + playerData.upgradeLevels["ForceProduction"] + 1), playerData.forceProd, playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"] + 1);//доработать для отображение затрат энергии на работу
        
        if(playerData.forceReg == 0)
            OnForceGenerationUpgraded(currentForceGenerationUpgradeCost, playerData.expCur, 0, playerData.forceRegBasic);
        else
            OnForceGenerationUpgraded(currentForceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"] + 1));
        
        OnForceSpendUpgraded(currentForceSpendUpgradeCost, playerData.expCur, playerData.forceSpend, playerData.forceSpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["ForceSpend"] + 1));


        OnProductionTimeUpgraded(currentProductionTimeUpgradeCost, playerData.expCur, playerData.productionTime, playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"] + 1));
        OnProductionCountUpgraded(currentProductionCountUpgradeCost, playerData.expCur, playerData.productionCount, playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"] + 1);
        OnExperienceIncomeUpgraded(currentExpIncomeUpgradeCost, playerData.expCur, playerData.experienceMult * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"] + 1)*10);
        Debug.Log("UpgradeSystem init");
    }
    
    #region Upgrades
    public void UpgradeMaxEnergy()
    {
        if (playerData.expCur >= currentEnergyMaxUpgradeCost)
        {
            playerData.expCur -= currentEnergyMaxUpgradeCost;
            playerData.upgradeLevels["EnergyLimit"]++;
            currentEnergyMaxUpgradeCost = (int)Mathf.Pow(basicEnergyMaxUpgradeCost, (playerData.upgradeLevels["EnergyLimit"] - globalData.researchLevels["EnergyLimit"]) + 1);
            playerData.energyMax = playerData.energyMaxBasic * (playerData.upgradeLevels["EnergyLimit"] * 2);
            OnEnergyLimitUpgraded(currentEnergyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.upgradeLevels["EnergyLimit"] + 1) * 2));
        }
    }
    public void UpgradeEnergyRegen()
    {
        if (playerData.expCur >= currentEnergyRegUpgradeCost)
        {
            playerData.expCur -= currentEnergyRegUpgradeCost;
            playerData.upgradeLevels["EnergyRegeneration"]++;
            currentEnergyRegUpgradeCost = (int)Mathf.Pow(basicEnergyRegUpgradeCost, (playerData.upgradeLevels["EnergyRegeneration"] - globalData.researchLevels["EnergyRegeneration"]) + 1);
            playerData.energyReg = playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"];
            OnEnergyRegenUpgraded(currentEnergyRegUpgradeCost, playerData.expCur, playerData.energyReg, playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"] + 1);
        }
    }
    public void UpgradeEnergySpend()
    {
        if (playerData.expCur >= currentEnergySpendUpgradeCost)
        {
            playerData.expCur -= currentEnergySpendUpgradeCost;
            playerData.upgradeLevels["EnergySpend"]++;
            currentEnergySpendUpgradeCost = (int)Mathf.Pow(basicEnergySpendUpgradeCost, (playerData.upgradeLevels["EnergySpend"] - globalData.researchLevels["EnergySpend"]) + 1);
            playerData.energySpend = playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"]);
            OnEnergySpendUpgraded(currentEnergySpendUpgradeCost, playerData.expCur, playerData.energySpend, playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"] + 1));
        }
    }
    public void UpgradeForceProduction()
    {
        if(playerData.expCur >= currentForceProductionUpgradeCost)
        {
            playerData.expCur -= currentForceProductionUpgradeCost;
            playerData.upgradeLevels["ForceProduction"]++;
            currentForceProductionUpgradeCost = (int)Mathf.Pow(basicForceProductionUpgradeCost, (playerData.upgradeLevels["ForceProduction"] - globalData.researchLevels["ForceProduction"]) + 1);
            playerData.workCost++;
            playerData.forceProd++;
            playerData.workCost = (playerData.workCostBasic + playerData.upgradeLevels["ForceProduction"]) * playerData.energySpend;
            playerData.forceProd = playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"];
            OnForceProductionUpgraded(currentForceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCostBasic + playerData.upgradeLevels["ForceProduction"] + 1) * playerData.energySpend, playerData.forceProd, playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"] + 1);
        }
    }
    public void UpgradeForceSpend()
    {
        if (playerData.expCur >= currentForceSpendUpgradeCost)
        {
            playerData.expCur -= currentForceSpendUpgradeCost;
            playerData.upgradeLevels["ForceSpend"]++;
            currentForceSpendUpgradeCost = (int)Mathf.Pow(basicForceSpendUpgradeCost, (playerData.upgradeLevels["ForceSpend"] - globalData.researchLevels["ForceSpend"]) + 1);
            playerData.forceSpend = playerData.forceSpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["ForceSpend"]);
            OnForceSpendUpgraded(currentForceSpendUpgradeCost, playerData.expCur, playerData.forceSpend, (float)Math.Pow(0.95f, playerData.upgradeLevels["ForceSpend"] + 1));
        }
    }
    public void UpgradeForceAutoGeneration()
    {
        if(playerData.expCur >= currentForceGenerationUpgradeCost)
        {
            playerData.expCur -= currentForceGenerationUpgradeCost;
            if(playerData.upgradeLevels["ForceGeneration"] == 0)
            {
                playerData.forceReg = playerData.forceRegBasic;
                playerData.upgradeLevels["ForceGeneration"]++;
                currentForceGenerationUpgradeCost = (int)Mathf.Pow(basicForceGenerationUpgradeCost, (playerData.upgradeLevels["ForceGeneration"] - globalData.researchLevels["ForceGeneration"]) + 1);
                OnForceGenerationUpgraded(currentForceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]));
            }
            else
            {
                playerData.forceReg = playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]);
                playerData.upgradeLevels["ForceGeneration"]++;
                currentForceGenerationUpgradeCost = (int)Mathf.Pow(basicForceGenerationUpgradeCost, (playerData.upgradeLevels["ForceGeneration"] - globalData.researchLevels["ForceGeneration"]) + 1);
                OnForceGenerationUpgraded(currentForceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]));
            }
        }
    }
    public void UpgradeProductionTime()
    {
        if(playerData.expCur >= currentProductionTimeUpgradeCost)
        {
            playerData.expCur -= currentProductionTimeUpgradeCost;
            playerData.upgradeLevels["ProductionSpeed"]++;
            currentProductionTimeUpgradeCost = (int)Mathf.Pow(basicProductionTimeUpgradeCost, (playerData.upgradeLevels["ProductionSpeed"] - globalData.researchLevels["ProductionSpeed"]) + 1);
            playerData.productionTime = playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"]);
            OnProductionTimeUpgraded(currentProductionTimeUpgradeCost, playerData.expCur, playerData.productionTime, playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"] + 1));
        }
    }
    public void UpgradeProductionCount()
    {
        if (playerData.expCur >= currentProductionCountUpgradeCost)
        {
            playerData.expCur -= currentProductionCountUpgradeCost;
            playerData.upgradeLevels["ProductionCount"]++;
            currentProductionCountUpgradeCost = (int)Mathf.Pow(basicProductionCountUpgradeCost, (playerData.upgradeLevels["ProductionCount"] - globalData.researchLevels["ProductionCount"]) + 1);
            playerData.productionCount = playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"];
            OnProductionCountUpgraded(currentProductionCountUpgradeCost, playerData.expCur, playerData.productionCount, playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"] + 1);
        }
    }
    public void UpgradeExpirienceIncome()
    {
        if(playerData.expCur >= currentExpIncomeUpgradeCost)
        {
            playerData.expCur -= currentExpIncomeUpgradeCost;
            playerData.upgradeLevels["ExperienceMult"]++;
            currentExpIncomeUpgradeCost = (int)Mathf.Pow(basicExpIncomeUpgradeCost, (playerData.upgradeLevels["ExperienceMult"] - globalData.researchLevels["ExperienceMult"]) + 1);
            playerData.experienceMult = playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"]);
            OnExperienceIncomeUpgraded(currentExpIncomeUpgradeCost, playerData.expCur, playerData.experienceMult * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"] + 1)*10);
        }
    }
    #endregion

}
