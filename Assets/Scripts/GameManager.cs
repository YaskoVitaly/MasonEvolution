using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    [SerializeField]
    private GlobalData globalData;

    //[SerializeField]
    //private PlayerData playerData;

    [SerializeField]
    private ContractData contractData;

    [SerializeField]
    private ContractManager contractManager;

    [SerializeField]
    private ResearchSystem researchSystem;

    [SerializeField]
    private PlayerController playerController;
    
    [SerializeField]
    private ObjectScheme objectScheme;
    
    [SerializeField]
    private ObjectCreator objectCreator;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private UpgradeSystem upgradeSystem;

    [SerializeField]
    private CoreUI coreUI;

    [SerializeField]
    private MetaUI metaUI;

    [SerializeField]
    private GameObject quarkPrefab;

    public int productSizeX;
    public int productSizeY;
    public int productSizeZ;

    private bool isFirstStart = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("InstanceCreate");
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
            Debug.Log("Gameobject destoed " + gameObject.name);
        }
    }
    private void Start()
    {
        if(!isFirstStart)
            isFirstStart = true;

        LoadData();
        SceneManager.activeSceneChanged += SceneCheck;
        if (SceneManager.GetActiveScene().name == "CoreGamePlayScene")
        {
            CoreInit();
            Debug.LogWarning("This scene is a CoreGamePlayScene" + gameObject.name);

        }
        else if (SceneManager.GetActiveScene().name == "MetaGamePlayScene")
        {
            MetaInit();
            Debug.LogWarning("This scene is a MetaGamePlayScene" + gameObject.name);
        }
    }

    private void MetaInit()
    {
        metaUI = FindObjectOfType<MetaUI>();
        if (isFirstStart)
        {
            if (contractManager == null)
                contractManager = gameObject.AddComponent<ContractManager>();

            if (researchSystem == null)
                researchSystem = gameObject.AddComponent<ResearchSystem>();

            metaUI.contractInfoPanel.GetComponent<ContractInfo>().OnContractStarted += LoadCoreScene;
            metaUI.OnContractContinued += LoadCoreScene;
            metaUI.Init(globalData, researchSystem, contractManager);
            contractManager.Init(metaUI, globalData);
            researchSystem.Init(globalData, metaUI);
            isFirstStart = false;
        }
        else
        {
            metaUI.contractInfoPanel.GetComponent<ContractInfo>().OnContractStarted += LoadCoreScene;
            metaUI.OnContractContinued += LoadCoreScene;
            metaUI.Init(globalData, researchSystem, contractManager);
            contractManager.UpdateMetaUI(metaUI);
            researchSystem.UpdateMetaUI(metaUI);
        }
    }

    private void CoreInit()
    {
        Quark quark = quarkPrefab.GetComponent<Quark>();
        //PlayerDataInit();
        
        if (globalData.playerData == null)
        {
            PlayerDataInit();
        }
        else
        {
            globalData.playerData.currentContract = globalData.currentContract;
            globalData.playerData.isContinuedContract = true;
        }
        
        playerController = gameObject.AddComponent<PlayerController>();
        objectScheme = gameObject.AddComponent<ObjectScheme>();
        objectCreator = gameObject.AddComponent<ObjectCreator>();
        cameraController = gameObject.AddComponent<CameraController>();
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        coreUI = FindObjectOfType<CoreUI>();

        coreUI.Init(playerController, globalData.playerData, objectCreator, upgradeSystem);
        objectScheme.Init(quarkPrefab, productSizeX, productSizeY, productSizeZ); //переработать схему. Должны быть схемы на выбор.
        playerController.Init(globalData.playerData, objectCreator, objectScheme);
        objectCreator.Init(playerController, objectScheme, coreUI, quarkPrefab);
        upgradeSystem.Init(globalData, objectCreator, coreUI);
        cameraController.Init(Camera.main, new Vector3(productSizeX/2 * quark.size, productSizeY/2 * quark.size, productSizeZ/2 * quark.size));//Откорректировать фокус камеры. Добавить управление камерой (вращение вокруг объекта, приближение/отдаление).

        playerController.OnContractCompleated += LoadMetaScene;
        coreUI.OnMetaLoaded += LoadMetaScene;
        
        Debug.Log("CoreInit");
    }
    
    private void PlayerDataInit()
    {
        globalData.playerData = new PlayerData();

        globalData.playerData.forceCur = 0;
        globalData.playerData.expCur = 0;
        globalData.playerData.expTotal = 0;
        globalData.playerData.completedObjects = 0;
        globalData.playerData.currentContract = globalData.currentContract;

        globalData.playerData.upgradeLevels["EnergyLimit"] = globalData.researchLevels["EnergyLimit"];
        globalData.playerData.upgradeLevels["EnergyRegeneration"] = globalData.researchLevels["EnergyRegeneration"];
        globalData.playerData.upgradeLevels["EnergySpend"] = globalData.researchLevels["EnergySpend"];
        globalData.playerData.upgradeLevels["ForceProduction"] = globalData.researchLevels["ForceProduction"];
        globalData.playerData.upgradeLevels["ForceGeneration"] = globalData.researchLevels["ForceGeneration"];
        globalData.playerData.upgradeLevels["ForceSpend"] = globalData.researchLevels["ForceSpend"];
        globalData.playerData.upgradeLevels["ProductionSpeed"] = globalData.researchLevels["ProductionSpeed"];
        globalData.playerData.upgradeLevels["ProductionCount"] = globalData.researchLevels["ProductionCount"];
        globalData.playerData.upgradeLevels["ExperienceMult"] = globalData.researchLevels["ExperienceMult"];

        globalData.playerData.energyMax = globalData.playerData.energyMaxBasic * (globalData.playerData.upgradeLevels["EnergyLimit"] + 1);
        globalData.playerData.energyCur = globalData.playerData.energyMax;
        globalData.playerData.energyReg = globalData.playerData.energyRegBasic + globalData.playerData.upgradeLevels["EnergyRegeneration"];

        if(globalData.playerData.upgradeLevels["EnergySpend"] > 0)
            globalData.playerData.energySpend = globalData.playerData.energySpendBasic * (float)Math.Pow(0.9f, globalData.playerData.upgradeLevels["EnergySpend"]);
        else 
            globalData.playerData.energySpend = globalData.playerData.energySpendBasic;
        
        if(globalData.playerData.upgradeLevels["ForceProduction"] > 0)
        {
            globalData.playerData.forceProd = globalData.playerData.forceProdBasic + globalData.playerData.upgradeLevels["ForceProduction"];
            globalData.playerData.workCost = (globalData.playerData.workCostBasic * globalData.playerData.forceProd) * globalData.playerData.energySpend;
        }
        


        if (globalData.playerData.upgradeLevels["ForceGeneration"] == 0)
            globalData.playerData.forceReg = 0;
        else if (globalData.playerData.upgradeLevels["ForceGeneration"] == 1)
            globalData.playerData.forceReg = globalData.playerData.forceRegBasic;
        else
            globalData.playerData.forceReg = globalData.playerData.forceRegBasic * (float)Math.Pow(0.9f, globalData.playerData.upgradeLevels["ForceGeneration"]);

        if (globalData.playerData.upgradeLevels["ForceSpend"] > 0)
            globalData.playerData.forceSpend = globalData.playerData.forceSpendBasic * (float)Math.Pow(0.9f, globalData.playerData.upgradeLevels["ForceSpend"]);
        else
            globalData.playerData.forceSpend = globalData.playerData.forceSpendBasic;

        

        if (globalData.playerData.upgradeLevels["ProductionSpeed"] > 0)
            globalData.playerData.productionTime = globalData.playerData.productionTimeBasic * (float)Math.Pow(0.9f, globalData.playerData.upgradeLevels["ProductionSpeed"]);
        else
            globalData.playerData.productionTime = globalData.playerData.productionTimeBasic;

        globalData.playerData.productionCount = globalData.playerData.productionCountBasic + globalData.playerData.upgradeLevels["ProductionCount"];

        if (globalData.playerData.upgradeLevels["ExperienceMult"] > 0)
            globalData.playerData.experienceMult = globalData.playerData.experienceMultBasic * (float)Math.Pow(1.2f, globalData.playerData.upgradeLevels["ExperienceMult"]);
        else
            globalData.playerData.experienceMult = globalData.playerData.experienceMultBasic;
    }

    public void SaveData()
    {
        globalData.ConvertDictionary();
        globalData.playerData.ConvertDictionary();
        globalData.globalTime = DateTime.Now.ToString();
        string json = JsonUtility.ToJson(globalData);
        File.WriteAllText(Application.persistentDataPath + "/globalData.json", json);
        Debug.Log("GlobalData saved");
        Debug.Log("GlobalTime: " + globalData.globalTime);

    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/globalData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            globalData = JsonUtility.FromJson<GlobalData>(json);
            globalData.GetDictionary();
            globalData.playerData.GetDictionary();
            TimeSpan timeSpan = DateTime.Now - DateTime.Parse(globalData.globalTime);
            globalData.timePeriod = timeSpan.Days * (3600*24) + timeSpan.Hours * 3600 + timeSpan.Minutes * 60 + timeSpan.Seconds;
            Debug.Log("GlobalData loaded: " + path);
            Debug.Log("Time period: " + globalData.timePeriod);

        }
        else if (globalData == null)
        {
            globalData = new GlobalData();
            globalData.GetDictionary();
            globalData.playerData.GetDictionary();
            Debug.Log("GlobalData new");
        }
    }
    public void LoadCoreScene(ContractData cd)
    {
        if(globalData.currentContract == null)
        {
            globalData.currentContract = cd;
            globalData.activeContracts.Remove(cd);
            SaveData();
            metaUI.Unsubscribe();
            metaUI.contractInfoPanel.GetComponent<ContractInfo>().OnContractStarted -= LoadCoreScene;
            metaUI.OnContractContinued -= LoadCoreScene;
            Destroy(metaUI.gameObject);
            SceneManager.LoadScene("CoreGamePlayScene");
        }
        else
        {
            metaUI.Unsubscribe();
            metaUI.contractInfoPanel.GetComponent<ContractInfo>().OnContractStarted -= LoadCoreScene;
            metaUI.OnContractContinued -= LoadCoreScene;
            Destroy(metaUI.gameObject);
            SceneManager.LoadScene("CoreGamePlayScene");
        }
        
    }

    public void LoadMetaScene(float exp, bool isCompleted)
    {
        if(isCompleted)
        {
            globalData.totalExperience += exp;
            globalData.money += globalData.currentContract.reward;
            globalData.currentContract = null;
        }
        
        
        SaveData();//Сделать окно награды за контракт, блокирующее интерфейс меты.

        playerController.OnContractCompleated -= LoadMetaScene;
        coreUI.OnMetaLoaded -= LoadMetaScene;

        Destroy(playerController);
        Destroy(objectScheme);
        Destroy(objectCreator);
        Destroy(cameraController);
        Destroy(upgradeSystem);
        Destroy(coreUI);

        SceneManager.LoadScene("MetaGamePlayScene");
    }
    private void SceneCheck(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().name == "CoreGamePlayScene")
        {
            CoreInit();
            Debug.LogWarning("This scene is a CoreGamePlayScene" + gameObject.name);

        }
        else if (SceneManager.GetActiveScene().name == "MetaGamePlayScene")
        {
            MetaInit();
            Debug.LogWarning("This scene is a MetaGamePlayScene" + gameObject.name);
        }
    }
}
