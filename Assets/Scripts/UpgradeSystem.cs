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
    public int energySpendUpgradeCost = 4;
    public int forceProductionUpgradeCost = 2;
    public int forceGenerationUpgradeCost = 3;
    public int forceSpendUpgradeCost = 4;
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



        OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.energyLimitUpgradeLevel+1) * 2));
        OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur, playerData.energyRegBasic, playerData.energyRegBasic + playerData.energyRegenerationUpgradeLevel+1);
        OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur, playerData.energySpendBasic, playerData.energySpendBasic * (float)Math.Pow(0.9f, playerData.energySpendUpgradeLevel+1));

        OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCostBasic + playerData.forceProductionUpgradeLevel+1) * playerData.energySpend, playerData.forceProd, playerData.forceProdBasic + playerData.forceProductionUpgradeLevel+1);
        OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, 0, playerData.forceRegBasic);
        OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur, playerData.forceSpendBasic, (float)Math.Pow(0.9f, playerData.forceSpendUpgradeLevel+1));

        OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur, playerData.productionTimeBasic, playerData.productionTimeBasic * (float)Math.Pow(0.9f, playerData.productionTimeUpgradeLevel+1));
        OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur, playerData.productionCountBasic, playerData.productionCountBasic + playerData.productionTimeUpgradeLevel+1);
        OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur, playerData.experienceMultBasic * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.expIncomeUpgradeLevel+1)*10);
        Debug.Log("UpgradeSystem init");
    }
    
    #region Upgrades
    public void UpgradeMaxEnergy()
    {
        if (playerData.expCur >= energyMaxUpgradeCost)
        {
            playerData.expCur -= energyMaxUpgradeCost;
            playerData.energyLimitUpgradeLevel++;
            energyMaxUpgradeCost *= 2;
            playerData.energyMax = playerData.energyMaxBasic * (playerData.energyLimitUpgradeLevel * 2);
            OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax, playerData.energyMaxBasic * ((playerData.energyLimitUpgradeLevel + 1) * 2));
        }
    }
    public void UpgradeEnergyRegen()
    {
        if (playerData.expCur >= energyRegUpgradeCost)
        {
            playerData.expCur -= energyRegUpgradeCost;
            playerData.energyRegenerationUpgradeLevel++;
            energyRegUpgradeCost *= 2;
            playerData.energyReg = playerData.energyRegBasic + playerData.energyRegenerationUpgradeLevel;
            OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur, playerData.energyReg, playerData.energyRegBasic + playerData.energyRegenerationUpgradeLevel+1);
        }
    }
    public void UpgradeEnergySpend()
    {
        if (playerData.expCur >= energySpendUpgradeCost)
        {
            playerData.expCur -= energySpendUpgradeCost;
            playerData.energySpendUpgradeLevel++;
            energySpendUpgradeCost *= 2;
            playerData.energySpend = playerData.energySpendBasic * (float)Math.Pow(0.9f, playerData.energySpendUpgradeLevel);
            OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur, playerData.energySpend, playerData.energySpendBasic * (float)Math.Pow(0.9f, playerData.energySpendUpgradeLevel+1));
        }
    }
    public void UpgradeForceProduction()
    {
        if(playerData.expCur >= forceProductionUpgradeCost)
        {
            playerData.expCur -= forceProductionUpgradeCost;
            playerData.forceProductionUpgradeLevel++;
            forceProductionUpgradeCost *= 2;
            playerData.workCost++;
            playerData.forceProd++;
            playerData.workCost = (playerData.workCostBasic + playerData.forceProductionUpgradeLevel)*playerData.energySpend;
            playerData.forceProd = playerData.forceProdBasic + playerData.forceProductionUpgradeLevel;
            OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur, playerData.workCost, (playerData.workCostBasic + playerData.forceProductionUpgradeLevel+1) * playerData.energySpend, playerData.forceProd, playerData.forceProdBasic + playerData.forceProductionUpgradeLevel+1);
        }
    }
    public void UpgradeForceSpend()
    {
        if (playerData.expCur >= forceSpendUpgradeCost)
        {
            playerData.expCur -= forceSpendUpgradeCost;
            playerData.forceSpendUpgradeLevel++;
            forceSpendUpgradeCost *= 2;
            playerData.forceSpend = playerData.forceSpendBasic * (float)Math.Pow(0.9f, playerData.forceSpendUpgradeLevel);
            OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur, playerData.forceSpend, (float)Math.Pow(0.9f, playerData.forceSpendUpgradeLevel+1));
        }
    }
    public void UpgradeForceAutoGeneration()
    {
        if(playerData.expCur >= forceGenerationUpgradeCost)
        {
            playerData.expCur -= forceGenerationUpgradeCost;
            forceGenerationUpgradeCost *= 2;
            if(playerData.forceGenerationUpgradeLevel == 0)
            {
                playerData.forceReg = playerData.forceRegBasic;
                playerData.forceGenerationUpgradeLevel++;
                OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, (float)Math.Pow(0.9f, playerData.forceGenerationUpgradeLevel));
            }
            else
            {
                playerData.forceGenerationUpgradeLevel++;
                playerData.forceTime = playerData.forceTimeBasic * (float)Math.Pow(0.9f, playerData.forceGenerationUpgradeLevel);
                OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur, playerData.forceReg, (float)Math.Pow(0.9f, playerData.forceGenerationUpgradeLevel + 1));
            }
        }
    }
    public void UpgradeProductionTime()
    {
        if(playerData.expCur >= productionTimeUpgradeCost)
        {
            playerData.expCur -= productionTimeUpgradeCost;
            playerData.productionTimeUpgradeLevel++;
            productionTimeUpgradeCost *= 2;
            playerData.productionTime = playerData.productionTimeBasic * (float)Math.Pow(0.9f, playerData.productionTimeUpgradeLevel);
            OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur, playerData.productionTime, playerData.productionTimeBasic * (float)Math.Pow(0.9f, playerData.productionTimeUpgradeLevel+1));
        }
    }
    public void UpgradeProductionCount()
    {
        if (playerData.expCur >= productionCountUpgradeCost)
        {
            playerData.expCur -= productionCountUpgradeCost;
            playerData.productionCountUpgradeLevel++;
            productionCountUpgradeCost *= 2;
            playerData.productionCount = playerData.productionCountBasic + playerData.productionCountUpgradeLevel;
            OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur, playerData.productionCount, playerData.productionCountBasic + playerData.productionCountUpgradeLevel + 1);
        }
    }
    public void UpgradeExpirienceIncome()
    {
        if(playerData.expCur >= expIncomeUpgradeCost)
        {
            playerData.expCur -= expIncomeUpgradeCost;
            playerData.expIncomeUpgradeLevel++;
            expIncomeUpgradeCost *= 2;
            playerData.experienceMult = playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.expIncomeUpgradeLevel);
            OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur, playerData.experienceMult * 10, playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.expIncomeUpgradeLevel+1)*10);
        }
    }
    #endregion

}
