using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoreUI : MonoBehaviour
{
    public Action<int> OnQuarkGenerated;
    public Action<int, int, float, List<Quark>> OnProductionFinished;

    private ObjectCreator _objectCreator;
    private PlayerController _playerController;
    private UpgradeSystem _upgradeSystem;

    public float timer;

    public Button energyUpgradeOpenButton;
    public GameObject energyUpgradePanel;
    public Button forceUpgradeOpenButton;
    public GameObject forceUpgradePanel;
    public Button productionUpgradeOpenButton;
    public GameObject productionUpgradePanel;

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
    public TextMeshProUGUI forceTimeUpgradePriceText;
    public TextMeshProUGUI productionTimeUpgradePriceText;
    public TextMeshProUGUI productionCountUpgradePriceText;
    public TextMeshProUGUI experienceIncomeUpgradePriceText;

    public Image energyBar;
    public Image productionBar;

    private Coroutine tempCoroutine;
    public void Init(PlayerController playerController, ObjectCreator objectCreator, UpgradeSystem upgradeSystem)
    {
        _playerController = playerController;
        _objectCreator = objectCreator;
        _upgradeSystem = upgradeSystem;

        upgradeSystem.OnEnergyLimitUpgraded += EnegryLimitUpgradePriceChange;
        upgradeSystem.OnEnergyRegenUpgraded += EnegryRegenUpgradePriceChange;
        upgradeSystem.OnEnergySpendUpgraded += EnegrySpendUpgradePriceChange;
        upgradeSystem.OnForceProductionUpgraded += ForceProductionUpgradePriceChange;
        upgradeSystem.OnForceGenerationUpgraded += ForceGenerationUpgradePriceChange;
        upgradeSystem.OnForceTimeUpgraded += ForceTimeUpgradePriceChange;
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
        experienceText.text = "EXP: " + playerController.experience.ToString();
        quarkCounterText.text = "Quark count: " + playerController.currentQuark;

        FinishedObjectsChange(0);
        StartCoroutine(PlayTimer());
    }
    private void Update()
    {
        
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
    }
    private void EnergyChange(float energyCur, float energyMax)
    {
        energyText.text = energyCur.ToString("0.0") + "/" + energyMax.ToString("0");
        energyBar.fillAmount = energyCur / energyMax;
    }
    private void ForceChange(float value)
    {
        forceCountText.text = "Force: " + value.ToString();
    }
    private void FinishedObjectsChange(int value)
    {
        finishedObjectsText.text = "Finished objects: " + value.ToString();
    }
    private void QuarkCounterChange(int count)
    {
        quarkCounterText.text = "Quark count: " + count;
    }
    #endregion
    #region Upgrades
    private void EnegryLimitUpgradePriceChange(int count, float exp)
    {
        energyLimitUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void EnegryRegenUpgradePriceChange(int count, float exp)
    {
        energyRegenUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void EnegrySpendUpgradePriceChange(int count, float exp)
    {
        energySpendUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ForceProductionUpgradePriceChange(int count, float exp)
    {
        forceProductionUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ForceGenerationUpgradePriceChange(int count, float exp)
    {
        forceGenerationUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ForceTimeUpgradePriceChange(int count, float exp)
    {
        forceTimeUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ProductionTimeUpgradePriceChange(int count, float exp)
    {
        productionTimeUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ProductionCountUpgradePriceChange(int count, float exp)
    {
        productionCountUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    private void ExperienceIncomeUpgradePriceChange(int count, float exp)
    {
        experienceIncomeUpgradePriceText.text = "Exp: " + count;
        ExperinceChange(exp);
    }
    #endregion
    private void Production(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)
    {
        //ProductionBarFill(time, currentQuark, expCur, expCount, purchasedQuarks);
        
        if(tempCoroutine != null)
        {
            Debug.Log("Production bar filler stopped");
            StopCoroutine(tempCoroutine);
            tempCoroutine = null;
        }
        
        Debug.Log("Production bar filler started");
        tempCoroutine = StartCoroutine(ProductionBarFiller(time, currentQuark, expCur, expCount, purchasedQuarks));
    }
    

    private IEnumerator ProductionBarFiller(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)
    {
        float productionTimer = 0;
        Debug.Log("Prod timer: " + productionTimer + "Prod time: " + time);

        while (productionTimer < time)
        {
            productionTimer += Time.deltaTime;
            productionBar.fillAmount = productionTimer / time;
            if (productionTimer >= time)
            {
                productionBar.fillAmount = 0;
                OnProductionFinished(purchasedQuarks.Count, currentQuark, expCur, purchasedQuarks);
                Debug.Log("Prod timer: " + productionTimer + "Prod time: " + time);
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
}
