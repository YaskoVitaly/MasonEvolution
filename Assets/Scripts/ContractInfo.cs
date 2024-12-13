using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractInfo : MonoBehaviour
{
    public Action<ContractData> OnContractStarted;
    public Action<ContractData> OnContractDeleted;
    public Action<ContractData, bool> OnNextContractSelected;
    public Action<ContractData, bool> OnPreviousContractSelected;

    private MetaUI metaUI;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public Button startContractButton;
    public Button nextContractButton;
    public Button prevContractButton;
    public Button deleteContractButton;
    public Button cancelButton;

    public void Init(ContractData cd, MetaUI _metaUI)
    {
        metaUI = _metaUI;
        titleText.text = cd.title;
        descriptionText.text = cd.description;
        rewardText.text = cd.reward.ToString();
        startContractButton.onClick.AddListener(() => StartContract(cd));
        deleteContractButton.onClick.AddListener(() => DeleteContract(cd));
        nextContractButton.onClick.AddListener(() => NextContract(cd, true));
        prevContractButton.onClick.AddListener(() => PrevContract(cd, false));
        cancelButton.onClick.AddListener(Cancel);
    }

    public void UpdateData(ContractData cd)
    {
        startContractButton.onClick.RemoveAllListeners();
        deleteContractButton.onClick.RemoveAllListeners();
        prevContractButton.onClick.RemoveAllListeners();
        nextContractButton.onClick.RemoveAllListeners();

        titleText.text = cd.title;
        descriptionText.text = cd.description;
        rewardText.text = cd.reward.ToString();

        startContractButton.onClick.AddListener(() => StartContract(cd));
        deleteContractButton.onClick.AddListener(() => DeleteContract(cd));
        nextContractButton.onClick.AddListener(() => NextContract(cd, true));
        prevContractButton.onClick.AddListener(() => PrevContract(cd, false));
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void StartContract(ContractData cd)
    {
        OnContractStarted(cd);
        Destroy(gameObject);
    }
    private void NextContract(ContractData cd, bool flag)
    {
        OnNextContractSelected(cd, true);
    }
    private void PrevContract(ContractData cd, bool flag)
    {
        OnNextContractSelected(cd, false);
    }
    private void DeleteContract(ContractData cd)
    {
        OnContractDeleted(cd);
    }
    private void Cancel()
    {
        gameObject.SetActive(false);
    }

}
