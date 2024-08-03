using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public Action<int, float, float> OnEnergyLimitUpgraded;
    public Action<int, float> OnEnergyRegenUpgraded;
    public Action<int, float> OnEnergySpendUpgraded;
    public Action<int, float> OnForceProductionUpgraded;
    public Action<int, float> OnForceGenerationUpgraded;
    public Action<int, float> OnForceSpendUpgraded;
    public Action<int, float> OnProductionTimeUpgraded;
    public Action<int, float> OnProductionCountUpgraded;
    public Action<int, float> OnExperienceIncomeUpgraded;

    public int energyMaxUpgradeCost = 1;
    public int energyRegUpgradeCost = 1;
    public int energySpendUpgradeCost = 1;
    public int forceProductionUpgradeCost = 1;
    public int forceGenerationUpgradeCost = 1;
    public int forceSpendUpgradeCost = 1;
    public int productionTimeUpgradeCost = 1;
    public int productionCountUpgradeCost = 1;
    public int expIncomeUpgradeCost = 1;

    /*
    private int energyLimitUpgradeLevel = 0;
    private int energyRegenerationUpgradeLevel = 0;
    private int energySpendUpgradeLevel = 0;
    private int forceProductionUpgradeLevel = 0;
    private int forceGenerationUpgradeLevel = 0;
    private int forceSpendUpgradeLevel = 0;
    private int productionTimeUpgradeLevel = 0;
    private int productionCountUpgradeLevel = 0;
    private int expIncomeUpgradeLevel = 0;
    */

    private PlayerData playerData;
    private ObjectCreator objectCreator;
    private CoreUI coreUI;

    public void Init(PlayerData _playerData, ObjectCreator _objectCreator, CoreUI _coreUI)
    {
        playerData = _playerData;
        objectCreator = _objectCreator;
        coreUI = _coreUI;
    }

    void Start()
    {
        OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax);
        OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur);
        OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur);

        OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur);
        OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur);
        OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur);

        OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur);
        OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur);
        OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur);
    }

    void Update()
    {
        
    }
    #region Upgrades
    public void UpgradeMaxEnergy()
    {
        if (playerData.expCur >= energyMaxUpgradeCost)
        {
            playerData.expCur -= energyMaxUpgradeCost;
            energyMaxUpgradeCost *= 2;
            playerData.energyMax *= 2;
            OnEnergyLimitUpgraded(energyMaxUpgradeCost, playerData.expCur, playerData.energyMax);
        }
    }
    public void UpgradeEnergyRegen()
    {
        if (playerData.expCur >= energyRegUpgradeCost)
        {
            playerData.expCur -= energyRegUpgradeCost;
            energyRegUpgradeCost *= 2;
            playerData.energyReg += 1f;
            OnEnergyRegenUpgraded(energyRegUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeEnergySpend()
    {
        if (playerData.expCur >= energySpendUpgradeCost)
        {
            playerData.expCur -= energySpendUpgradeCost;
            energySpendUpgradeCost *= 2;
            playerData.energySpend *= 0.9f;
            OnEnergySpendUpgraded(energySpendUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeForceProduction()
    {
        if(playerData.expCur >= forceProductionUpgradeCost)
        {
            playerData.expCur -= forceProductionUpgradeCost;
            forceProductionUpgradeCost *= 2;
            playerData.workCost++;
            playerData.forceProd++;
            OnForceProductionUpgraded(forceProductionUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeForceSpend()
    {
        if (playerData.expCur >= forceSpendUpgradeCost)
        {
            playerData.expCur -= forceSpendUpgradeCost;
            forceSpendUpgradeCost *= 2;
            playerData.forceSpend *= 0.9f;
            OnForceSpendUpgraded(forceSpendUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeForceAutoGeneration()
    {
        if(playerData.expCur >= forceGenerationUpgradeCost)
        {
            playerData.expCur -= forceGenerationUpgradeCost;
            forceGenerationUpgradeCost *= 2;
            if(playerData.forceReg == 0)
                playerData.forceReg++;
            else
                playerData.forceTime *= 0.9f;
            OnForceGenerationUpgraded(forceGenerationUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeProductionTime()
    {
        if(playerData.expCur >= productionTimeUpgradeCost)
        {
            playerData.expCur -= productionTimeUpgradeCost;
            productionTimeUpgradeCost *= 2;
            playerData.productionTime -= playerData.productionTime/10;
            OnProductionTimeUpgraded(productionTimeUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeProductionCount()
    {
        if (playerData.expCur >= productionCountUpgradeCost)
        {
            playerData.expCur -= productionCountUpgradeCost;
            productionCountUpgradeCost *= 2;
            playerData.productionCount++;
            OnProductionCountUpgraded(productionCountUpgradeCost, playerData.expCur);
        }
    }
    public void UpgradeExpirienceIncome()
    {
        if(playerData.expCur >= expIncomeUpgradeCost)
        {
            playerData.expCur -= expIncomeUpgradeCost;
            expIncomeUpgradeCost *= 2;
            playerData.experienceMult *= 1.2f;
            OnExperienceIncomeUpgraded(expIncomeUpgradeCost, playerData.expCur);
        }
    }
    #endregion

}
