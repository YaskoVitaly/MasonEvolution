using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public int totalExperience;
    public int money;
    public Dictionary<string, int> resources = new Dictionary<string, int>(); // ��������� � �� ����������
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>(); // ������ ���������

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
