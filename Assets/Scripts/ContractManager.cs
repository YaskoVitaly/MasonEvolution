using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    public List<ContractData> contracts;

    private ContractData currentContract;

    public Action<ContractData> OnContractSelected;

    public void SelectContract(int index)
    {
        if (index >= 0 && index < contracts.Count)
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
}
