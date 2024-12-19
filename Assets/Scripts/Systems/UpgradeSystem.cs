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

    public int energyMaxUpgradeCost = 2;
    public int energyRegUpgradeCost = 1;
    public int energySpendUpgradeCost = 5;
    public int forceProductionUpgradeCost = 2;
    public int forceGenerationUpgradeCost = 2;
    public int forceSpendUpgradeCost = 5;
    public int productionTimeUpgradeCost = 3;
    public int productionCountUpgradeCost = 3;
    public int expIncomeUpgradeCost = 4;
    
    

    private PlayerData playerData;
    private ObjectCreator objectCreator;
    private CoreUI coreUI;

    public void Init(PlayerData _playerData, ObjectCreator _objectCreator, CoreUI _coreUI)
    {
        playerData = _playerData;
        objectCreator = _objectCreator;
        coreUI = _coreUI;

        OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.upgradeLevels["EnergyLimit"] + 1) * 2));
        OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur, playerData.energyReg, playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"] + 1);
        OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur, playerData.energySpend, playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"] + 1));

        OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCost + playerData.upgradeLevels["ForceProduction"] + 1), playerData.forceProd, playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"] + 1);//доработать для отображение затрат энергии на работу
        
        if(playerData.forceReg == 0)
            OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, 0, playerData.forceRegBasic);
        else
            OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"] + 1));
        
        OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur, playerData.forceSpend, playerData.forceSpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["ForceSpend"] + 1));


        OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur, playerData.productionTime, playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"] + 1));
        OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur, playerData.productionCount, playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"] + 1);
        OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur, playerData.experienceMult * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"] + 1)*10);
        Debug.Log("UpgradeSystem init");
    }
    
    #region Upgrades
    public void UpgradeMaxEnergy()
    {
        if (playerData.expCur >= energyMaxUpgradeCost)
        {
            playerData.expCur -= energyMaxUpgradeCost;
            playerData.upgradeLevels["EnergyLimit"]++;
            energyMaxUpgradeCost *= 2;
            playerData.energyMax = playerData.energyMaxBasic * (playerData.upgradeLevels["EnergyLimit"] * 2);
            OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.upgradeLevels["EnergyLimit"] + 1) * 2));
        }
    }
    public void UpgradeEnergyRegen()
    {
        if (playerData.expCur >= energyRegUpgradeCost)
        {
            playerData.expCur -= energyRegUpgradeCost;
            playerData.upgradeLevels["EnergyRegeneration"]++;
            energyRegUpgradeCost *= 2;
            playerData.energyReg = playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"];
            OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur, playerData.energyReg, playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"] + 1);
        }
    }
    public void UpgradeEnergySpend()
    {
        if (playerData.expCur >= energySpendUpgradeCost)
        {
            playerData.expCur -= energySpendUpgradeCost;
            playerData.upgradeLevels["EnergySpend"]++;
            energySpendUpgradeCost *= 2;
            playerData.energySpend = playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"]);
            OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur, playerData.energySpend, playerData.energySpendBasic * (float)Math.Pow(0.95f, playerData.upgradeLevels["EnergySpend"] + 1));
        }
    }
    public void UpgradeForceProduction()
    {
        if(playerData.expCur >= forceProductionUpgradeCost)
        {
            playerData.expCur -= forceProductionUpgradeCost;
            playerData.upgradeLevels["ForceProduction"]++;
            forceProductionUpgradeCost *= 2;
            playerData.workCost++;
            playerData.forceProd++;
            playerData.workCost = (playerData.workCostBasic + playerData.upgradeLevels["ForceProduction"]) * playerData.energySpend;
            playerData.forceProd = playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"];
            OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCostBasic + playerData.upgradeLevels["ForceProduction"] + 1) * playerData.energySpend, playerData.forceProd, playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"] + 1);
        }
    }
    public void UpgradeForceSpend()
    {
        if (playerData.expCur >= forceSpendUpgradeCost)
        {
            playerData.expCur -= forceSpendUpgradeCost;
            playerData.upgradeLevels["ForceSpend"]++;
            forceSpendUpgradeCost *= 2;
            playerData.forceSpend = playerData.forceSpendBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ForceSpend"]);
            OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur, playerData.forceSpend, (float)Math.Pow(0.95f, playerData.upgradeLevels["ForceSpend"] + 1));
        }
    }
    public void UpgradeForceAutoGeneration()
    {
        if(playerData.expCur >= forceGenerationUpgradeCost)
        {
            playerData.expCur -= forceGenerationUpgradeCost;
            forceGenerationUpgradeCost *= 2;
            if(playerData.upgradeLevels["ForceGeneration"] == 0)
            {
                playerData.forceReg = playerData.forceRegBasic;
                playerData.upgradeLevels["ForceGeneration"]++;
                OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]));
            }
            else
            {
                playerData.forceReg = playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]);
                playerData.upgradeLevels["ForceGeneration"]++;
                OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, playerData.forceRegBasic * (float)Math.Pow(0.8f, playerData.upgradeLevels["ForceGeneration"]));
            }
        }
    }
    public void UpgradeProductionTime()
    {
        if(playerData.expCur >= productionTimeUpgradeCost)
        {
            playerData.expCur -= productionTimeUpgradeCost;
            playerData.upgradeLevels["ProductionSpeed"]++;
            productionTimeUpgradeCost *= 2;
            playerData.productionTime = playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"]);
            OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur, playerData.productionTime, playerData.productionTimeBasic * (float)Math.Pow(0.88f, playerData.upgradeLevels["ProductionSpeed"] + 1));
        }
    }
    public void UpgradeProductionCount()
    {
        if (playerData.expCur >= productionCountUpgradeCost)
        {
            playerData.expCur -= productionCountUpgradeCost;
            playerData.upgradeLevels["ProductionCount"]++;
            productionCountUpgradeCost *= 2;
            playerData.productionCount = playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"];
            OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur, playerData.productionCount, playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"] + 1);
        }
    }
    public void UpgradeExpirienceIncome()
    {
        if(playerData.expCur >= expIncomeUpgradeCost)
        {
            playerData.expCur -= expIncomeUpgradeCost;
            playerData.upgradeLevels["ExperienceMult"]++;
            expIncomeUpgradeCost *= 2;
            playerData.experienceMult = playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"]);
            OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur, playerData.experienceMult * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"] + 1)*10);
        }
    }
    #endregion

}
