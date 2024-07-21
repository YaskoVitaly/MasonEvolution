using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public Action OnSchemeUpdated;
    public Action<float, List<Quark>> OnQuarkGenerated;

    private PlayerController _playerController;
    private ObjectScheme _objectScheme;
    private CoreUI _coreUI;
    public GameObject quark;
    
    
    public void Init(PlayerController playerController, ObjectScheme objectScheme, CoreUI coreUI)
    {
        _playerController = playerController;
        _objectScheme = objectScheme;
        _coreUI = coreUI;
        _coreUI.OnProductionFinished += QuarkGeneration;
        _playerController.OnObjectCompleted += SchemeUpdate;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
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
        for(int i = 0; i < _objectScheme.quarksList.Count; i++)
        {
            _objectScheme.quarksList[i].GetComponent<MeshRenderer>().enabled = false;
        }
        Debug.Log("Scheme updated");
        OnSchemeUpdated();
    }
}
