using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractButton : MonoBehaviour
{
    public void Init(ContractData cd)
    {
        TextMeshProUGUI[] textList = GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < textList.Length; i++)
        {
            if (textList[i].name == "Title")
            {
                textList[i].text = cd.title;
            }
            if (textList[i].name == "Description")
            {
                textList[i].text = "Create " + cd.count + " stone blocks " + cd.sizeY + "x" + cd.sizeX + "x" + cd.sizeZ;
            }
            if (textList[i].name == "Reward")
            {
                textList[i].text = "Reward: " + cd.reward * cd.count;
            }
        }
    }
}
