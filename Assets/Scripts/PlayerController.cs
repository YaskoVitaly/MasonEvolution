using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int> OnObjectCompleted;
    public Action<float, int, float, float, List<Quark>> OnProductionStarted;
    public Action<float, float> OnEnergyChanged;
    public Action<float> OnWorked;
    public Action<float> OnExperienceChanged;
    public Action<float, bool> OnContractCompleated;

    public PlayerData playerData;
    public ObjectScheme objectScheme;

    public int currentQuark = 0;
    public int completedObjects = 0;

    private bool isProduction = false;

    private Coroutine creator;
    private List<Quark> productionQuarks;
    
    public void Init(PlayerData _playerData, ObjectCreator _objectCreator, ObjectScheme _objectScheme)
    {
        playerData = _playerData;
        objectScheme = _objectScheme;
        _objectCreator.OnQuarkGenerated += ProductionCompleate;
        _objectCreator.OnSchemeUpdated += Launcher;

        OnEnergyChanged(playerData.energyCur, playerData.energyMax);
        StartCoroutine(EnergyRegeneration());
        StartCoroutine(ForceGeneration());
        Launcher();
        Debug.Log("PlayerController init");
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
        Debug.Log("Current quark: " + currentQuark + "; Object quark count: " + objectScheme.quarksList.Count + "; Production: " + isProduction);

        creator = StartCoroutine(ObjectCreator(objectScheme.CurrentQuarks(playerData.productionCount, currentQuark)));
    }
    public void ProductionCompleate(float exp, List<Quark> purchasedQuarks)//переписать под список кварков
    {
        if(purchasedQuarks.Last() != objectScheme.quarksList.Last())
        {
            Debug.Log("Production compleate");
            ExperinceChange(exp);
            currentQuark += purchasedQuarks.Count;
            isProduction = false;
            Launcher();
        }
        else
        {
            ExperinceChange(exp);
            ProductCompleate();
        }
    }
    private void ProductCompleate()
    {
        if (completedObjects < playerData.currentContract.count-1)
        {
            ExperinceChange(TotalCost(objectScheme.quarksList) * playerData.experienceMult);
            completedObjects++;
            currentQuark = 0;
            Debug.Log("Object compleated");
            isProduction = false;
            OnObjectCompleted(completedObjects);
        }
        else
        {
            ExperinceChange(TotalCost(objectScheme.quarksList) * playerData.experienceMult);
            completedObjects++;
            currentQuark = 0;
            Debug.Log("Object compleated");
            isProduction = false;
            OnObjectCompleted(completedObjects);
            OnContractCompleated(playerData.expCur/100, true);
        }
        
    }
    private void ExperinceChange(float value)
    {
        playerData.expCur += value;
        OnExperienceChanged(playerData.expCur);
    }
    private List<Quark> QuarkListAssembly(List<Quark> selectedQuarks) //протестировать метод, убедиться, что все работает верно.
    {
        List<Quark> purchasedQuarks = new List<Quark>(); //создаю новый список кварков, для покупки
        float totalCost = 0; //создаю переменную, для общей стоимости кварков в списке
        for (int i = 0; i < selectedQuarks.Count; i++) //собираю список кварков из списка кварков
        {
            if(totalCost*playerData.forceSpend + selectedQuarks[i].cost* playerData.forceSpend <= playerData.forceCur) //проверяю возможность покупки
            {
                totalCost += selectedQuarks[i].cost * playerData.forceSpend; //прибавляю к общей стоимости цену очередного кварка
                purchasedQuarks.Add(selectedQuarks[i]);
            }
            else
            {
                //Debug.Log("Not enough force!");
            }
        }
        Debug.Log("QuarkListAssembly: " + " Total cost: " + totalCost * playerData.forceSpend + "; Purchased quarks count: " + purchasedQuarks.Count);
        return purchasedQuarks;
    }
    private float TotalCost(List<Quark> selectedQuarks)
    {
        float totalCost = 0;
        for(int i = 0;i < selectedQuarks.Count;i++)
        {
            totalCost += selectedQuarks[i].cost;
        }
        return totalCost;
    }
    private void ObjectCreate(List<Quark> purchasedQuarks)
    {
        float totalCost = TotalCost(purchasedQuarks);
        
        if(purchasedQuarks.Count > 0 && totalCost * playerData.forceSpend <= playerData.forceCur)
        {
            Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost* playerData.forceSpend + " - Before deducting");
            playerData.forceCur -= totalCost * playerData.forceSpend;
            OnWorked(playerData.forceCur);
            isProduction = true;

            OnProductionStarted(playerData.productionTime, currentQuark, totalCost * playerData.experienceMult, playerData.expCur, purchasedQuarks);
            Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost * playerData.forceSpend + " - After deducting" + "; Production time: " + playerData.productionTime);
            /*
            if (playerData.forceCur < 100)
            {
                OnProductionStarted(playerData.productionTime, currentQuark, totalCost * playerData.experienceMult, playerData.expCur, purchasedQuarks);
                Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost * playerData.forceSpend + " - After deducting" + "; Production time: " + playerData.productionTime);
            }
            else if(playerData.forceCur >= 100 && playerData.forceCur < 1000)
            {
                OnProductionStarted(playerData.productionTime / 1.25f, currentQuark, totalCost * playerData.experienceMult, playerData.expCur, purchasedQuarks);
                Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost * playerData.forceSpend + " - After deducting" + "; Production time: " + playerData.productionTime / 1.25f);
            }
            else if(playerData.forceCur >= 1000 && playerData.forceCur < 10000)
            {
                OnProductionStarted(playerData.productionTime / 1.5f, currentQuark, totalCost * playerData.experienceMult, playerData.expCur, purchasedQuarks);
                Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost * playerData.forceSpend + " - After deducting" + "; Production time: " + playerData.productionTime / 1.5f);
            }
            else if(playerData.forceCur >= 10000)
            {
                OnProductionStarted(playerData.productionTime / 2, currentQuark, totalCost * playerData.experienceMult, playerData.expCur, purchasedQuarks);
                Debug.Log("Current force: " + playerData.forceCur + " Total cost: " + totalCost * playerData.forceSpend + " - After deducting" + "; Production time: " + playerData.productionTime / 2);
            }
            */
            if (creator != null)
                StopCoroutine(creator);
        }
    }
    private IEnumerator ObjectCreator(List<Quark> currentQuarks)
    {
        while (currentQuark <= objectScheme.quarksList.Count && !isProduction)
        {
            if(currentQuarks.Count > 0 && playerData.forceCur >= currentQuarks[0].cost)
            {
                List<Quark> purchasedQuarks = QuarkListAssembly(currentQuarks);
                float totalCost = TotalCost(purchasedQuarks);
                Debug.Log("Total cost: " + totalCost);
                if (playerData.forceCur >= totalCost * playerData.forceSpend)
                {
                    if (purchasedQuarks != null && currentQuark <= objectScheme.quarksList.Count)
                    {
                        ObjectCreate(purchasedQuarks);
                    }
                    else
                        Debug.LogWarning("Player controller can't create");
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator EnergyRegeneration()
    {
        while (true)
        {
            if (playerData.energyCur < playerData.energyMax)
            {
                playerData.energyCur += playerData.energyReg * Time.deltaTime;
                OnEnergyChanged(playerData.energyCur, playerData.energyMax);
                if (playerData.energyCur > playerData.energyMax)
                {
                    playerData.energyCur = playerData.energyMax;
                    OnEnergyChanged(playerData.energyCur, playerData.energyMax);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ForceGeneration()
    {
        while (true)
        {
            if (playerData.forceReg > 0 && playerData.energyCur >= playerData.workCost * playerData.energySpend)
            {
                Work();
            }
            yield return new WaitForSeconds(playerData.forceReg);
        }
    }
    public void Work()
    {
        if (playerData.energyCur >= playerData.workCost * playerData.energySpend)
        {
            playerData.energyCur -= playerData.workCost * playerData.energySpend;
            playerData.forceCur += playerData.forceProd;
            OnWorked(playerData.forceCur);
        }
    }

}
