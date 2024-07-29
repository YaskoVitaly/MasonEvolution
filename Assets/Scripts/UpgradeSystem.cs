using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public Action<int, float> OnEnergyLimitUpgraded;
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

    public PlayerController _playerController;
    public ObjectCreator _objectCreator;
    public CoreUI _coreUI;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _objectCreator = GetComponent<ObjectCreator>();
        
    }
    void Start()
    {
        _coreUI = GetComponent<CoreUI>();

        OnEnergyLimitUpgraded(energyMaxUpgradeCost, _playerController.experience);
        OnEnergyRegenUpgraded(energyRegUpgradeCost, _playerController.experience);
        OnEnergySpendUpgraded(energySpendUpgradeCost, _playerController.experience);

        OnForceProductionUpgraded(forceProductionUpgradeCost, _playerController.experience);
        OnForceGenerationUpgraded(forceGenerationUpgradeCost, _playerController.experience);
        OnForceSpendUpgraded(forceSpendUpgradeCost, _playerController.experience);

        OnProductionTimeUpgraded(productionTimeUpgradeCost, _playerController.experience);
        OnProductionCountUpgraded(productionCountUpgradeCost, _playerController.experience);
        OnExperienceIncomeUpgraded(expIncomeUpgradeCost, _playerController.experience);

    }

    void Update()
    {
        
    }
    #region Upgrades
    public void UpgradeMaxEnergy()
    {
        if (_playerController.experience >= energyMaxUpgradeCost)
        {
            _playerController.experience -= energyMaxUpgradeCost;
            energyMaxUpgradeCost *= 2;
            _playerController.energyMax *= 2;
            OnEnergyLimitUpgraded(energyMaxUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeEnergyRegen()
    {
        if (_playerController.experience >= energyRegUpgradeCost)
        {
            _playerController.experience -= energyRegUpgradeCost;
            energyRegUpgradeCost *= 2;
            _playerController.energyReg += 1f;
            OnEnergyRegenUpgraded(energyRegUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeEnergySpend()
    {
        if (_playerController.experience >= energySpendUpgradeCost)
        {
            _playerController.experience -= energySpendUpgradeCost;
            energySpendUpgradeCost *= 2;
            _playerController.energySpend *= 0.9f;
            OnEnergySpendUpgraded(energySpendUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeForceProduction()
    {
        if(_playerController.experience >= forceProductionUpgradeCost)
        {
            _playerController.experience -= forceProductionUpgradeCost;
            forceProductionUpgradeCost *= 2;
            _playerController.workCost++;
            _playerController.forceProduction++;
            OnForceProductionUpgraded(forceProductionUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeForceSpend()
    {
        if (_playerController.experience >= forceSpendUpgradeCost)
        {
            _playerController.experience -= forceSpendUpgradeCost;
            forceSpendUpgradeCost *= 2;
            _playerController.forceSpend *= 0.9f;
            OnForceSpendUpgraded(forceSpendUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeForceAutoGeneration()
    {
        if(_playerController.experience >= forceGenerationUpgradeCost)
        {
            _playerController.experience -= forceGenerationUpgradeCost;
            forceGenerationUpgradeCost *= 2;
            if(_playerController.forceReg == 0)
            _playerController.forceReg++;
            else
                _playerController.forceTime *= 0.9f;
            OnForceGenerationUpgraded(forceGenerationUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeProductionTime()
    {
        if(_playerController.experience >= productionTimeUpgradeCost)
        {
            _playerController.experience -= productionTimeUpgradeCost;
            productionTimeUpgradeCost *= 2;
            _playerController.productionTime -= _playerController.productionTime/10;
            OnProductionTimeUpgraded(productionTimeUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeProductionCount()
    {
        if (_playerController.experience >= productionCountUpgradeCost)
        {
            _playerController.experience -= productionCountUpgradeCost;
            productionCountUpgradeCost *= 2;
            _playerController.productionCount++;
            OnProductionCountUpgraded(productionCountUpgradeCost, _playerController.experience);
        }
    }
    public void UpgradeExpirienceIncome()
    {
        if(_playerController.experience >= expIncomeUpgradeCost)
        {
            _playerController.experience -= expIncomeUpgradeCost;
            expIncomeUpgradeCost *= 2;
            _playerController.experienceMult *= 1.2f;
            OnExperienceIncomeUpgraded(expIncomeUpgradeCost, _playerController.experience);
        }
    }
    #endregion

}
