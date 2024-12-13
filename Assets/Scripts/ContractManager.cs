using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContractManager : MonoBehaviour
{
    private MetaUI metaUI;
    private GlobalData globalData;
    private ContractData currentContract;


    public Action<ContractData> OnContractGenerated;
    public Action<float> OnContractTimerUpdated;


    public void Init(MetaUI _metaUI, GlobalData _globalData)
    {
        metaUI = _metaUI;
        globalData = _globalData;
        if(globalData.timePeriod > 0)
        {
            globalData.nextContractTime += globalData.timePeriod;
        }

        StartCoroutine(ContractTimer());
        Debug.Log(globalData.activeContracts.Count);

        if (globalData.activeContracts.Count == 0)
        {
            ContractData contractData = globalData.possibleContracts[0];
            globalData.activeContracts.Add(contractData);
            OnContractGenerated(contractData);
        }
        else if (metaUI.contractButtons.Count < globalData.activeContracts.Count)
        {
            Debug.Log("You have " +  globalData.activeContracts.Count + " contracts");
            GenerateContractButtons();
        }
    }
    public void UpdateMetaUI(MetaUI _metaUI)
    {
        metaUI = _metaUI;
        if(metaUI.contractButtons.Count < globalData.activeContracts.Count)
        {
            GenerateContractButtons();
        }
    }
    public void GenerateContractButtons()
    {
        foreach (ContractData cd in globalData.activeContracts)
        {
            Debug.Log("ContractGenerated");
            OnContractGenerated(cd);
        }
    }

    public void AddRandomContract()
    {
        if (globalData.activeContracts.Count < globalData.maxContracts)
        {
            ContractData newContract = new ContractData();
            newContract = globalData.possibleContracts[UnityEngine.Random.Range(0, globalData.possibleContracts.Count)];
            Debug.Log("Active contacts: " + globalData.activeContracts.Count);

            Debug.Log("Random contract generated: " + newContract.title);
            globalData.activeContracts.Add(newContract);
            if (metaUI != null)
            {
                OnContractGenerated(newContract);
            }
            globalData.nextContractTime -= globalData.contractCooldown;
        }
        else
        {
            Debug.Log("Full contract panel");
        }
    }
    private IEnumerator ContractTimer()//Пофиксить генерацию лишнего контракта.
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (globalData.activeContracts.Count < globalData.maxContracts)
            {
                if (globalData.nextContractTime < globalData.contractCooldown)
                {
                    globalData.nextContractTime += Time.deltaTime;
                    if(metaUI != null)
                    {
                        OnContractTimerUpdated(globalData.nextContractTime);
                    }
                }
                else
                {
                    AddRandomContract();
                }
            }
            else
            {
                globalData.nextContractTime = 0;
                if(metaUI != null)
                {
                    OnContractTimerUpdated(-1);
                }
            }
            
        }
    }
    
    /*
    private void CreateContractButton(ContractData contractData)
    {
        GameObject buttonObject = Instantiate(metaUI.contractButtonPrefab, metaUI.contractPanel);
        Debug.Log(metaUI.contractPanel.transform.position);
        metaUI.contractButtons.Add(buttonObject);
        ContractButton contractButton = buttonObject.GetComponent<ContractButton>();
        UnityEngine.UI.Button button = buttonObject.GetComponent<UnityEngine.UI.Button>();
        ContractData localContract = contractData;
        contractButton.Init(localContract);
        button.onClick.AddListener(() => SelectContract(localContract, buttonObject));
    }
    
    public void SelectContract(ContractData contractData, GameObject buttonObject)
    {
        OnContractSelected(contractData);
        Debug.Log("Button pressed");

        
        Destroy(buttonObject);
        globalData.activeContracts.Remove(contractData);
        globalData.currentContract = contractData;
        
    }
    */
}
