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

    [SerializeField]
    private PlayerData playerData;

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

            contractManager.OnContractSelected += LoadCoreScene;
            metaUI.Init(globalData, researchSystem, contractManager);
            contractManager.Init(metaUI, globalData);
            researchSystem.Init(globalData, metaUI);
            isFirstStart = false;
        }
        else
        {
            metaUI.Init(globalData, researchSystem, contractManager);
            contractManager.UpdateMetaUI(metaUI);
            researchSystem.UpdateMetaUI(metaUI);
        }
    }

    private void CoreInit()
    {
        Quark quark = quarkPrefab.GetComponent<Quark>();

        PlayerDataInit();

        playerController = gameObject.AddComponent<PlayerController>();
        objectScheme = gameObject.AddComponent<ObjectScheme>();
        objectCreator = gameObject.AddComponent<ObjectCreator>();
        cameraController = gameObject.AddComponent<CameraController>();
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        coreUI = FindObjectOfType<CoreUI>();

        coreUI.Init(playerController, playerData, objectCreator, upgradeSystem);
        objectScheme.Init(quarkPrefab, productSizeX, productSizeY, productSizeZ); //переработать схему. Должны быть схемы на выбор.
        playerController.Init(playerData, objectCreator, objectScheme);
        objectCreator.Init(playerController, objectScheme, coreUI, quarkPrefab);
        upgradeSystem.Init(playerData, objectCreator, coreUI);
        cameraController.Init(Camera.main, new Vector3(productSizeX/2 * quark.size, productSizeY/2 * quark.size, productSizeZ/2 * quark.size));//Откорректировать фокус камеры. Добавить управление камерой (вращение вокруг объекта, приближение/отдаление).

        playerController.OnContractCompleated += LoadMetaScene;
        coreUI.OnMetaLoaded += LoadMetaScene;
        
        Debug.Log("CoreInit");
    }

    private void PlayerDataInit()
    {
        playerData = new PlayerData();

        playerData.forceCur = 0;
        playerData.expCur = 0;
        playerData.expTotal = 0;
        playerData.completedObjects = 0;
        playerData.currentContract = globalData.currentContract;

        playerData.upgradeLevels["EnergyLimit"] = globalData.researchLevels["EnergyLimit"];
        playerData.upgradeLevels["EnergyRegeneration"] = globalData.researchLevels["EnergyRegeneration"];
        playerData.upgradeLevels["EnergySpend"] = globalData.researchLevels["EnergySpend"];
        playerData.upgradeLevels["ForceProduction"] = globalData.researchLevels["ForceProduction"];
        playerData.upgradeLevels["ForceGeneration"] = globalData.researchLevels["ForceGeneration"];
        playerData.upgradeLevels["ForceSpend"] = globalData.researchLevels["ForceSpend"];
        playerData.upgradeLevels["ProductionSpeed"] = globalData.researchLevels["ProductionSpeed"];
        playerData.upgradeLevels["ProductionCount"] = globalData.researchLevels["ProductionCount"];
        playerData.upgradeLevels["ExperienceMult"] = globalData.researchLevels["ExperienceMult"];

        playerData.energyMax = playerData.energyMaxBasic * (playerData.upgradeLevels["EnergyLimit"] + 1);
        playerData.energyCur = playerData.energyMax;
        playerData.energyReg = playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"];

        if(playerData.upgradeLevels["EnergySpend"] > 0)
            playerData.energySpend = playerData.energySpendBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["EnergySpend"]);
        else 
            playerData.energySpend = playerData.energySpendBasic;
        
        if(playerData.upgradeLevels["ForceProduction"] > 0)
        {
            playerData.forceProd = playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"];
            playerData.workCost = (playerData.workCostBasic * playerData.forceProd) * playerData.energySpend;
        }
        


        if (playerData.upgradeLevels["ForceGeneration"] == 0)
            playerData.forceReg = 0;
        else if (playerData.upgradeLevels["ForceGeneration"] == 1)
            playerData.forceReg = playerData.forceRegBasic;
        else
            playerData.forceReg = playerData.forceRegBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ForceGeneration"]);

        if (playerData.upgradeLevels["ForceSpend"] > 0)
            playerData.forceSpend = playerData.forceSpendBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ForceSpend"]);
        else
            playerData.forceSpend = playerData.forceSpendBasic;

        

        if (playerData.upgradeLevels["ProductionSpeed"] > 0)
            playerData.productionTime = playerData.productionTimeBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ProductionSpeed"]);
        else
            playerData.productionTime = playerData.productionTimeBasic;

        playerData.productionCount = playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"];

        if (playerData.upgradeLevels["ExperienceMult"] > 0)
            playerData.experienceMult = playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"]);
        else
            playerData.experienceMult = playerData.experienceMultBasic;
    }

    public void SaveData()
    {
        globalData.ConvertDictionary();
        string json = JsonUtility.ToJson(globalData);
        File.WriteAllText(Application.persistentDataPath + "/globalData.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/globalData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            globalData = JsonUtility.FromJson<GlobalData>(json);
            globalData.GetDictionary();
            Debug.Log("GlobalData loaded: " + path);
        }
        else if (globalData == null)
        {
            globalData = new GlobalData();
            globalData.GetDictionary();
            Debug.Log("GlobalData new");
        }
    }
    public void LoadCoreScene()
    {
        SaveData();
        metaUI.Unsubscribe();
        Destroy(metaUI.gameObject);
        SceneManager.LoadScene("CoreGamePlayScene");
    }

    public void LoadMetaScene(float exp)
    {
        globalData.totalExperience += exp;
        globalData.money += globalData.currentContract.reward;
        globalData.currentContract = null;

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
    public void ApplicationQuit()
    {
        SaveData();
        Application.Quit();
    }
}
