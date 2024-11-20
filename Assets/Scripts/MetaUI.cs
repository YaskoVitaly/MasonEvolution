using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetaUI : MonoBehaviour
{
    public Action<int> OnContractSelected;

    public GameObject contractButtonPrefab;
    public Transform contractPanel;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI experienceText;

    public List<GameObject> contractButtons;

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


}
