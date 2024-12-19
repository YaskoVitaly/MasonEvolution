using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ResearchData;

[CreateAssetMenu(fileName = "ContractData", menuName = "Contract/ContractData", order = 2)]
[Serializable]
public class ContractData : ScriptableObject
{
    public string type;
    public string title;
    public string description;
    public GameObject quark;
    public int count;
    public int sizeX;
    public int sizeY;
    public int sizeZ;
    public int reward;
}
