using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoreUI : MonoBehaviour
{
    public Action<int> OnQuarkGenerated;
    public Action<int, int, float, List<Quark>> OnProductionFinished;
    public Action<float> OnMetaLoaded;

    private ObjectCreator objectCreator;
    private PlayerController playerController;
    private UpgradeSystem upgradeSystem;
    private PlayerData playerData;

    public float timer;

    public Button energyUpgradeOpenButton;
    public GameObject energyUpgradePanel;
    public Button forceUpgradeOpenButton;
    public GameObject forceUpgradePanel;
    public Button productionUpgradeOpenButton;
    public GameObject productionUpgradePanel;

    public Button energyLimitUpgradeButton;
    public Button energyRegenUpgradeButton;
    public Button energySpendUpgradeButton;
    public Button forceProductionUpgradeButton;
    public Button forceGenerationUpgradeButton;
    public Button forceSpendUpgradeButton;
    public Button productionTimeUpgradeButton;
    public Button productionCountUpgradeButton;
    public Button experienceIncomeUpgradeButton;

    public TextMeshProUGUI forceCountText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI productionText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI quarkCounterText;
    public TextMeshProUGUI finishedObjectsText;

    public TextMeshProUGUI energyLimitUpgradePriceText;
    public TextMeshProUGUI energyRegenUpgradePriceText;
    public TextMeshProUGUI energySpendUpgradePriceText;
    public TextMeshProUGUI forceProductionUpgradePriceText;
    public TextMeshProUGUI forceGenerationUpgradePriceText;
    public TextMeshProUGUI forceSpendUpgradePriceText;
    public TextMeshProUGUI productionTimeUpgradePriceText;
    public TextMeshProUGUI productionCountUpgradePriceText;
    public TextMeshProUGUI experienceIncomeUpgradePriceText;

    public TextMeshProUGUI energyLimitUpgradeDescriptionText;
    public TextMeshProUGUI energyRegenUpgradeDescriptionText;
    public TextMeshProUGUI energySpendUpgradeDescriptionText;
    public TextMeshProUGUI forceProductionUpgradeDescriptionText;
    public TextMeshProUGUI forceGenerationUpgradeDescriptionText;
    public TextMeshProUGUI forceSpendUpgradeDescriptionText;
    public TextMeshProUGUI productionTimeUpgradeDescriptionText;
    public TextMeshProUGUI productionCountUpgradeDescriptionText;
    public TextMeshProUGUI experienceIncomeUpgradeDescriptionText;

    public Image energyBar;
    public Image productionBar;

    private Coroutine tempCoroutine;
    public void Init(PlayerController _playerController, PlayerData _playerData, ObjectCreator _objectCreator, UpgradeSystem _upgradeSystem)
    {
        playerController = _playerController;
        objectCreator = _objectCreator;
        upgradeSystem = _upgradeSystem;
        playerData = _playerData;

        upgradeSystem.OnEnergyLimitUpgraded += EnegryLimitUpgradePriceChange;
        upgradeSystem.OnEnergyRegenUpgraded += EnegryRegenUpgradePriceChange;
        upgradeSystem.OnEnergySpendUpgraded += EnegrySpendUpgradePriceChange;
        upgradeSystem.OnForceProductionUpgraded += ForceProductionUpgradePriceChange;
        upgradeSystem.OnForceGenerationUpgraded += ForceGenerationUpgradePriceChange;
        upgradeSystem.OnForceSpendUpgraded += ForceSpendUpgradePriceChange;
        upgradeSystem.OnProductionTimeUpgraded += ProductionTimeUpgradePriceChange;
        upgradeSystem.OnProductionCountUpgraded += ProductionCountUpgradePriceChange;
        upgradeSystem.OnExperienceIncomeUpgraded += ExperienceIncomeUpgradePriceChange;


        playerController.OnEnergyChanged += EnergyChange;
        playerController.OnExperienceChanged += ExperinceChange;
        playerController.OnObjectCompleted += FinishedObjectsChange;
        playerController.OnProductionStarted += Production;
        playerController.OnWorked += ForceChange;

        OnQuarkGenerated += QuarkCounterChange;

        productionBar.fillAmount = 0;
        timer = 0;
        forceCountText.text = "Force: 0";
        experienceText.text = "EXP: " + _playerData.expCur.ToString();
        quarkCounterText.text = "Quark count: " + playerController.currentQuark;

        FinishedObjectsChange(0);
        StartCoroutine(PlayTimer());
        ExperinceChange(0);
        Debug.Log("CoreUI initialized");
    }
    public void Work()
    {
        playerController.Work();
    }
    private void CheckUpgradeButtonPrice(float value, Button button, int upgradePrice)
    {
        if(value < upgradePrice)
            button.interactable = false;
        else
            button.interactable = true;

    }
    public void OpenEnergyUpgradePanel()
    {
        if (!energyUpgradePanel.activeSelf)
        {
            energyUpgradePanel.SetActive(true);
            energyUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 S Icon");
        }
        else
        {
            energyUpgradePanel.SetActive(false);
            energyUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 N Icon");
        }
    }
    public void OpenForceUpgradePanel()
    {
        if (!forceUpgradePanel.activeSelf)
        {
            forceUpgradePanel.SetActive(true);
            forceUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 S Icon");
        }
        else
        {
            forceUpgradePanel.SetActive(false);
            forceUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 N Icon");
        }
    }
    public void OpenProductionUpgradePanel()
    {
        if (!productionUpgradePanel.activeSelf)
        {
            productionUpgradePanel.SetActive(true);
            productionUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 S Icon");
        }
        else
        {
            productionUpgradePanel.SetActive(false);
            productionUpgradeOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 N Icon");
        }
    }
    #region Text Changers
    private void ExperinceChange(float value)
    {
        experienceText.text = "Exp: " + value.ToString("0.0");
        CheckUpgradeButtonPrice(playerData.expCur, energyLimitUpgradeButton, upgradeSystem.energyMaxUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, energyRegenUpgradeButton, upgradeSystem.energyRegUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, energySpendUpgradeButton, upgradeSystem.energySpendUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, forceProductionUpgradeButton, upgradeSystem.forceProductionUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, forceGenerationUpgradeButton, upgradeSystem.forceGenerationUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, forceSpendUpgradeButton, upgradeSystem.forceSpendUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, productionTimeUpgradeButton, upgradeSystem.productionTimeUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, productionCountUpgradeButton, upgradeSystem.productionCountUpgradeCost);
        CheckUpgradeButtonPrice(playerData.expCur, experienceIncomeUpgradeButton, upgradeSystem.expIncomeUpgradeCost);
    }
    private void EnergyChange(float energyCur, float energyMax)
    {
        energyText.text = energyCur.ToString("0.0") + "/" + energyMax.ToString("0");
        energyBar.fillAmount = energyCur / energyMax;
    }
    private void ForceChange(float value)
    {
        forceCountText.text = "Force: " + value.ToString("0.0");
    }
    private void FinishedObjectsChange(int value)
    {
        finishedObjectsText.text = "Finished objects: " + value.ToString() +" / "+ playerData.currentContract.count;
    }
    private void QuarkCounterChange(int count)
    {
        quarkCounterText.text = "Quark count: " + count;
    }
    #endregion
    #region Upgrades

    public void EnergyLimitUpgrade()
    {
        upgradeSystem.UpgradeMaxEnergy();
    }
    public void EnergyRegenUpgrade()
    {
        upgradeSystem.UpgradeEnergyRegen();
    }
    public void EnergySpendUpgrade()
    {
        upgradeSystem.UpgradeEnergySpend();
    }
    public void ForceProductionUpgrade()
    {
        upgradeSystem.UpgradeForceProduction();
    }
    public void ForceGenerationUpgrade()
    {
        upgradeSystem.UpgradeForceAutoGeneration();
    }
    public void ForceSpendUpgrade()
    {
        upgradeSystem.UpgradeForceSpend();
    }
    public void ProductionTimeUpgrade()
    {
        upgradeSystem.UpgradeProductionTime();
    }
    public void ProductionCountUpgrade()
    {
        upgradeSystem.UpgradeProductionCount();
    }
    public void ExperienceIncomeUpgrade()
    {
        upgradeSystem.UpgradeExpirienceIncome();
    }

    private void EnegryLimitUpgradePriceChange(int count, float exp, float energyMaxCur, float energyMaxNext)
    {
        energyLimitUpgradePriceText.text = "Exp: " + count;
        energyLimitUpgradeDescriptionText.text = energyMaxCur.ToString("0") + " => " + (energyMaxNext).ToString("0");
        ExperinceChange(exp);
    }
    private void EnegryRegenUpgradePriceChange(int count, float exp, float energyRegCur, float energyRegNext)
    {
        energyRegenUpgradePriceText.text = "Exp: " + count;
        energyRegenUpgradeDescriptionText.text = energyRegCur.ToString("0") + " => " + energyRegNext.ToString("0");
        ExperinceChange(exp);
    }
    private void EnegrySpendUpgradePriceChange(int count, float exp, float energySpendCur, float energySpendNext)
    {
        energySpendUpgradePriceText.text = "Exp: " + count;
        energySpendUpgradeDescriptionText.text = energySpendCur.ToString("0.0") + " => " + energySpendNext.ToString("0.0");

        ExperinceChange(exp);
    }
    private void ForceProductionUpgradePriceChange(int count, float exp, float workCostCur, float workCostNext, float forceProdCur, float forceProdNext)
    {
        forceProductionUpgradePriceText.text = "Exp: " + count;
        forceProductionUpgradeDescriptionText.text = forceProdCur.ToString("0") + " => " + forceProdNext.ToString("0");
        ExperinceChange(exp);
    }
    private void ForceGenerationUpgradePriceChange(int count, float exp, float forceGenCur, float forceGenNext)
    {
        forceGenerationUpgradePriceText.text = "Exp: " + count;
        forceGenerationUpgradeDescriptionText.text = forceGenCur.ToString("0.0") + " => " + forceGenNext.ToString("0.0");
        ExperinceChange(exp);
    }
    private void ForceSpendUpgradePriceChange(int count, float exp, float forceSpendCur, float forceSpendNext)
    {
        forceSpendUpgradePriceText.text = "Exp: " + count;
        forceSpendUpgradeDescriptionText.text = forceSpendCur.ToString("0.0") + " => " + forceSpendNext.ToString("0.0");
        ExperinceChange(exp);
    }
    private void ProductionTimeUpgradePriceChange(int count, float exp, float prodTimeCur, float prodTimeNext)
    {
        productionTimeUpgradePriceText.text = "Exp: " + count;
        productionTimeUpgradeDescriptionText.text = prodTimeCur.ToString("0.0") + " => " + prodTimeNext.ToString("0.0");
        ExperinceChange(exp);
    }
    private void ProductionCountUpgradePriceChange(int count, float exp, float prodCountCur, float prodCountNext)
    {
        productionCountUpgradePriceText.text = "Exp: " + count;
        productionCountUpgradeDescriptionText.text = prodCountCur.ToString("0") + " => " + prodCountNext.ToString("0");
        ExperinceChange(exp);
    }
    private void ExperienceIncomeUpgradePriceChange(int count, float exp, float expMultCur, float expMultNext)
    {
        experienceIncomeUpgradePriceText.text = "Exp: " + count;
        experienceIncomeUpgradeDescriptionText.text = expMultCur.ToString("0.0") + " => " + expMultNext.ToString("0.0");
        ExperinceChange(exp);
    }
    #endregion
    private void Production(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)//¬ыводить врем€ до окончани€ производства
    {
        if(tempCoroutine != null)
        {
            StopCoroutine(tempCoroutine);
            tempCoroutine = null;
        }
        tempCoroutine = StartCoroutine(ProductionBarFiller(time, currentQuark, expCur, expCount, purchasedQuarks));
    }
    private IEnumerator ProductionBarFiller(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)
    {
        float productionTimer = 0;
        while (productionTimer < time)
        {
            productionTimer += Time.deltaTime;
            productionText.text = productionTimer.ToString("0.0") + "/" + time.ToString("0.0");
            productionBar.fillAmount = productionTimer / time;
            if (productionTimer >= time)
            {
                productionBar.fillAmount = 0;
                OnProductionFinished(purchasedQuarks.Count, currentQuark, expCur, purchasedQuarks);
            }
            yield return null;
        }
    }
    private IEnumerator PlayTimer()
    {
        while (true)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("0.00");
            yield return new WaitForEndOfFrame();
        }
    }

    public void LoadMeta()
    {
        OnMetaLoaded(playerData.expTotal/100);
    }
}
