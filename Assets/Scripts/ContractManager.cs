using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContractManager : MonoBehaviour
{
    public GameObject quarkPrefab;

    private MetaUI metaUI;
    private GlobalData globalData;
    private ContractData currentContract;

    public Action<ContractData> OnContractSelected;



    public void Init(MetaUI _metaUI, GlobalData _globalData, GameObject quark)
    {
        
        metaUI = _metaUI;
        globalData = _globalData;
        quarkPrefab = quark;
        //metaUI.OnContractSelected += SelectContract;
        InvokeRepeating(nameof(AddRandomContract), 0f, globalData.newContractTime);
    }

    public void AddRandomContract()
    {
        if (globalData.activeContracts.Count >= globalData.maxContracts) return;

        ContractData newContract = GenerateRandomContract();
        Debug.Log("Random contract generated: " + newContract.count);
        globalData.activeContracts.Add(newContract);

        if(metaUI != null)
        {
            CreateContractButton(newContract);
        }
    }

    private ContractData GenerateRandomContract()
    {
        return new ContractData
        {
            title = "Common contract",
            quark = quarkPrefab,
            count = UnityEngine.Random.Range(10, 20),
            sizeX = 4,
            sizeY = 2,
            sizeZ = 8,
            reward = 1
        };
    }
    private void CreateContractButton(ContractData contractData)
    {
        GameObject buttonObject = Instantiate(metaUI.contractButtonPrefab, metaUI.contractPanel);
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

    /*
    public void SelectContract(int index)
    {
        if (index >= 0 && index < globalData.activeContracts.Count)
        {
            currentContract = contracts[index];
            Debug.Log($"Выбран контракт: {currentContract.title}");
            OnContractSelected(currentContract);
            // Запускаем кор-геймплей
            //StartCoreGameplay();
        }
        else
        {
            Debug.LogError("Некорректный индекс контракта.");
        }
    }
    */
}
