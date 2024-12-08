using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractInfo : MonoBehaviour
{
    public Action<ContractData> OnContractStarted;

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
        nextContractButton.onClick.AddListener(() => NextContract(cd));
        prevContractButton.onClick.AddListener(() => PrevContract(cd));
        cancelButton.onClick.AddListener(Cancel);

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
    private void NextContract(ContractData cd)
    {

    }
    private void PrevContract(ContractData cd)
    {

    }
    private void DeleteContract(ContractData cd)
    {

    }
    private void Cancel()
    {
        gameObject.SetActive(false);
    }

}
