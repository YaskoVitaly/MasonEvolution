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
        experienceText.text = "EXP: " + playerController.experience.ToString();
        quarkCounterText.text = "Quark count: " + playerController.currentQuark;

        FinishedObjectsChange(0);
        StartCoroutine(PlayTimer());

        CheckUpgradeButtonPrice(_playerController.experience, energyLimitUpgradeButton, _upgradeSystem.energyMaxUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, energyRegenUpgradeButton, _upgradeSystem.energyRegUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, energySpendUpgradeButton, _upgradeSystem.energySpendUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceProductionUpgradeButton, _upgradeSystem.forceProductionUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceGenerationUpgradeButton, _upgradeSystem.forceGenerationUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceSpendUpgradeButton, _upgradeSystem.forceSpendUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, productionTimeUpgradeButton, _upgradeSystem.productionTimeUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, productionCountUpgradeButton, _upgradeSystem.productionCountUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, experienceIncomeUpgradeButton, _upgradeSystem.expIncomeUpgradeCost);
    }
    private void Update()
    {
        
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
        CheckUpgradeButtonPrice(_playerController.experience, energyLimitUpgradeButton, _upgradeSystem.energyMaxUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, energyRegenUpgradeButton, _upgradeSystem.energyRegUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, energySpendUpgradeButton, _upgradeSystem.energySpendUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceProductionUpgradeButton, _upgradeSystem.forceProductionUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceGenerationUpgradeButton, _upgradeSystem.forceGenerationUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, forceSpendUpgradeButton, _upgradeSystem.forceSpendUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, productionTimeUpgradeButton, _upgradeSystem.productionTimeUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, productionCountUpgradeButton, _upgradeSystem.productionCountUpgradeCost);
        CheckUpgradeButtonPrice(_playerController.experience, experienceIncomeUpgradeButton, _upgradeSystem.expIncomeUpgradeCost);
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
        finishedObjectsText.text = "Finished objects: " + value.ToString();
    }
    private void QuarkCounterChange(int count)
    {
        quarkCounterText.text = "Quark count: " + count;
    }
    #endregion
    #region Upgrades
    private void EnegryLimitUpgradePriceChange(int count, float exp, float energyMax)
    {
        energyLimitUpgradePriceText.text = "Exp: " + count;
        energyLimitUpgradeDescriptionText.text = energyMax.ToString("0") + " => " + (energyMax*2).ToString("0");
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
    private void ForceSpendUpgradePriceChange(int count, float exp)
    {
        forceSpendUpgradePriceText.text = "Exp: " + count;
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
    private void Production(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)//¬ыводить врем€ до окончани€ производства
    {
        //ProductionBarFill(time, currentQuark, expCur, expCount, purchasedQuarks);
        
        if(tempCoroutine != null)
        {
            //Debug.Log("Production bar filler stopped");
            StopCoroutine(tempCoroutine);
            tempCoroutine = null;
        }
        
        //Debug.Log("Production bar filler started");
        tempCoroutine = StartCoroutine(ProductionBarFiller(time, currentQuark, expCur, expCount, purchasedQuarks));
    }
    

    private IEnumerator ProductionBarFiller(float time, int currentQuark, float expCur, float expCount, List<Quark> purchasedQuarks)
    {
        float productionTimer = 0;
        //Debug.Log("Prod timer: " + productionTimer + "Prod time: " + time);

        while (productionTimer < time)
        {
            productionTimer += Time.deltaTime;
            productionText.text = productionTimer.ToString("0.0") + "/" + time.ToString("0.0");
            productionBar.fillAmount = productionTimer / time;
            if (productionTimer >= time)
            {
                productionBar.fillAmount = 0;
                OnProductionFinished(purchasedQuarks.Count, currentQuark, expCur, purchasedQuarks);
                //Debug.Log("Prod timer: " + productionTimer + "Prod time: " + time);
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
