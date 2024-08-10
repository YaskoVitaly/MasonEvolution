using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public Action OnSchemeUpdated;
    public Action<float, List<Quark>> OnQuarkGenerated;

    private ObjectScheme objectScheme;
    private GameObject quark;
    
    public void Init(PlayerController playerController, ObjectScheme _objectScheme, CoreUI coreUI, GameObject _quark)
    {
        objectScheme = _objectScheme;
        quark = _quark;
        coreUI.OnProductionFinished += QuarkGeneration;
        playerController.OnObjectCompleted += SchemeUpdate;
    }
    
    private void QuarkGeneration(int quarkCount, int currentQuark, float exp, List<Quark> purchasedQuarks)//переписать под список кварков
    {
        for (int i = 0; i < purchasedQuarks.Count; i++)
        {
            purchasedQuarks[i].GetComponent<MeshRenderer>().enabled = true;
        }
        OnQuarkGenerated(exp, purchasedQuarks);
    }
    private void SchemeUpdate(int compleatedObjects) //Создавать новые схемы, а не обновлять единственную.
    {
        for(int i = 0; i < objectScheme.quarksList.Count; i++)
        {
            objectScheme.quarksList[i].GetComponent<MeshRenderer>().enabled = false;
        }
        Debug.Log("Scheme updated");
        OnSchemeUpdated();
    }
}
