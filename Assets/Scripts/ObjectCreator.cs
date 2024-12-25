using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public Action OnSchemeUpdated;
    public Action<float, List<Quark>> OnQuarkGenerated;

    private ObjectScheme objectScheme;
    
    public void Init(PlayerController playerController, ObjectScheme _objectScheme, CoreUI coreUI, PlayerData _playerData)
    {
        objectScheme = _objectScheme;
        coreUI.OnProductionFinished += QuarkGeneration;
        playerController.OnObjectCompleted += SchemeUpdate;
        if(_playerData.currentQuark > 0)
        {
            for(int i = 0; i < _playerData.currentQuark; i++)
            {
                objectScheme.quarksList[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        Debug.Log("ObjectCreator init");
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
