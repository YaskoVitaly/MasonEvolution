using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Action<int> OnObjectCompleted;
    public Action<float, int, float, float, List<Quark>> OnProductionStarted;
    public Action<float, float> OnEnergyChanged;
    public Action<float> OnWorked;
    public Action<float> OnExperienceChanged;

    public PlayerData _playerData;
    public CameraController _cam;
    public ObjectCreator _objectCreator;
    public ObjectScheme _objectScheme;
    public UpgradeSystem _upgradeSystem;
    public CoreUI _coreUI;

    public int currentQuark = 0;

    public float energyMax;
    public float energyCur;
    public float energyReg;
    public float workCost;
    public float forceProduction;
    public float forceCur;
    public float forceReg;
    public float experience;
    public float productionTime;

    public float energySpend = 1;
    public float forceTime = 5;
    public int productionCount = 1;
    public float experienceMult = 0.1f;
    public int completedObjects = 0;

    private bool isProduction = false;
    private Coroutine creator;
    private List<Quark> productionQuarks;

    private void Awake()
    {
        _cam = GetComponent<CameraController>();
        _objectCreator = GetComponent<ObjectCreator>();
        _objectScheme = GetComponent<ObjectScheme>();
        _upgradeSystem = GetComponent<UpgradeSystem>();
        _coreUI = GetComponent<CoreUI>();
        _coreUI.Init(this, _objectCreator, _upgradeSystem);
        _objectCreator.Init(this, _objectScheme, _coreUI);
        _objectScheme.Init(_objectCreator);
    }
    void Start()
    {
        _objectCreator.OnQuarkGenerated += ProductionCompleate;//переписать под список кварков
        _objectCreator.OnSchemeUpdated += Launcher;
        _cam.zeroPoint = new Vector3(_objectScheme.sizeX/2, _objectScheme.sizeY/2, _objectScheme.sizeZ/2);
        experience = 0;
        energyMax = 10;
        energyCur = energyMax;
        OnEnergyChanged(energyCur, energyMax);
        StartCoroutine(EnergyRegeneration());
        StartCoroutine(ForceGeneration());
        Launcher();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Work();
        }
    }
    private void Launcher()
    {
        if (creator != null)
        {
            StopCoroutine(creator);
        }
        Debug.Log("Launcher");
        creator = StartCoroutine(ObjectCreator(_objectScheme.CurrentQuarks(productionCount, currentQuark)));
    }
    public void ProductionCompleate(float exp, List<Quark> purchasedQuarks)//переписать под список кварков
    {
        Debug.Log("Production compeate");
        ExperinceChange(exp);
        currentQuark += purchasedQuarks.Count;
        if (currentQuark >= _objectScheme.quarksList.Count)
            currentQuark = _objectScheme.quarksList.Count-1;
        isProduction = false;
        Launcher();
    }
    private void ExperinceChange(float value)
    {
        experience += value;
        OnExperienceChanged(experience);
    }
    private List<Quark> QuarkListAssembly(List<Quark> selectedQuarks) //протестировать метод, убедиться, что все работает верно.
    {
        List<Quark> purchasedQuarks = new List<Quark>(); //создаю новый список кварков, для покупки
        int totalCost = 0; //создаю переменную, для общей стоимости кварков в списке
        for (int i = 0; i < selectedQuarks.Count; i++) //собираю список кварков из списка кварков
        {
            if(totalCost + selectedQuarks[i].cost <= forceCur) //проверяю возможность покупки
            {
                totalCost += selectedQuarks[i].cost; //прибавляю к общей стоимости цену очередного кварка
                purchasedQuarks.Add(selectedQuarks[i]);
                Debug.Log(totalCost);
            }
            else
            {
                Debug.Log("Not enough force!");
            }
        }
        return purchasedQuarks;
    }
    private float TotalCost(List<Quark> selectedQuarks)
    {
        int totalCost = 0;
        for(int i = 0;i < selectedQuarks.Count;i++)
        {
            totalCost += selectedQuarks[i].cost;
        }
        return totalCost;
    }
    private void ObjectCreate(List<Quark> selectedQuarks)//написать метод для создания кварков
    {
        float totalCost = TotalCost(selectedQuarks);
        
        if(selectedQuarks.Count > 0 && totalCost <= forceCur)
        {
            forceCur -= totalCost;
            OnWorked(forceCur);
            isProduction = true;
            OnProductionStarted(productionTime, currentQuark, totalCost * experienceMult, experience, selectedQuarks);
            if(creator != null)
                StopCoroutine(creator);
            Debug.Log("Current force: " + forceCur + " Total cost: " + totalCost);
        }
    }
    private IEnumerator ObjectCreator(List<Quark> currentQuarks)
    {
        while (currentQuark <= _objectScheme.quarksList.Count && !isProduction)
        {
            List<Quark> purchasedQuarks = QuarkListAssembly(currentQuarks);
            float totalCost = TotalCost(purchasedQuarks);
            Debug.Log("Total cost: " + totalCost);
            if (forceCur >= totalCost)
            {
                if (currentQuarks != null && currentQuark < _objectScheme.quarksList.Count)
                {
                    ObjectCreate(purchasedQuarks);
                }
                else if (currentQuark == _objectScheme.quarksList.Count)
                {
                    completedObjects++;
                    currentQuark = 0;
                    Debug.Log("Object compleated");
                    OnObjectCompleted(completedObjects);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator EnergyRegeneration()
    {
        while (true)
        {
            if (energyCur < energyMax)
            {
                energyCur += energyReg * Time.deltaTime;
                OnEnergyChanged(energyCur, energyMax);
                if (energyCur > energyMax)
                {
                    energyCur = energyMax;
                    OnEnergyChanged(energyCur, energyMax);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ForceGeneration()
    {
        while (true)
        {
            if (forceReg > 0 && energyCur >= forceReg*energySpend)
            {
                forceCur += forceReg;
                energyCur -= workCost * energySpend;
                OnWorked(forceCur);
            }
            yield return new WaitForSeconds(forceTime);
        }
    }
    public void Work()
    {
        if (energyCur >= workCost*energySpend)
        {
            energyCur -= workCost * energySpend;
            forceCur += forceProduction;
            OnWorked(forceCur);
        }
    }

}
