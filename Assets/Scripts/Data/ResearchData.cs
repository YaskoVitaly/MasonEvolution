using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchData", menuName = "Research/ResearchData", order = 1)]
public class ResearchData : ScriptableObject
{
    [System.Serializable]
    public class ResearchLevel
    {
        public int level;
        public int costCurrency;
        public int costExperience;
        public float timeRequired;
    }

    public string researchName;
    public List<ResearchLevel> levels = new List<ResearchLevel>();
}
