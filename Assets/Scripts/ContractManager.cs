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
            //CreateContractButton(contractData);
        }
        else
        {
            GenerateContractButtons();
        }
    }
    public void UpdateMetaUI(MetaUI _metaUI)
    {
        metaUI = _metaUI;
        GenerateContractButtons();
    }
    public void GenerateContractButtons()
    {
        foreach (ContractData cd in globalData.activeContracts)
        {
            OnContractGenerated(cd);
            //CreateContractButton(cd);
        }
    }

    public void AddRandomContract()
    {
        if (globalData.activeContracts.Count >= globalData.maxContracts)
        {
            Debug.Log("Full contract panel");
        }
        else
        {
            ContractData newContract = new ContractData();
            newContract = globalData.possibleContracts[UnityEngine.Random.Range(0, globalData.possibleContracts.Count)];
            Debug.Log("Random contract generated: " + newContract.title);
            globalData.activeContracts.Add(newContract);
            if (metaUI != null)
            {
                OnContractGenerated(newContract);
                //CreateContractButton(newContract);
            }
            globalData.nextContractTime -= globalData.contractCooldown;
        }
    }
    private IEnumerator ContractTimer()
    {
        while (true)
        {
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
            yield return new WaitForSeconds(Time.deltaTime);
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
