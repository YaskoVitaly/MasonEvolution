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
    private bool isGenerated = false;

    public Action<ContractData> OnContractSelected;

    public void Init(MetaUI _metaUI, GlobalData _globalData)
    {
        metaUI = _metaUI;
        
        if(!isGenerated)
        {
            globalData = _globalData;
            InvokeRepeating(nameof(AddRandomContract), globalData.newContractTime, globalData.newContractTime);
            if(globalData.activeContracts.Count < globalData.maxContracts)
            {
                CreateContractButton(globalData.possibleContracts[0]);
            }
            isGenerated = true;
        }
        else
        {
            GenerateContractButtons();
        }
    }

    public void GenerateContractButtons()
    {
        foreach (ContractData cd in globalData.activeContracts)
        {
            CreateContractButton(cd);
        }
    }

    public void AddRandomContract()
    {
        if (globalData.activeContracts.Count >= globalData.maxContracts) return;

        ContractData newContract = new ContractData();
        newContract = globalData.possibleContracts[UnityEngine.Random.Range(0, globalData.possibleContracts.Count)];
        Debug.Log("Random contract generated: " + newContract.title);
        globalData.activeContracts.Add(newContract);
        if(metaUI != null)
        {
            CreateContractButton(newContract);
        }
    }
    
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
        Destroy(buttonObject);
        globalData.activeContracts.Remove(contractData);
        OnContractSelected(contractData);
    }
}
