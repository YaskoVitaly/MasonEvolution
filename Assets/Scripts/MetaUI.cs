using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    public Action<int> OnContractSelected;
    public Action<string> OnResearchSelected;

    public GameObject researchPanel;
    public Button researchOpenButton;

    public Button energyLimitResearchButton;
    public Button energyRegenResearchButton;
    public Button energySpendResearchButton;
    public Button forceProductionResearchButton;
    public Button forceGenerationResearchButton;
    public Button forceSpendResearchButton;
    public Button productionTimeResearchButton;
    public Button productionCountResearchButton;
    public Button experienceIncomeResearchButton;

    public GameObject contractButtonPrefab;
    public Transform contractPanel;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI experienceText;

    public List<GameObject> contractButtons;
    public List<Button> researchButtons;
    
    public void Init(int money, float exp)
    {

        moneyText.text = "Money: " + money;
        experienceText.text = "Exp: " + exp;
        Debug.Log("Money: " + money + "; " + "Exp: " + exp);
    }

    public void SelectContract(int index)
    {
        OnContractSelected(index);
    }

    public void SelectResearch(string name)
    {
        OnResearchSelected(name);
    }
    public void OpenResearchPanel()
    {
        if (!researchPanel.activeSelf)
        {
            researchPanel.SetActive(true);
            researchOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 N Icon");

        }
        else
        {
            researchPanel.SetActive(false);
            researchOpenButton.image.sprite = Resources.Load<Sprite>("Sprites/UI/Free Flat Arrow 1 S Icon");
        }
    }

}
